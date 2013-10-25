using System;
using System.Collections;
using LLVMSharp.Compiler;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.Semantic;

namespace lsc.Compiler.Semantic
{
    public class SemanticAnalysis
    {
        #region Properties

        private ErrorList _errors;
        private string _methodReturnValue = null;
        private TypeChecker typeChecker = null;
        private Hashtable _classHTable = null;
        private Hashtable _structHTable = null;
        private Hashtable _localvarHTable = new Hashtable();
        private AstClass AstClass = null;
        private AstStruct AstStruct = null;
        private bool inLoop = false;
        private AstSourceFileCollection _sourceFiles = null;
        private AstLocalVariableCollection _localvarslist = new AstLocalVariableCollection();

        #endregion


        /// <summary>
        /// This is the method that added the symbol table information
        /// to each block, if there is an error, it will update the errorslist
        /// </summary>
        /// <param name="errors"></param>
        public void CreateSymbolTableForAll(LLVMSharpCompiler Compiler, AstType astType)
        {
            _errors = Compiler.Errors;

            typeChecker = new TypeChecker(Compiler);

            _classHTable = Compiler.ClassHashtable;
            _structHTable = Compiler.StructHashtable;
            _sourceFiles = Compiler.AstProgram.SourceFiles;

            if (astType is AstClass)
                AstClass = (AstClass)astType;
            else if (astType is AstStruct)
                AstStruct = (AstStruct)astType;

            if (AstClass != null)
            {
                //Accessor
                foreach (AstAccessor aItem in AstClass.AstAccessorCollection)
                {
                    //if (aItem.AstGetAccessor == null || aItem.AstSetAccessor == null)
                    //{
                    //    ErrorInfo e = new ErrorInfo();
                    //    e.col = aItem.ColumnNumber;
                    //    e.fileName = aItem.Path;
                    //    e.line = aItem.LineNumber;
                    //    e.type = ErrorType.SymenticError;
                    //    e.message = "Accessor " + AstClass.FullQualifiedName + "." + aItem.Name + " must declare a body beacause it's not marked abstract. Automatically implemented properties must implement must define both get & set accessors";
                    //    Compiler.Errors.Add(e);
                    //    continue;
                    //}
                    if (aItem.AstGetAccessor != null)
                    {
                        _methodReturnValue = aItem.FullyQualifiedType;
                        if (aItem.AstGetAccessor.AstBlock != null)
                        {
                            CreateSymbolTable(_errors, aItem.AstGetAccessor.AstBlock, null);
                        }

                    }
                    if (aItem.AstSetAccessor != null)
                    {
                        if (aItem.AstSetAccessor.AstBlock != null)
                        {
                            CreateSymbolTable(_errors, aItem.AstSetAccessor.AstBlock, null);
                        }
                    }


                }

                //Constructors
                foreach (AstConstructor cItem in AstClass.AstConstructorCollection)
                {
                    CreateSymbolTable(_errors, cItem.AstBlock, cItem.Parameters);
                }

                //Method
                foreach (AstMethod mItem in AstClass.AstMethodCollection)
                {
                    _methodReturnValue = ((AstMethod)mItem).FullQReturnType;
                    _localvarslist.Clear();
                    CreateSymbolTable(_errors, mItem.AstBlock, mItem.Parameters);
                    mItem.LocalVariables = _localvarslist;
                }
            }
            else if (AstStruct != null)
            {
                //Accessor
                foreach (AstAccessor aItem in AstStruct.AstAccessorCollection)
                {
                    //if (aItem.AstGetAccessor == null || aItem.AstSetAccessor == null)
                    //{
                    //    ErrorInfo e = new ErrorInfo();
                    //    e.col = aItem.ColumnNumber;
                    //    e.fileName = aItem.Path;
                    //    e.line = aItem.LineNumber;
                    //    e.type = ErrorType.SymenticError;
                    //    e.message = "Accessor " + AstStruct.FullQualifiedName + "." + aItem.Name + " must declare a body beacause it's not marked abstract. Automatically implemented properties must implement must define both get & set accessors";
                    //    Compiler.Errors.Add(e);
                    //    continue;
                    //}
                    if (aItem.AstGetAccessor != null)
                    {
                        _methodReturnValue = aItem.FullyQualifiedType;
                        if (aItem.AstGetAccessor.AstBlock != null)
                        {
                            CreateSymbolTable(_errors, aItem.AstGetAccessor.AstBlock, null);
                        }

                    }
                    if (aItem.AstSetAccessor != null)
                    {
                        if (aItem.AstSetAccessor.AstBlock != null)
                        {
                            CreateSymbolTable(_errors, aItem.AstSetAccessor.AstBlock, null);
                        }
                    }

                }

                //Constructors
                foreach (AstConstructor cItem in AstStruct.AstConstructorCollection)
                {
                    CreateSymbolTable(_errors, cItem.AstBlock, cItem.Parameters);
                }

                //Method
                foreach (AstMethod mItem in AstStruct.AstMethodCollection)
                {
                    _methodReturnValue = ((AstMethod)mItem).FullQReturnType;

                    CreateSymbolTable(_errors, mItem.AstBlock, mItem.Parameters);
                }
            }

        }

        /// <summary>
        /// This method is common method for all to create Symbol Table
        /// </summary>
        /// <param name="_errors"></param>
        private void CreateSymbolTable(ErrorList _errors, AstBlock astBlock, AstParameterCollection astParams)
        {
            SymbolTableNode symTNode = new SymbolTableNode(_errors);

            symTNode.OpenScope();

            //Check parameters and add them to the first scope
            if (astParams != null)
            {
                foreach (AstParameter param in astParams)
                {
                    if (string.IsNullOrEmpty(param.FullQualifiedType))
                    {

                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type for parameter '" + param.Name + "' not found(are you missing a using directive or assembly reference?)", param.LineNumber, param.ColumnNumber, param.Path));
                        continue;
                    }
                    symTNode.Insert(param.Name, param.FullQualifiedType, param.IsArray, false,
                                    param.LineNumber, param.ColumnNumber, param.Path);
                }

            }

            if (astBlock != null)
                CreateBlockSymbol(symTNode, astBlock.AstStatementCollection);
            symTNode.CloseScope();
        }

        /// <summary>
        /// Helper Method that add local variable to symbol table
        /// It will iterate all the AstStatements and it will check for semantic 
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="_astBlock"></param>
        private void CreateBlockSymbol(SymbolTableNode symTNode, AstStatementCollection _statCol)
        {
            foreach (AstStatement sItem in _statCol)
            {
                if (sItem is AstLocalVariableDeclaration)
                {
                    AstLocalVariableDeclaration lvar = (AstLocalVariableDeclaration)sItem;
                    symTNode.Insert(lvar.Name, lvar.FullQualifiedType, lvar.IsArray,
                                    lvar.IsConstant, lvar.LineNumber, lvar.ColumnNumber,
                                    lvar.Path);
                    _localvarslist.Add(lvar);

                    CheckExpression(symTNode, lvar.Initialization);

                    //Check the initializer type
                    if (lvar.Initialization != null && lvar.Initialization.AssociatedType != "Unknown" &&
                        lvar.Initialization.AssociatedType != lvar.AssociatedType)
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                                        ((AstLocalVariableDeclaration)sItem).Initialization.AssociatedType +
                                        "' to '" + lvar.AssociatedType + "'", sItem.LineNumber, sItem.ColumnNumber, sItem.Path));
                    }
                }

