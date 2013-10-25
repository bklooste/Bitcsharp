using System;
using System.Collections;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.Semantic;

namespace LLVMSharp.Compiler
{
    public class ClassHierarchyNode
    {
        #region Properties
        private ErrorList _errors;
        private string _methodReturnValue = null;
        private TypeChecker typeChecker = null;
        private Hashtable _classHTable = null;

        #endregion


        public ClassHierarchyNode()
        {
            MethodTable = new Hashtable();
            //ObjectLayout = new ObjectLayoutNode(this);
        }

        public AstClass AstClass;


        public string FullClassName
        {
            get
            {
                if (AstClass == null)
                    return string.Empty;
                else
                    return AstClass.FullQualifiedName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///     key: encoded method name
        ///     value: AstMethod
        /// </remarks>
        public Hashtable MethodTable = null;
        //public ObjectLayoutNode ObjectLayout = null;

        public ClassHierarchyNodeCollection ClassHierarchyNodeCollection = new ClassHierarchyNodeCollection();

        public override string ToString()
        {
            return AstClass.ToString();
        }

        /// <summary>
        /// This is the method that added the symbol table information
        /// to each block, if there is an error, it will update the errorslist
        /// </summary>
        /// <param name="errors"></param>
        public void CreateSymbolTableForAll(LLVMSharpCompiler Compiler)
        {
            _errors = Compiler.Errors;

            typeChecker = new TypeChecker(Compiler);

            _classHTable = Compiler.ClassHashtable;

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
                    //    e.message = "Accessor " + AstClass.FullQualifiedName + "." + aItem.Name + " Must declare a body beacause it's not marked abstract. Automatically implemented properties must implement must define both get & set accessors";
                    //    Compiler.Errors.Add(e);
                    //    continue;
                    //}
                    if (aItem.AstGetAccessor != null)
                    {
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
                    _methodReturnValue = mItem.FullQReturnType;

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
                    AstLocalVariableDeclaration s = (AstLocalVariableDeclaration)sItem;
                    symTNode.Insert(s.Name,
                                    s.FullQualifiedType,
                                    s.IsArray,
                                    s.IsConstant,
                                    s.LineNumber,
                                    s.ColumnNumber,
                                    s.Path);
                    CheckExpression(symTNode, s.Initialization);
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


                    AstStatement _astElseStat = ((AstIfCondition)sItem).AstStatementElse;
                    if (_astElseStat is AstBlock)
                    {
                        symTNode.OpenScope();
                        CreateBlockSymbol(symTNode, ((AstBlock)_astElseStat).AstStatementCollection);
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
                        CreateBlockSymbol(symTNode, ((AstBlock)_astStat).AstStatementCollection);
                        symTNode.CloseScope();
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
                }

                else if (sItem is AstForLoop)//For FOR block
                {

                    symTNode.OpenScope();
                    //Added the initializer first
                    CreateBlockSymbol(symTNode, ((AstForLoop)sItem).Initializers);

                    //Check the type for Condition, second
                    CheckExpression(symTNode, ((AstForLoop)sItem).Condition);
                    if (((AstForLoop)sItem).Condition.AssociatedType != "System.Boolean")
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
                        CreateBlockSymbol(symTNode, ((AstBlock)_astStat).AstStatementCollection);
                    }
                    else if (_astStat is AstStatement) //if only one statement
                    {
                        AstStatementCollection astStatCol = new AstStatementCollection();
                        astStatCol.Add(_astStat);
                        CreateBlockSymbol(symTNode, astStatCol);
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
                            if (((AstReturn)sItem).AssociatedType != _methodReturnValue)
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
                    CheckExpression(symTNode, ((AstAssignmentStatement)sItem).AstAssignmentExpression.RValue);
                    CheckExpression(symTNode, ((AstAssignmentStatement)sItem).AstAssignmentExpression.LValue);

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
            }
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

            if (astMethodCall.ArgumentCollection != null)
            {
                string[] args = new string[astMethodCall.ArgumentCollection.Count];
                int i = 0;
                foreach (AstArgument item in astMethodCall.ArgumentCollection)
                    args[i++] = item.AssociatedType;

                //Look in the method table
                var ret = MethodLookup(astMethodCall.Name, args);

                if (ret == null)
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The function name '" + astMethodCall.Name +
                            "' with '" + i + "' arguments does not exist in the current context",
                            astMethodCall.LineNumber, astMethodCall.ColumnNumber,
                            astMethodCall.Path));
                }
                else
                {
                    astMethodCall.AssociatedType = ret.FullQReturnType;
                }

            }

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
            if (fqt != null && _classHTable.ContainsKey(fqt))
            {
                AstClass cItem = (AstClass)_classHTable[fqt];

                var astConst = cItem.AstConstructorLookup(type, args, false);
                if (astConst != null)
                {
                    return astConst;
                }
            }

            return null;
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

            if (_classHTable.ContainsKey(lvfqt))
            {
                AstClass cItem = (AstClass)_classHTable[lvfqt];
                var ret = cItem.TypeConverLookup(lvt, rvfqt, astAssignmentExpression);

                if (ret == null)  //If Left object don't have the method, then check in the right Object
                {
                    if (_classHTable.ContainsKey(rvfqt))
                    {
                        cItem = (AstClass)_classHTable[rvfqt];
                        ret = cItem.TypeConverLookup(lvt, rvfqt, astAssignmentExpression);
                        if (ret != null)
                        {
                            return true;
                        }
                    }
                }
                else
                    return true;

            }

            return false;
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

            if (_classHTable.ContainsKey(fqt))
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
            //Check in the Left Object
            if (_classHTable.ContainsKey(lvfqt))
            {

                AstClass cItem = (AstClass)_classHTable[lvfqt];
                var ret = cItem.OpOverloadLookup(fqtparam, astBinaryExpression);

                if (ret != null)
                {
                    overloaded = true;
                    //This is for the type of an expression
                    //The type of the Object has to be the return type of OpOverLoad return type
                    //Expression will return the type ONLY if the two type are equal, otherwise Unknown
                    //So, we are making the two type equal
                    astBinaryExpression.LeftOperand.AssociatedType = ret.FullQReturnType;
                    astBinaryExpression.RightOperand.AssociatedType = ret.FullQReturnType;
                }
            }

            //Check in the Right Object if the two Object are different
            if (_classHTable.ContainsKey(rvfqt) && lvfqt != rvfqt)
            {
                AstClass cItem = (AstClass)_classHTable[rvfqt];
                var ret = cItem.OpOverloadLookup(fqtparam, astBinaryExpression);
                if (ret != null)
                {
                    if (overloaded)//If Left != Right Object & the method is already in Left Object, it is ambiguous
                    {
                        string expr = "";

                        if (astBinaryExpression is AstAdditionExpression)
                            expr = "+";
                        else if (astBinaryExpression is AstSubtractionExpression)
                            expr = "-";
                        else if (astBinaryExpression is AstMuliplicationExpression)
                            expr = "*";
                        else if (astBinaryExpression is AstDivisionExpression)
                            expr = "/";

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
                        astBinaryExpression.LeftOperand.AssociatedType = ret.FullQReturnType;
                        astBinaryExpression.RightOperand.AssociatedType = ret.FullQReturnType;
                    }
                    overloaded = true; //the flag is set to true anyways
                }
            }

            return overloaded;
        }

        /// <summary>
        /// Check for Variable Reference
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astVariableReference"></param>
        private void CheckVarReference(SymbolTableNode symTNode, AstVariableReference astVariableReference)
        {
            //First, Check if it has member reference
            if (!CheckLocalVarMemberRef(symTNode, astVariableReference)) return;


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
                    }
                }
                else
                {
                    astVariableReference.AssociatedType = astField.FullQualifiedType;
                    astVariableReference.OwnerObj = astField;
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
                        astVariableReference.MemberRefCollection.Add(astAccessor.FullyQualifiedType);
                        astVariableReference.OwnerObj = astAccessor;
                        return astAccessor;
                    }
                }
                else
                {
                    astVariableReference.MemberRefCollection.Add(astField.FullQualifiedType);
                    astVariableReference.OwnerObj = astField;
                    astVariableReference.IsArray = astField.IsArray;
                    astVariableReference.IsConst = astField.IsConstant;
                    return astField;
                }

            }
            else // If we found in symbol table
            {
                astVariableReference.MemberRefCollection.Add(obj.Type);
                astVariableReference.IsArray = obj.isArray;
                astVariableReference.IsConst = obj.isConst;
                return obj;
            }
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

            if (Obj is SymbolObject)
                fqt = ((SymbolObject)Obj).Type;
            else if (Obj is AstField)
                fqt = ((AstField)Obj).FullQualifiedType;
            else if (Obj is AstAccessor)
                fqt = ((AstAccessor)Obj).FullyQualifiedType;


            if (fqt != null && _classHTable.ContainsKey(fqt))
            {
                AstClass cItem = (AstClass)_classHTable[fqt];

                AstField astField = cItem.LookupObjectLayout(Member);
                if (astField != null)
                {
                    astVariableReference.MemberRefCollection.Add(astField.FullQualifiedType);
                    return astField;
                }
                else
                {
                    AstAccessor astAccess = cItem.AccessorLookup(Member, false);
                    if (astAccess != null)
                    {
                        astVariableReference.MemberRefCollection.Add(astAccess.FullyQualifiedType);
                        return astAccess;
                    }
                    else
                    {
                        astAccess = cItem.AccessorLookup(Member, true);
                        if (astAccess != null)
                        {
                            astVariableReference.MemberRefCollection.Add(astAccess.FullyQualifiedType);
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
        /// This method will check the Variable Reference to see if it contins '.'
        /// if it does, it will identify the variable name and its member
        /// Finally, it will update the AstMemberRefCollection, which is a collection of AstVarRef
        /// </summary>
        /// <param name="symTNode"></param>
        /// <param name="astMemberReference"></param>
        private bool CheckLocalVarMemberRef(SymbolTableNode symTNode, AstVariableReference astVariableReference)
        {
            string FullVarName = astVariableReference.VariableName;

            if (!string.IsNullOrEmpty(FullVarName) && FullVarName.Contains("."))
            {
                //Mark as a member reference
                astVariableReference.IsMemberReference = true;

                var VarNameArr = FullVarName.Split('.');
                if (VarNameArr != null)
                {
                    //Check root member object
                    var Ret = CheckVarReference(symTNode, astVariableReference, VarNameArr[0]);

                    if (Ret != null)//if there is no error
                    {
                        for (int i = 1; i < VarNameArr.Length && Ret != null; i++)
                        {
                            Ret = ObjMemberLookup(symTNode, astVariableReference, Ret, VarNameArr[i]);
                        }
                        //Update the Variable Information
                        if (Ret != null)
                        {
                            var fqt = "Unknown";

                            if (Ret is SymbolObject)
                                fqt = ((SymbolObject)Ret).Type;
                            else if (Ret is AstField)
                                fqt = ((AstField)Ret).FullQualifiedType;
                            else if (Ret is AstAccessor)
                                fqt = ((AstAccessor)Ret).FullyQualifiedType;

                            astVariableReference.AssociatedType = fqt;
                            astVariableReference.VariableName = VarNameArr[VarNameArr.Length - 1];

                        }
                        else
                            return false;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The name '" +
                                    VarNameArr[0] + "' does not exist in the current context",
                                    astVariableReference.LineNumber, astVariableReference.ColumnNumber, astVariableReference.Path));
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// This Helper method will look up the Accessor name in Method Table
        /// </summary>
        /// <param name="p"></param>
        private AstAccessor AccessorLookup(string p)
        {
            var ret = AstClass.AccessorLookup(p, false);
            if (ret == null)
            {
                ret = AstClass.AccessorLookup(p, true);
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
            var ret = AstClass.MethodLookup(p, fqtArguments, false);
            if (ret == null)
            {
                ret = AstClass.MethodLookup(p, fqtArguments, true);
            }
            return ret;
        }

        /// <summary>
        /// This Helper method will look the field name in Object Layout Table
        /// </summary>
        /// <param name="p"></param>
        private AstField ObjectLayoutLookup(string p)
        {

            return AstClass.LookupObjectLayout(p);
        }

    }

    public class ClassHierarchyNodeCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is ClassHierarchyNode))
                throw new ArgumentException("You can add type of only ClassHierarchyNode.");
            return base.Add(value);
        }

        public int Add(ClassHierarchyNode value)
        {
            return base.Add(value);
        }
    }
}