                // Blocks
                else if (sItem is AstIfCondition) //For IF block
                {
                    //Check the Condition Expression
                    CheckExpression(symTNode, ((AstIfCondition)sItem).Condition);

                    //Check the type for Condition
                    if (((AstIfCondition)sItem).Condition.AssociatedType != "System.Boolean")
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                                    ((AstIfCondition)sItem).Condition.AssociatedType +
                                    "' to 'bool'", sItem.LineNumber, sItem.ColumnNumber, sItem.Path));
                    }

                    AstStatement _astStat = ((AstIfCondition)sItem).AstStatement;
                    if (_astStat is AstBlock)
                    {
                        symTNode.OpenScope();
                        CreateBlockSymbol(symTNode, ((AstBlock)_astStat).AstStatementCollection);
                        symTNode.CloseScope();
                    }
                    else if (_astStat is AstStatement) //if only one statement
                    {
                        AstStatementCollection astStatCol = new AstStatementCollection();
                        astStatCol.Add(_astStat);

                        symTNode.OpenScope();
                        CreateBlockSymbol(symTNode, astStatCol);
                        symTNode.CloseScope();
                    }

                    //Else Statement
                    AstStatement _astElseStat = ((AstIfCondition)sItem).AstStatementElse;

                    if (_astElseStat != null)
                    {
                        AstStatementCollection astElseStatCol;

                        if (_astElseStat is AstBlock)
                            astElseStatCol = ((AstBlock)_astElseStat).AstStatementCollection;
                        else
                        {
                            astElseStatCol = new AstStatementCollection();
                            astElseStatCol.Add(_astElseStat);
                        }

                        symTNode.OpenScope();
                        CreateBlockSymbol(symTNode, astElseStatCol);
                        symTNode.CloseScope();
                    }
                }
                else if (sItem is AstDoLoop) //For DO block
                {
                    AstStatement _astStat = ((AstDoLoop)sItem).AstStatement;

                    //Check the Condition Expression
                    CheckExpression(symTNode, ((AstDoLoop)sItem).Condition);
                    //Check the type for Condition
                    if (((AstDoLoop)sItem).Condition.AssociatedType != "System.Boolean")
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                                    ((AstDoLoop)sItem).Condition.AssociatedType +
                                    "' to 'bool'", sItem.LineNumber, sItem.ColumnNumber, sItem.Path));
                    }
                    if (_astStat is AstBlock)
                    {
                        symTNode.OpenScope();
                        inLoop = true;
                        CreateBlockSymbol(symTNode, ((AstBlock)_astStat).AstStatementCollection);
                        symTNode.CloseScope();
                        inLoop = false;
                    }

                }
                else if (sItem is AstWhileLoop)//For WHILE block
                {
                    AstStatement _astStat = ((AstWhileLoop)sItem).AstStatement;


                    //Check the Condition Expression
                    CheckExpression(symTNode, ((AstWhileLoop)sItem).Condition);
                    //Check the type for Condition
                    if (((AstWhileLoop)sItem).Condition.AssociatedType != "System.Boolean")
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                                    ((AstWhileLoop)sItem).Condition.AssociatedType +
                                    "' to 'bool'", sItem.LineNumber, sItem.ColumnNumber, sItem.Path));
                    }

                    if (_astStat is AstBlock)
                    {
                        symTNode.OpenScope();
                        inLoop = true;
                        CreateBlockSymbol(symTNode, ((AstBlock)_astStat).AstStatementCollection);
                        symTNode.CloseScope();
                        inLoop = false;
                    }
                    else if (_astStat is AstStatement) //if only one statement
                    {
                        AstStatementCollection astStatCol = new AstStatementCollection();
                        astStatCol.Add(_astStat);

                        symTNode.OpenScope();
                        inLoop = true;

                        CreateBlockSymbol(symTNode, astStatCol);
                        symTNode.CloseScope();
                        inLoop = false;
                    }
                }

                else if (sItem is AstForLoop)//For FOR block
                {

                    symTNode.OpenScope();
                    //Added the initializer first
                    CreateBlockSymbol(symTNode, ((AstForLoop)sItem).Initializers);

                    //Check the type for Condition, second
                    CheckExpression(symTNode, ((AstForLoop)sItem).Condition);
                    if (((AstForLoop)sItem).Condition != null && ((AstForLoop)sItem).Condition.AssociatedType != "System.Boolean")
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                                     ((AstForLoop)sItem).Condition.AssociatedType +
                                    "' to 'bool'", sItem.LineNumber, sItem.ColumnNumber, sItem.Path));
                    }

                    //Check the Increment, third
                    foreach (IAstExpression increExp in ((AstForLoop)sItem).IncrementExpressions)
                        CheckExpression(symTNode, increExp);

                    //..then add body
                    AstStatement _astStat = ((AstForLoop)sItem).Body;
                    if (_astStat is AstBlock)
                    {
                        inLoop = true;
                        CreateBlockSymbol(symTNode, ((AstBlock)_astStat).AstStatementCollection);
                        inLoop = false;
                    }
                    else if (_astStat is AstStatement) //if only one statement
                    {
                        AstStatementCollection astStatCol = new AstStatementCollection();
                        astStatCol.Add(_astStat);
                        inLoop = true;
                        CreateBlockSymbol(symTNode, astStatCol);
                        inLoop = false;
                    }
                    symTNode.CloseScope();
                }

                //Statments
                else if (sItem is AstReturn)
                {
                    CheckExpression(symTNode, ((AstReturn)sItem).AstExpression);
                    AstReturn r = (AstReturn)sItem;
                    if (_methodReturnValue == "System.Void")
                    {
                        if (r.AstExpression != null)
                        {
                            _errors.Add(new ErrorInfo(ErrorType.SymenticError,
                                                      "Since the method returns void a return keyword must not be followed by an object expression.",
                                        r.LineNumber, r.ColumnNumber, r.Path));
                        }
                    }
                    else
                    {
                        if (r.AstExpression == null)
                        {
                            _errors.Add(new ErrorInfo(ErrorType.SymenticError,
                                                      string.Format("An object of type convertible to {0} is required",
                                                                    _methodReturnValue),
                                                      r.LineNumber, r.ColumnNumber, r.Path));
                        }
                        else
                        {
                            if (((AstReturn)sItem).AssociatedType != _methodReturnValue &&
                                (!typeChecker.IsSubType(_methodReturnValue, ((AstReturn)sItem).AssociatedType))
                                )
                            {
                                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                                                                                   ((AstReturn)sItem).AssociatedType +
                                                                                   "' to '" + _methodReturnValue + "'",
                                                          sItem.LineNumber, sItem.ColumnNumber, sItem.Path));
                            }
                        }
                    }
                }

                else if (sItem is AstAssignmentStatement)
                {
                    CheckExpression(symTNode, ((AstAssignmentStatement)sItem).AstAssignmentExpression.LValue);
                    var lvar = ((AstAssignmentStatement)sItem).AstAssignmentExpression.LValue;

                    if (lvar is AstVariableReference && ((AstVariableReference)lvar).IsConst)
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The left-hand side of an assignment must be a variable, property or indexer",
                                       ((AstVariableReference)lvar).LineNumber, ((AstVariableReference)lvar).ColumnNumber, ((AstVariableReference)lvar).Path));
                    }
                    else
                    {

                        CheckExpression(symTNode, ((AstAssignmentStatement)sItem).AstAssignmentExpression.RValue);

                        //Check for Add/Div/Mul/Sub Assignment
                        if (!(((AstAssignmentStatement)sItem).AstAssignmentExpression is AstSimpleAssignmentExpression))
                        {
                            CheckAssignmentExpression(symTNode, ((AstAssignmentStatement)sItem).AstAssignmentExpression);
                        }

                        // Type Checking for LValue and RValue should perform here 
                        // Note: this consider AstAssignmentStatement has only two operand 
                        // The type checking for each assignment should already done by CheckAssignment method
                        AstAssignmentStatement asgStat = ((AstAssignmentStatement)sItem);
                        IAstExpression lvalue = asgStat.AstAssignmentExpression.LValue;
                        IAstExpression rvalue = asgStat.AstAssignmentExpression.RValue;
                        if (!asgStat.IsTypeMatch &&
                            (!typeChecker.IsSubType(lvalue.AssociatedType, rvalue.AssociatedType)) &&
                            (!CheckTConvert(asgStat.AstAssignmentExpression))
                            )
                        {
                            if (rvalue.AssociatedType != null &&
                                lvalue.AssociatedType != null &&
                                lvalue.AssociatedType != "Unknown" &&
                                rvalue.AssociatedType != "Unknown")
                                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                                            rvalue.AssociatedType + "' to '" + lvalue.AssociatedType + "'",
                                           sItem.LineNumber, sItem.ColumnNumber, sItem.Path));

                        }
                    }
                }

                else if (sItem is AstPreDecrement)
                {
                    CheckExpression(symTNode, ((AstPreDecrement)sItem).AstExpression);
                    //Check Operator Overload
                    CheckOpOverload(sItem);
                }

                else if (sItem is AstPreIncrement)
                {
                    CheckExpression(symTNode, ((AstPreIncrement)sItem).AstExpression);
                    //Check Operator Overload
                    CheckOpOverload(sItem);
                }

                else if (sItem is AstPostDecrement)
                {
                    CheckExpression(symTNode, ((AstPostDecrement)sItem).AstExpression);
                    //Check Operator Overload
                    CheckOpOverload(sItem);
                }

                else if (sItem is AstPostIncrement)
                {
                    CheckExpression(symTNode, ((AstPostIncrement)sItem).AstExpression);
                    //Check Operator Overload
                    CheckOpOverload(sItem);
                }

                else if (sItem is AstMethodCall)
                {
                    CheckMethodCall(symTNode, (AstMethodCall)sItem);
                }
                else if (sItem is AstBreak || sItem is AstContinue)
                {
                    CheckBreakContinue(symTNode, sItem);
                }
                else if (sItem is IAstExpression)
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Only assignment, call, increment, decrement, and new object expressions can be used as a statement",
                            sItem.LineNumber, sItem.ColumnNumber,
                            sItem.Path));
                }

            }
        }
        /// <summary>
        /// Check if break & continue are in the loop
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="sItem"></param>
        private void CheckBreakContinue(SymbolTableNode symTNode, AstStatement sItem)
        {
            if (!inLoop)
                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "No enclosing loop out of which to break or continue",
                            sItem.LineNumber, sItem.ColumnNumber,
                            sItem.Path));
        }

        private void CheckAssignmentExpression(SymbolTableNode symTNode, AstAssignmentExpression astAssignmentExpression)
        {
            // to do : check variable value
            //if (astAssignmentExpression.LValue.AssociatedType == "Unknown")
            //{
            //    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Use of unassigned local variable '"+ astAssignmentExpression.LValue.AssociatedType +"'",
            //                          astAssignmentExpression.LineNumber, astAssignmentExpression.ColumnNumber, astAssignmentExpression.Path));

            //}
        }


        /// <summary>
        /// Check for methods
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astMethodCall"></param>
        private void CheckMethodCall(SymbolTableNode symTNode, AstMethodCall astMethodCall)
        {
            //Check Expression for each Argument to have better type
            foreach (AstArgument astArg in astMethodCall.ArgumentCollection)
                CheckExpression(symTNode, astArg.AstExpression);

            //Do the member Method Call Check
            if (CheckLocalVarMemberRef(symTNode, astMethodCall)) return;

            if (astMethodCall.ArgumentCollection != null)
            {
                string[] args = GiveMeArguments(astMethodCall.ArgumentCollection);

                //Look in the method table
                var ret = MethodLookup(astMethodCall.Name, args);

                string param = String.Join(",", args);
                param = "(" + param + ")";

                if (ret == null)
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The function name '" + astMethodCall.Name +
                            "' with '" + param + "' arguments does not exist in the current context",
                            astMethodCall.LineNumber, astMethodCall.ColumnNumber,
                            astMethodCall.Path));
                }
                else
                {
                    astMethodCall.AssociatedType = ret.FullQReturnType;
                    astMethodCall.MemberRefCollection.Add(ret);
                }

            }

        }
        /// <summary>
        /// Helper function that give arguments in string array
        /// This is needed to call the method lookup
        /// </summary>
        /// <param name="astArgumentCollection"></param>
        /// <returns></returns>
        private string[] GiveMeArguments(AstArgumentCollection astArgumentCollection)
        {
            string[] args = new string[astArgumentCollection.Count];
            int i = 0;
            foreach (AstArgument item in astArgumentCollection)
                args[i++] = item.AssociatedType;

            return args;
        }

        /// <summary>
        /// It will look all the expression, categorize and do necesscary semantic checks
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="iExpr"></param>
        private void CheckExpression(SymbolTableNode symTNode, IAstExpression iExpr)
        {
            if (iExpr is AstVariableReference)
            {
                CheckVarReference(symTNode, (AstVariableReference)iExpr);
            }
            else if (iExpr is AstAssignmentExpression)
            {
                CheckAssignment(symTNode, (AstAssignmentExpression)iExpr);
            }
            else if (iExpr is AstBinaryExpression)
            {
                CheckBinaryExpression(symTNode, (AstBinaryExpression)iExpr);
            }
            else if (iExpr is AstIsExpression)
            {
                CheckExpression(symTNode, ((AstIsExpression)iExpr).AstExpression);
            }
            else if (iExpr is AstAsExpression)
            {
                CheckExpression(symTNode, ((AstAsExpression)iExpr).AstExpression);
            }
            else if (iExpr is AstPreDecrement)
            {
                CheckExpression(symTNode, ((AstPreDecrement)iExpr).AstExpression);
                //Check Operator Overload
                CheckOpOverload(iExpr);
            }
            else if (iExpr is AstPreIncrement)
            {
                CheckExpression(symTNode, ((AstPreIncrement)iExpr).AstExpression);
                //Check Operator Overload
                CheckOpOverload(iExpr);
            }
            else if (iExpr is AstPostDecrement)
            {
                CheckExpression(symTNode, ((AstPostDecrement)iExpr).AstExpression);
                //Check Operator Overload
                CheckOpOverload(iExpr);
            }
            else if (iExpr is AstPostIncrement)
            {
                CheckExpression(symTNode, ((AstPostIncrement)iExpr).AstExpression);
                //Check Operator Overload
                CheckOpOverload(iExpr);
            }
            else if (iExpr is AstNot)
            {
                CheckExpression(symTNode, ((AstNot)iExpr).AstExpression);
            }
            else if (iExpr is AstNewType)
            {
                CheckNewType(symTNode, (AstNewType)iExpr);
            }
            else if (iExpr is AstMemberReference)
            {
                //to do: Check for member reference
            }
            else if (iExpr is AstUnaryMinus)
            {
                CheckExpression(symTNode, ((AstUnaryMinus)iExpr).AstExpression);
            }
            else if (iExpr is AstUnaryPlus)
            {
                CheckExpression(symTNode, ((AstUnaryPlus)iExpr).AstExpression);
            }
            else if (iExpr is AstMethodCall)
            {
                CheckMethodCall(symTNode, (AstMethodCall)iExpr);
            }
            else if (iExpr is AstTypeCast)
            {
                CheckTypeCast(symTNode, (AstTypeCast)iExpr);
            }
            else if (iExpr is AstArrayInitialization)
            {
                //Array are not working yet
            }
        }

        /// <summary>
        /// Check for type cast
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astTypeCast"></param>
        private void CheckTypeCast(SymbolTableNode symTNode, AstTypeCast astTypeCast)
        {
            //Do something with astTypeCast.Type first

            //Then do the expression
            CheckExpression(symTNode, astTypeCast.AstExpression);
        }

        /// <summary>
        /// Check for new type 
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astNewType"></param>
        private void CheckNewType(SymbolTableNode symTNode, AstNewType astNewType)
        {
            //If it is array
            //NOTE: arrays are not working at the moment
            //If it is class type
            //Lookup the Constructor with the New Type, with the list of expression

            if (astNewType.AstArgumentCollection != null)
            {
                string[] args = new string[astNewType.AstArgumentCollection.Count];
                int i = 0;
                string arg = "'(";
                foreach (AstArgument item in astNewType.AstArgumentCollection)
                {
                    //Check Expression first
                    CheckExpression(symTNode, item.AstExpression);

                    if (i > 0)
                        arg += ",";

                    args[i++] = item.AssociatedType;

                    arg += item.AssociatedType;
                }

                if (i > 0)
                    arg += ")'";
                else
                {
                    arg = "'0' arguments";
                }

                var ret = ObjectConstructorLookup(astNewType.FullQualifiedType, astNewType.Type, args);
                if (ret == null)
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "'" +
                            (astNewType.FullQualifiedType == null ? astNewType.Type : astNewType.FullQualifiedType) +
                            "'" + " does not contain a constructor that takes " + arg,
                            astNewType.LineNumber, astNewType.ColumnNumber,
                            astNewType.Path));
                }
            }

        }

        /// <summary>
        /// This method will help NewType to look up the object constructor
        /// </summary>
        /// <param name="p"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private AstConstructor ObjectConstructorLookup(string fqt, string type, string[] args)
        {
            AstConstructor astConst = null;
            //If it is Class
            if (fqt != null)
            {
                if (_classHTable.ContainsKey(fqt))
                {

                    AstClass cItem = (AstClass)_classHTable[fqt];

                    astConst = cItem.AstConstructorLookup(type, args, false);

                }
                else if (_structHTable.ContainsKey(fqt))
                {
                    AstStruct sItem = (AstStruct)_structHTable[fqt];

                    astConst = sItem.AstConstructorLookup(type, args, false);
                }

            }

            return astConst;
        }

        /// <summary>
        /// Check for binary Expression
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astBinaryExpression"></param>
        private void CheckBinaryExpression(SymbolTableNode symTNode, AstBinaryExpression astBinaryExpression)
        {
            CheckExpression(symTNode, astBinaryExpression.LeftOperand);
            CheckExpression(symTNode, astBinaryExpression.RightOperand);

            // Type Checking for LeftOperand and RightOperand should perform here 
            IAstExpression roper = astBinaryExpression.RightOperand;
            IAstExpression loper = astBinaryExpression.LeftOperand;
            if ((IsBuiltinType(astBinaryExpression.AssociatedType) && !astBinaryExpression.IsTypeMatch) ||
                (!IsBuiltinType(astBinaryExpression.AssociatedType) && !CheckOpOverload(astBinaryExpression))
                )
            {
                if (roper.AssociatedType != null &&
                   loper.AssociatedType != null &&
                   loper.AssociatedType != "Unknown" &&
                   roper.AssociatedType != "Unknown")
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                            roper.AssociatedType + "' to '" + loper.AssociatedType + "'",
                            astBinaryExpression.LineNumber, astBinaryExpression.ColumnNumber,
                            astBinaryExpression.Path));
            }

        }

        private bool IsBuiltinType(string fqt)
        {
            return (fqt == "System.Object" ||
               fqt == "System.Boolean" ||
               fqt == "System.Char" ||
               fqt == "System.Int32" ||
               fqt == "System.Object" ||
               fqt == "System.Single" ||
               fqt == "System.String" ||
               fqt == "System.Void");
        }

        /// <summary>
        /// Check for Assignment Expression
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astAssignmentExpression"></param>
        private void CheckAssignment(SymbolTableNode symTNode, AstAssignmentExpression astAssignmentExpression)
        {
            CheckExpression(symTNode, astAssignmentExpression.RValue);
            CheckExpression(symTNode, astAssignmentExpression.LValue);

            // Type Checking for LValue and RValue should perform here             
            IAstExpression lvalue = astAssignmentExpression.LValue;
            IAstExpression rvalue = astAssignmentExpression.RValue;
            if (!astAssignmentExpression.IsTypeMatch &&
                (!typeChecker.IsSubType(lvalue.AssociatedType, rvalue.AssociatedType)) &&
                (!CheckTConvert(astAssignmentExpression) &&
                (!CheckArrayType(lvalue, rvalue)))
               )
            {
                if (rvalue.AssociatedType != null &&
                    lvalue.AssociatedType != null &&
                    lvalue.AssociatedType != "Unknown" &&
                    rvalue.AssociatedType != "Unknown")
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Cannot implicitly convert from '" +
                                rvalue.AssociatedType + "' to '" + lvalue.AssociatedType + "'",
                                astAssignmentExpression.LineNumber, astAssignmentExpression.ColumnNumber,
                                astAssignmentExpression.Path));
            }

        }

        /// <summary>
        /// Helper function to check Array Type
        /// </summary>
        /// <param name="lvalue"></param>
        /// <param name="rvalue"></param>
        /// <returns></returns>
        private bool CheckArrayType(IAstExpression lvalue, IAstExpression rvalue)
        {
            if (rvalue is AstVariableReference && lvalue is AstVariableReference)
            {

            }

            return true;
        }

        /// <summary>
        /// Helper function to check Type Converter
        /// </summary>
        /// <param name="astAssignmentExpression"></param>
        /// <returns></returns>
        private bool CheckTConvert(AstAssignmentExpression astAssignmentExpression)
        {
            string lvfqt = astAssignmentExpression.LValue.AssociatedType;
            string rvfqt = astAssignmentExpression.RValue.AssociatedType;

            //Make sure we have value
            if (lvfqt == null || rvfqt == null ||
               lvfqt == "Unknown" || rvfqt == "Unknown")
                return false;

            //Get just type for return type
            string lvt = lvfqt.Substring(lvfqt.LastIndexOf(".") + 1);

            bool overloaded = false;
            AstClass cItem = null;
            AstStruct sItem = null;
            AstTypeConverter ret = null;

            //check for the left operand            
            if (_classHTable != null && _classHTable.ContainsKey(lvfqt)) //check in class
            {
                cItem = (AstClass)_classHTable[lvfqt];
                ret = cItem.TypeConverLookup(lvt, rvfqt, astAssignmentExpression);
                if (ret != null) overloaded = true;
            }
            else if (_structHTable != null && _structHTable.ContainsKey(lvfqt)) //check in struct
            {
                sItem = (AstStruct)_structHTable[lvfqt];
                ret = sItem.TypeConverLookup(lvt, rvfqt, astAssignmentExpression);
                if (ret != null) overloaded = true;
            }

            //Check for the right operand
            if (_classHTable != null && _classHTable.ContainsKey(rvfqt))
            {
                cItem = (AstClass)_classHTable[rvfqt];
                ret = cItem.TypeConverLookup(lvt, rvfqt, astAssignmentExpression);
            }
            else if (_structHTable != null && _structHTable.ContainsKey(rvfqt))
            {
                sItem = (AstStruct)_structHTable[rvfqt];
                ret = sItem.TypeConverLookup(lvt, rvfqt, astAssignmentExpression);
            }

            if (ret != null && overloaded)
            {
                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Ambiguous user defined conversions '" +
                                lvfqt + ".implicit operator " + lvfqt + "(" + rvfqt + ")'" +
                                " and '" +
                                rvfqt + ".implicit operator " + lvfqt + "(" + rvfqt + ")'" +
                                "when converting from '" + rvfqt + "' to '" + lvfqt + "'",
                                astAssignmentExpression.LineNumber, astAssignmentExpression.ColumnNumber,
                                astAssignmentExpression.Path));
            }
            else if (ret != null)
            {
                overloaded = true;
            }

            return overloaded;
        }

        private bool CheckOpOverload(IAstExpression astExpression)
        {
            if (astExpression is AstBinaryExpression)
            {
                return CheckBinaryOpOverload((AstBinaryExpression)astExpression);
            }
            else //if not binary, we assume unary, since this method will only be called 
            //on binary or unary expression
            {
                return CheckUnaryOpOverlaod(astExpression);
            }
        }

        private bool CheckUnaryOpOverlaod(IAstExpression astExpression)
        {
            string fqt = astExpression.AssociatedType;

            if (fqt == null || fqt == "Unknown") return false;

            //Make parameter array list as the lookup function needed
            string[] fqtparam = new string[1];
            fqtparam[0] = fqt;

            //if it is class
            if (AstClass != null && _classHTable.ContainsKey(fqt))
            {
                AstClass cItem = (AstClass)_classHTable[fqt];
                var ret = cItem.OpOverloadLookup(fqtparam, astExpression);

                if (ret != null) return true;
                else
                {
                    string expr = "''";
                    if (astExpression is AstPostDecrement || astExpression is AstPreDecrement)
                        expr = "'--'";
                    else if (astExpression is AstPostIncrement || astExpression is AstPreIncrement)
                        expr = "'++'";

                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Operator " + expr + " cannot be applied to operand of type '" + fqt + "'",
                                ((AstStatement)astExpression).LineNumber, ((AstStatement)astExpression).ColumnNumber,
                                ((AstStatement)astExpression).Path));
                }
            }
            else if (AstStruct != null && _structHTable.ContainsKey(fqt))
            {
                AstStruct sItem = (AstStruct)_structHTable[fqt];
                var ret = sItem.OpOverloadLookup(fqtparam, astExpression);

                if (ret != null) return true;
                else
                {
                    string expr = "''";
                    if (astExpression is AstPostDecrement || astExpression is AstPreDecrement)
                        expr = "'--'";
                    else if (astExpression is AstPostIncrement || astExpression is AstPreIncrement)
                        expr = "'++'";

                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Operator " + expr + " cannot be applied to operand of type '" + fqt + "'",
                                ((AstStatement)astExpression).LineNumber, ((AstStatement)astExpression).ColumnNumber,
                                ((AstStatement)astExpression).Path));
                }
            }

            return false;
        }

        private bool CheckBinaryOpOverload(AstBinaryExpression astBinaryExpression)
        {
            string lvfqt = astBinaryExpression.LeftOperand.AssociatedType;
            string rvfqt = astBinaryExpression.RightOperand.AssociatedType;

            //Make sure we have value
            if (lvfqt == null || rvfqt == null ||
               lvfqt == "Unknown" || rvfqt == "Unknown")
                return false;

            string[] fqtparam = new string[2];
            //Making parameter array list as the lookup function needed
            fqtparam[0] = lvfqt;
            fqtparam[1] = rvfqt;

            bool overloaded = false;
            bool isTwice = false;
            AstClass cItem = null;
            AstStruct sItem = null;
            AstOperatorOverload rret = null;
            AstOperatorOverload lret = null;

            //Check in the Left Object
            if (_classHTable != null && _classHTable.ContainsKey(lvfqt)) //check in class
            {
                cItem = (AstClass)_classHTable[lvfqt];
                rret = cItem.OpOverloadLookup(fqtparam, astBinaryExpression);

                if (rret != null)
                {
                    overloaded = true;
                    //This is for the type of an expression
                    //The type of the Object has to be the return type of OpOverLoad return type
                    //Expression will return the type ONLY if the two type are equal, otherwise Unknown
                    //So, we are making the two type equal
                    astBinaryExpression.LeftOperand.AssociatedType = rret.FullQReturnType;
                    astBinaryExpression.RightOperand.AssociatedType = rret.FullQReturnType;
                }
            }
            else if (_structHTable != null && _structHTable.ContainsKey(lvfqt))//chekc in struct
            {
                sItem = (AstStruct)_structHTable[lvfqt];
                rret = sItem.OpOverloadLookup(fqtparam, astBinaryExpression);
                if (rret != null)
                {
                    overloaded = true;
                    //This is for the type of an expression
                    //The type of the Object has to be the return type of OpOverLoad return type
                    //Expression will return the type ONLY if the two type are equal, otherwise Unknown
                    //So, we are making the two type equal
                    astBinaryExpression.LeftOperand.AssociatedType = rret.FullQReturnType;
                    astBinaryExpression.RightOperand.AssociatedType = rret.FullQReturnType;
                }
            }

            if (lvfqt != rvfqt) //if the two operand are same, we won't need to check for the right
            {
                //Check in the Right Object 
                if (_classHTable != null && _classHTable.ContainsKey(rvfqt))
                {
                    cItem = (AstClass)_classHTable[rvfqt];
                    lret = cItem.OpOverloadLookup(fqtparam, astBinaryExpression);

                    if (lret != null)
                    {
                        if (overloaded) 
                            isTwice = true;
                        else
                            overloaded = true;
                        //This is for the type of an expression
                        //The type of the Object has to be the return type of OpOverLoad return type
                        //Expression will return the type ONLY if the two type are equal, otherwise Unknown
                        //So, we are making the two type equal
                        astBinaryExpression.LeftOperand.AssociatedType = lret.FullQReturnType;
                        astBinaryExpression.RightOperand.AssociatedType = lret.FullQReturnType;
                    }
              
                }
                else if (_structHTable != null && _structHTable.ContainsKey(rvfqt))
                {
                    sItem = (AstStruct)_structHTable[rvfqt];
                    lret = sItem.OpOverloadLookup(fqtparam, astBinaryExpression);

                    if (lret != null)
                    {
                        if (overloaded) 
                            isTwice = true;
                        else
                            overloaded = true;
                        //This is for the type of an expression
                        //The type of the Object has to be the return type of OpOverLoad return type
                        //Expression will return the type ONLY if the two type are equal, otherwise Unknown
                        //So, we are making the two type equal
                        astBinaryExpression.LeftOperand.AssociatedType = lret.FullQReturnType;
                        astBinaryExpression.RightOperand.AssociatedType = lret.FullQReturnType;
                    }
                }
            }

            // find the operation to give a correct message
            string expr = "";

            if (astBinaryExpression is AstAdditionExpression)
                expr = "+";
            else if (astBinaryExpression is AstSubtractionExpression)
                expr = "-";
            else if (astBinaryExpression is AstMuliplicationExpression)
                expr = "*";
            else if (astBinaryExpression is AstDivisionExpression)
                expr = "/";

            if (overloaded)
            {
                if (isTwice)//If the method is in both Objects, it is ambiguous
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The call is ambiguous between the following methods or properties: '" +
                            lvfqt + ".operator " + expr + "(" + lvfqt + "," + rvfqt + ")" +
                            "' and '" +
                            rvfqt + ".operator " + expr + "(" + lvfqt + "," + rvfqt + ")'",
                            astBinaryExpression.LineNumber, astBinaryExpression.ColumnNumber,
                            astBinaryExpression.Path));

                }
                else //if not overloaded, do the normal procedure for the type of expression
                {
                    //The type of the Object has to be the return type of OpOverLoad return type
                    //Expression will return the type ONLY if the two type are equal, otherwise Unknown
                    //So, we are making the two type equal
                    AstOperatorOverload ret = rret == null ? lret : rret;
                    astBinaryExpression.LeftOperand.AssociatedType = ret.FullQReturnType;
                    astBinaryExpression.RightOperand.AssociatedType = ret.FullQReturnType;
                }
                
            }
            else 
            {
                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Operator '" + expr + "' cannot be applied to operands of type" +
                            " '" + lvfqt + "' and '" + lvfqt + "'",
                            astBinaryExpression.LineNumber, astBinaryExpression.ColumnNumber,
                            astBinaryExpression.Path));
                overloaded = true; // the flag is set to true, since we already give an error message here
            }

            return overloaded;
        }

        /// <summary>
        /// Check for Variable Reference
        /// The first method for checking variable reference
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astVariableReference"></param>
        private void CheckVarReference(SymbolTableNode symTNode, AstVariableReference astVariableReference)
        {
            //First, Check if it has member reference
            if (CheckLocalVarMemberRef(symTNode, astVariableReference)) return;


            //Check in Symbol Table 
            SymbolObject obj = symTNode.Lookup(astVariableReference.VariableName);

            if (obj == null)
            {
                //Look in Object Layout Table
                AstField astField = ObjectLayoutLookup(astVariableReference.VariableName);

                if (astField == null)
                {
                    //Look in Method Table : Accessor
                    AstAccessor astAccessor = AccessorLookup(astVariableReference.VariableName);

                    if (astAccessor == null)
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The name '" + astVariableReference.VariableName + "' does not exist in the current context",
                                    astVariableReference.LineNumber, astVariableReference.ColumnNumber, astVariableReference.Path));
                    else
                    {
                        astVariableReference.AssociatedType = astAccessor.FullyQualifiedType;
                        astVariableReference.OwnerObj = astAccessor;
                        astVariableReference.MemberRefCollection.Add(astAccessor);
                    }
                }
                else
                {
                    astVariableReference.AssociatedType = astField.FullQualifiedType;
                    astVariableReference.OwnerObj = astField;
                    astVariableReference.MemberRefCollection.Add(astField);
                    astVariableReference.IsArray = astField.IsArray;
                    astVariableReference.IsConst = astField.IsConstant;
                }

            }
            else // If we found in symbol table, update the type
            {
                astVariableReference.AssociatedType = obj.Type;
                astVariableReference.IsArray = obj.isArray;
                astVariableReference.IsConst = obj.isConst;
            }


        }

        /// <summary>
        /// Check for Variable Reference with Variable Name
        /// This method is used for astVariableReference with member reference 
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astVariableReference"></param>
        /// <param name="VariableName"></param>
        /// <returns>Full Qualified Type Name if exist, otherwise "Unknown"</returns>       
        private object CheckVarReference(SymbolTableNode symTNode, AstVariableReference astVariableReference, string VariableName)
        {
            //Check in Symbol Table 
            SymbolObject obj = symTNode.Lookup(VariableName);

            if (obj == null)
            {
                //Look in Object Layout Table
                AstField astField = ObjectLayoutLookup(VariableName);

                if (astField == null)
                {
                    //Look in Method Table : Accessor
                    AstAccessor astAccessor = AccessorLookup(VariableName);

                    if (astAccessor == null)
                        return null;
                    else
                    {
                        astVariableReference.MemberRefCollection.Add(astAccessor);
                        return astAccessor;
                    }
                }
                else
                {
                    astVariableReference.MemberRefCollection.Add(astField);
                    return astField;
                }

            }
            else // If we found in symbol table
            {
                AstLocalVariableDeclaration lvar = GiveMeLocalVar(obj.Name);
                astVariableReference.MemberRefCollection.Add(lvar);
                return lvar;
            }
        }

        /// <summary>
        /// Check for Variable Reference with Variable Name
        /// This method is used for astMethodCall with member reference 
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astMethodCall"></param>
        /// <param name="p"></param>
        /// <returns>Full Qualified Type Name if exist, otherwise "Unknown"</returns>
        private object CheckVarReference(SymbolTableNode symTNode, AstMethodCall astMethodCall, string VariableName)
        {
            //Check in Symbol Table 
            SymbolObject obj = symTNode.Lookup(VariableName);

            if (obj == null)
            {
                //Look in Object Layout Table
                AstField astField = ObjectLayoutLookup(VariableName);

                if (astField == null)
                {
                    //Look in Method Table : Accessor
                    AstAccessor astAccessor = AccessorLookup(VariableName);

                    if (astAccessor == null)
                        return null;
                    else
                    {
                        astMethodCall.MemberRefCollection.Add(astAccessor);
                        return astAccessor;
                    }
                }
                else
                {
                    astMethodCall.MemberRefCollection.Add(astField);
                    return astField;
                }

            }
            else // If we found in symbol table
            {
                AstLocalVariableDeclaration lvar = GiveMeLocalVar(obj.Name);
                astMethodCall.MemberRefCollection.Add(lvar);
                return lvar;
            }
        }

        /// <summary>
        /// Get the LocalVarDeclare from Hashtable
        /// </summary>
        /// <param name="lvarname"></param>
        /// <returns></returns>
        private AstLocalVariableDeclaration GiveMeLocalVar(string lvarname)
        {
            for (int i = _localvarslist.Count - 1; i >= 0; i--)
            {
                AstLocalVariableDeclaration lvar = (AstLocalVariableDeclaration)_localvarslist[i];
                if (lvar != null && lvar.Name == lvarname)
                    return lvar;
            }
            return null;
        }


        /// <summary>
        /// Lookup the member name in Object
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="Obj"></param>
        /// <param name="Member"></param>
        /// <returns></returns>
        private object ObjMemberLookup(SymbolTableNode symTNode, AstVariableReference astVariableReference, object Obj, string Member)
        {
            string fqt = "";
            Object item = null;

            if (Obj is AstClass || Obj is AstStruct)
                item = Obj;
            else
            {
                if (Obj is AstLocalVariableDeclaration)
                    fqt = ((AstLocalVariableDeclaration)Obj).FullQualifiedType;
                else if (Obj is AstField)
                    fqt = ((AstField)Obj).FullQualifiedType;
                else if (Obj is AstAccessor)
                    fqt = ((AstAccessor)Obj).FullyQualifiedType;

                if (fqt != null)
                    item = GiveMeClassOrStruct(fqt);
            }

            if (item != null)
            {
                AstField astField = null;
                AstAccessor astAccess = null;
                bool sameType = false; //if the object is created in it's own object, it should be able to access private data

                if (item is AstClass)
                {
                    astField = ((AstClass)item).LookupObjectLayout(Member);
                    if (AstClass != null)
                        sameType = (AstClass.AssociatedType == ((AstClass)item).AssociatedType);
                }
                else if (item is AstStruct)
                {
                    astField = ((AstStruct)item).LookupObjectLayout(Member);
                    if (AstStruct != null)
                        sameType = (AstStruct.AssociatedType == ((AstStruct)item).AssociatedType);
                }

                if (astField != null && (astField.AstMemberModifierCollection.IsPublic || sameType))
                {
                    astVariableReference.MemberRefCollection.Add(astField);
                    return astField;
                }
                else
                {
                    if (item is AstClass)
                        astAccess = ((AstClass)item).AccessorLookup(Member, false);
                    else if (item is AstStruct)
                        astAccess = ((AstStruct)item).AccessorLookup(Member, false);

                    if (astAccess != null && astAccess.AstMemberModifierCollection.IsPublic)
                    {
                        astVariableReference.MemberRefCollection.Add(astAccess);
                        return astAccess;
                    }
                    else
                    {
                        if (item is AstClass)
                            astAccess = ((AstClass)item).AccessorLookup(Member, true);
                        else if (item is AstStruct)
                            astAccess = ((AstStruct)item).AccessorLookup(Member, true);

                        if (astAccess != null && (astAccess.AstMemberModifierCollection.IsPublic || sameType))
                        {
                            astVariableReference.MemberRefCollection.Add(astAccess);
                            return astAccess;
                        }
                        else
                        {
                            _errors.Add(new ErrorInfo(ErrorType.SymenticError, "'" +
                                    fqt + "' does not contain a definition for '" +
                                    Member + "' and no extension method '" + Member + "'" +
                                    " accepting a first argument of type '" + fqt + "'" +
                                    " could be found (are you missing a using directive or an assembly reference?)",
                                    astVariableReference.LineNumber, astVariableReference.ColumnNumber, astVariableReference.Path));
                            return null;
                        }
                    }
                }
            }

            _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The name '" +
                                    Member + "' does not exist in the current context",
                                    astVariableReference.LineNumber, astVariableReference.ColumnNumber, astVariableReference.Path));
            return null;
        }


        /// <summary>
        /// Lookup the member name in Object
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="Obj"></param>
        /// <param name="Member"></param>
        /// <returns></returns>
        private object ObjMemberLookup(SymbolTableNode symTNode, AstMethodCall astMethodCall, object Obj, string Member)
        {
            string fqt = "";
            Object item = null;

            if (Obj is AstClass || Obj is AstStruct)
                item = Obj;
            else
            {
                if (Obj is AstLocalVariableDeclaration)
                    fqt = ((AstLocalVariableDeclaration)Obj).FullQualifiedType;
                else if (Obj is AstField)
                    fqt = ((AstField)Obj).FullQualifiedType;
                else if (Obj is AstAccessor)
                    fqt = ((AstAccessor)Obj).FullyQualifiedType;

                if (fqt != null)
                    item = GiveMeClassOrStruct(fqt);
            }

            if (item != null)
            {
                AstField astField = null;
                AstAccessor astAccess = null;
                bool sameType = false; //if the object is created in it's own object, it should be able to access private data

                if (item is AstClass)
                {
                    astField = ((AstClass)item).LookupObjectLayout(Member);
                    if (AstClass != null)
                        sameType = (AstClass.AssociatedType == ((AstClass)item).AssociatedType);
                }
                else if (item is AstStruct)
                {
                    astField = ((AstStruct)item).LookupObjectLayout(Member);
                    if (AstStruct != null)
                        sameType = (AstStruct.AssociatedType == ((AstStruct)item).AssociatedType);
                }

                if (astField != null && (astField.AstMemberModifierCollection.IsPublic || sameType))
                {
                    astMethodCall.MemberRefCollection.Add(astField);
                    return astField;
                }
                else
                {
                    if (item is AstClass)
                        astAccess = ((AstClass)item).AccessorLookup(Member, false);
                    else if (item is AstStruct)
                        astAccess = ((AstStruct)item).AccessorLookup(Member, false);

                    if (astAccess != null && astAccess.AstMemberModifierCollection.IsPublic)
                    {
                        astMethodCall.MemberRefCollection.Add(astAccess);
                        return astAccess;
                    }
                    else
                    {
                        if (item is AstClass)
                            astAccess = ((AstClass)item).AccessorLookup(Member, true);
                        else if (item is AstStruct)
                            astAccess = ((AstStruct)item).AccessorLookup(Member, true);

                        if (astAccess != null && (astAccess.AstMemberModifierCollection.IsPublic || sameType))
                        {
                            astMethodCall.MemberRefCollection.Add(astAccess);
                            return astAccess;
                        }
                        else
                        {
                            _errors.Add(new ErrorInfo(ErrorType.SymenticError, "'" +
                                    fqt + "' does not contain a definition for '" +
                                    Member + "' and no extension method '" + Member + "'" +
                                    " accepting a first argument of type '" + fqt + "'" +
                                    " could be found (are you missing a using directive or an assembly reference?)",
                                    astMethodCall.LineNumber, astMethodCall.ColumnNumber, astMethodCall.Path));
                            return null;
                        }
                    }
                }
            }

            _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The name '" +
                                    Member + "' does not exist in the current context",
                                    astMethodCall.LineNumber, astMethodCall.ColumnNumber, astMethodCall.Path));
            return null;
        }

        /// <summary>
        /// This method will check the Variable Reference to see if it contins '.'
        /// if it does, it will identify the variable name and its member
        /// Finally, it will update the AstMemberRefCollection, which is a collection of AstVarRef
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astMemberReference"></param>
        private bool CheckLocalVarMemberRef(SymbolTableNode symTNode, AstVariableReference astVariableReference)
        {
            string FullVarName = astVariableReference.VariableName;
            bool isStaticCall = false;

            if (!string.IsNullOrEmpty(FullVarName) && FullVarName.Contains("."))
            {
                //Mark as a member reference
                astVariableReference.IsMemberReference = true;

                var VarNameArr = FullVarName.Split('.');
                if (VarNameArr != null)
                {
                    Object Ret = null;
                    int i = 0; //Counter 
                    string ObjName = VarNameArr[i];

                    //if 'this' or 'base'
                    if (VarNameArr[i] == "this" || VarNameArr[i] == "base")
                    {
                        if (AstClass == null)
                            Ret = AstStruct;
                        else
                            Ret = AstClass;

                        astVariableReference.MemberRefCollection.Add(Ret);
                    }

                    //Check root member object in local varRef
                    if (Ret == null)
                        Ret = CheckVarReference(symTNode, astVariableReference, ObjName);

                    //if not found, start checking for static                    
                    if (Ret == null)
                    {
                        //check if it is class/struct of current namespace
                        Ret = GiveMeClassOrStructforName(ObjName);

                        while (Ret == null && i < (VarNameArr.Length - 1))
                        {
                            i++;

                            ObjName = ObjName + "." + VarNameArr[i];

                            if (_classHTable.ContainsKey(ObjName))
                                Ret = _classHTable[ObjName];

                            if (_structHTable.ContainsKey(ObjName))
                                Ret = _structHTable[ObjName];
                        }
                        if (!isStaticCall && Ret != null) //if found, then it has to be static
                            isStaticCall = true;

                        if (Ret != null)
                            astVariableReference.MemberRefCollection.Add(Ret);
                    }


                    if (Ret != null)//if there is no error
                    {
                        i++;

                        for (int j = i; j < VarNameArr.Length && Ret != null; j++)
                        {
                            Ret = ObjMemberLookup(symTNode, astVariableReference, Ret, VarNameArr[j]);
                        }
                        //Update the Variable Information
                        if (Ret != null)
                        {
                            var fqt = "Unknown";

                            if (Ret is AstField)
                            {
                                AstField astField = (AstField)Ret;

                                if (isStaticCall && !astField.AstMemberModifierCollection.IsStatic)
                                {
                                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "An object reference is required for the non-static field, method, or property " +
                                            astField.Name, astVariableReference.LineNumber, astVariableReference.ColumnNumber,
                                            astVariableReference.Path));

                                }
                                else if (!isStaticCall && astField.AstMemberModifierCollection.IsStatic)
                                {
                                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Member '" + astField.Name + "' " +
                                        " cannot be accessed with an instance reference; qualify it with a type name instead",
                                        astVariableReference.LineNumber, astVariableReference.ColumnNumber,
                                        astVariableReference.Path));
                                }

                                fqt = astField.FullQualifiedType;
                                astVariableReference.IsArray = astField.IsArray;
                                astVariableReference.IsConst = astField.IsConstant;

                            }
                            else if (Ret is AstAccessor)
                            {
                                AstAccessor astAccess = (AstAccessor)Ret;
                                if (isStaticCall && !astAccess.AstMemberModifierCollection.IsStatic)
                                {
                                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "An object reference is required for the non-static field, method, or property " +
                                            astAccess.Name, astVariableReference.LineNumber, astVariableReference.ColumnNumber,
                                            astVariableReference.Path));

                                }
                                else if (!isStaticCall && astAccess.AstMemberModifierCollection.IsStatic)
                                {
                                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Member '" + astAccess.Name + "' " +
                                        " cannot be accessed with an instance reference; qualify it with a type name instead", astVariableReference.LineNumber, astVariableReference.ColumnNumber,
                                        astVariableReference.Path));
                                }

                                fqt = astAccess.FullyQualifiedType;
                            }

                            astVariableReference.AssociatedType = fqt;
                            astVariableReference.VariableName = VarNameArr[VarNameArr.Length - 1];

                        }
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The name '" +
                                    VarNameArr[0] + "' does not exist in the current context",
                                    astVariableReference.LineNumber, astVariableReference.ColumnNumber, astVariableReference.Path));
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method will check the Method Call to see if it contins '.'
        /// if it does, it will identify the variable name and its member
        /// Finally, it will update the AstMemberRefCollection, which is a collection of AstVarRef
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astMethodCall"></param>
        /// <returns></returns>
        private bool CheckLocalVarMemberRef(SymbolTableNode symTNode, AstMethodCall astMethodCall)
        {
            string FullVarName = astMethodCall.Name;
            bool isStaticCall = false;

            if (!string.IsNullOrEmpty(FullVarName) && FullVarName.Contains("."))
            {
                astMethodCall.IsMemberMethod = true;

                var VarNameArr = FullVarName.Split('.');
                if (VarNameArr != null)
                {
                    Object Ret = null;
                    int i = 0; //Counter 
                    string ObjName = VarNameArr[i];


                    //if 'this' or 'base'
                    if (VarNameArr[i] == "this" || VarNameArr[i] == "base")
                    {
                        if (AstClass == null)
                            Ret = AstStruct;
                        else
                            Ret = AstClass;

                        astMethodCall.MemberRefCollection.Add(Ret);
                    }


                    //Check root member object in local varRef
                    if (Ret == null)
                        Ret = CheckVarReference(symTNode, astMethodCall, ObjName);

                    //if not found, start checking for static                    
                    if (Ret == null)
                    {
                        //check if it is class/struct of current namespace
                        Ret = GiveMeClassOrStructforName(ObjName);

                        while (Ret == null && i < (VarNameArr.Length - 1))
                        {
                            i++;

                            ObjName = ObjName + "." + VarNameArr[i];

                            if (_classHTable.ContainsKey(ObjName))
                                Ret = _classHTable[ObjName];

                            if (_structHTable.ContainsKey(ObjName))
                                Ret = _structHTable[ObjName];
                        }

                        if (!isStaticCall && Ret != null) //if found, then it has to be static
                            isStaticCall = true;

                        if (Ret != null)
                            astMethodCall.MemberRefCollection.Add(Ret);

                    }

                    if (Ret != null)//if we found the localvar, field or accessor
                    {
                        i++;
                        //Check nested MemberVarRef before the actual Method Call
                        //Hence, only loop until the second last record of Array
                        for (int j = i; j < (VarNameArr.Length - 1) && Ret != null; j++)
                        {
                            Ret = ObjMemberLookup(symTNode, astMethodCall, Ret, VarNameArr[j]);
                        }

                        if (Ret != null && astMethodCall.ArgumentCollection != null)//Check for the Method Call of Last MemberVarRef
                        {
                            var fqt = "Unknown";
                            Object obj;
                            bool sameType = false;

                            if (Ret is AstClass || Ret is AstStruct)
                            {
                                obj = Ret;
                            }
                            else
                            {
                                if (Ret is AstLocalVariableDeclaration)
                                    fqt = ((AstLocalVariableDeclaration)Ret).FullQualifiedType;
                                else if (Ret is AstField)
                                    fqt = ((AstField)Ret).FullQualifiedType;
                                else if (Ret is AstAccessor)
                                    fqt = ((AstAccessor)Ret).FullyQualifiedType;

                                obj = GiveMeClassOrStruct(fqt);
                            }
                            if (obj != null)
                            {
                                if (obj is AstClass && AstClass != null)
                                    sameType = (((AstClass)obj).AssociatedType == AstClass.AssociatedType);
                                else if (obj is AstStruct && AstStruct != null)
                                    sameType = (((AstStruct)obj).AssociatedType == AstStruct.AssociatedType);
                            }
                            string[] args = GiveMeArguments(astMethodCall.ArgumentCollection);

                            var ret = MethodLookup(VarNameArr[VarNameArr.Length - 1], obj, args);

                            string param = String.Join(",", args);
                            param = "(" + param + ")";

                            if (ret == null)
                            {
                                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The function name '" + astMethodCall.Name +
                                        "' with '" + param + "' arguments does not exist in the current context",
                                        astMethodCall.LineNumber, astMethodCall.ColumnNumber,
                                        astMethodCall.Path));
                            }
                            else if (isStaticCall && !ret.AstMemberModifierCollection.IsStatic)
                            {
                                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "An object reference is required for the non-static field, method, or property " +
                                    ret.Name + param, astMethodCall.LineNumber, astMethodCall.ColumnNumber,
                                    astMethodCall.Path));
                            }
                            else if (!isStaticCall && ret.AstMemberModifierCollection.IsStatic)
                            {

                                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Member '" + ret.Name + param + "' " +
                                    "cannot be accessed with an instance reference; qualify it with a type name instead",
                                    astMethodCall.LineNumber, astMethodCall.ColumnNumber,
                                    astMethodCall.Path));
                            }
                            else if (!ret.AstMemberModifierCollection.IsPublic && !sameType)
                            {
                                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Member '" + ret.Name + param + "' " +
                                    "is inaccessible due to its protection level",
                                    astMethodCall.LineNumber, astMethodCall.ColumnNumber,
                                    astMethodCall.Path));
                            }
                            else
                            {
                                astMethodCall.AssociatedType = ret.FullQReturnType;
                                astMethodCall.Name = VarNameArr[VarNameArr.Length - 1];
                                astMethodCall.MemberRefCollection.Add(ret);
                            }
                        }
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The name '" +
                                    VarNameArr[0] + "' does not exist in the current context",
                                    astMethodCall.LineNumber, astMethodCall.ColumnNumber, astMethodCall.Path));
                    }

                }

                return true;
            }

            return false;
        }

        private object GiveMeClassOrStructforName(string objName)
        {
            string np = ""; //current name space
            string[] usingDirectives = null;
            Object ret = null;


            //Checking for current scope first
            if (AstClass != null)
            {
                if (AstClass.UsingDirectives.Length > 0)
                {
                    np = AstClass.UsingDirectives[0];
                    np = np + ".";
                }
                usingDirectives = AstClass.UsingDirectives;
            }

            else if (AstStruct != null)
            {
                np = AstStruct.UsingDirectives[0];
                usingDirectives = AstStruct.UsingDirectives;
            }

            if (_classHTable.ContainsKey(np + objName))
                ret = _classHTable[np + objName];

            if (_structHTable.ContainsKey(np + objName))
                ret = _structHTable[np + objName];

            //if not in the current name space, search in current file with using declarative 
            if (ret == null)
            {
                for (int i = 1; i < usingDirectives.Length && ret == null; i++)
                {
                    if (_classHTable.ContainsKey(usingDirectives[i] + "." + objName))
                        ret = _classHTable[usingDirectives[i] + "." + objName];
                    if (_structHTable.ContainsKey(usingDirectives[i] + "." + objName))
                        ret = _structHTable[usingDirectives[i] + "." + objName];
                }
            }

            return ret;
        }

        /// <summary>
        /// Helper function that return the Class or Struct Object 
        /// of the scope base on the fqt given
        /// </summary>
        /// <param name="fqt"> the fqt of desire object</param>
        /// <returns>AstStruct or AstClass</returns>
        private object GiveMeClassOrStruct(string fqt)
        {
            if (fqt == null)
                return null;

            if (_structHTable != null && _structHTable.ContainsKey(fqt))
                return (AstStruct)_structHTable[fqt];
            else if (_classHTable != null && _classHTable.ContainsKey(fqt))
                return (AstClass)_classHTable[fqt];

            return null;
        }


        /// <summary>
        /// This Helper method will look up the Accessor name in Method Table
        /// </summary>
        /// <param name="p"></param>
        private AstAccessor AccessorLookup(string p)
        {
            AstAccessor ret = null;
            //if it's class
            if (AstClass != null)
            {
                ret = AstClass.AccessorLookup(p, false);
                if (ret == null)
                {
                    ret = AstClass.AccessorLookup(p, true);
                }
            }
            else if (AstStruct != null) //or if it's struct
            {
                ret = AstStruct.AccessorLookup(p, false);
            }

            return ret;
        }

        /// <summary>
        /// This helper method will look up the Method in Method Table
        /// </summary>
        /// <param name="p">name of method</param>
        /// <param name="fqtArguments">list of argument in array</param>
        /// <returns></returns>
        private AstMethod MethodLookup(string p, string[] fqtArguments)
        {
            AstMethod ret = null;
            if (AstClass != null)//if it's class
            {
                ret = AstClass.MethodLookup(p, fqtArguments, false);
                if (ret == null)
                {
                    ret = AstClass.MethodLookup(p, fqtArguments, true);
                }
            }
            else if (AstStruct != null)//or if it's struct
            {
                ret = AstStruct.MethodLookup(p, fqtArguments, false);
            }

            return ret;
        }


        private AstMethod MethodLookup(string p, Object obj, string[] fqtArguments)
        {
            AstMethod ret = null;
            if (obj is AstClass)//if it's class
            {
                ret = ((AstClass)obj).MethodLookup(p, fqtArguments, false);
                if (ret == null)
                {
                    ret = ((AstClass)obj).MethodLookup(p, fqtArguments, true);
                }
            }
            else if (obj is AstStruct)//or if it's struct
            {
                ret = ((AstStruct)obj).MethodLookup(p, fqtArguments, false);
            }

            return ret;
        }


        /// <summary>
        /// This Helper method will look the field name in Object Layout Table
        /// </summary>
        /// <param name="p"></param>
        private AstField ObjectLayoutLookup(string p)
        {
            AstField ret = null;
            if (AstStruct != null)//if it's struct
                ret = AstStruct.LookupObjectLayout(p);
            else if (AstClass != null) //if it's class
                ret = AstClass.LookupObjectLayout(p);

            return ret;
        }


    }
}
