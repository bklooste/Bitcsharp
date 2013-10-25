using System.Collections;
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.Semantic
{
    public partial class ObjectHierarchy
    {
        public void CheckNestedNS(LLVMSharpCompiler Compiler)
        {
            foreach (AstSourceFile astSourceFile in Compiler.AstProgram.SourceFiles)
            {
                foreach (AstNamespaceBlock nsBlock in astSourceFile.AstNamespaceBlockCollection)
                {
                    if(nsBlock.AstNamespaceBlockCollection.Count!=0)
                    {
                        ErrorInfo e = new ErrorInfo();
                        e.type = ErrorType.SymenticError;
                        e.col = nsBlock.ColumnNumber;
                        e.line = nsBlock.LineNumber;
                        e.fileName = nsBlock.Path;
                        e.message = "Nested Namespace(s) in "+nsBlock.Namespace+"  detected. Nested namespaces are not supported";
                        Compiler.Errors.Add(e);
                    }
                }
            }
        }

        public void GenerateFQTNewType()
        {
            foreach (AstSourceFile SItem in _astProgram.SourceFiles)
            {
                _usingDeclarative.Clear();
                foreach (AstUsingDeclarative item in SItem.AstUsingDeclarativeCollection)
                {
                    if (!_usingDeclarative.Contains(item.Namespace))
                        _usingDeclarative.Add(item.Namespace, item);
                }
                foreach (AstType item in SItem.AstTypeCollection)
                {
                    if (item is AstClass) // parent class is only for classes
                        GenerateFQTNewType((AstClass)item, "");
                    else if (item is AstStruct)
                        GenerateFQTNewType((AstStruct)item, "");
                }

                foreach (AstNamespaceBlock item in SItem.AstNamespaceBlockCollection)
                {
                    GenerateFQTNewType(item, item.Namespace);
                }
            }
        }

        private void GenerateFQTNewType(AstNamespaceBlock item, string ns)
        {
            foreach (AstType i in item.AstTypeCollection)
            {
                if (i is AstClass) // parent class is only for classes
                    GenerateFQTNewType((AstClass)i, ns);
                else if (i is AstStruct)
                    GenerateFQTNewType((AstStruct)i, ns);
            }

            foreach (AstNamespaceBlock i in item.AstNamespaceBlockCollection)
            {
                if (string.IsNullOrEmpty(ns))
                    ns = item.Namespace + "." + i.Namespace;
                else
                    ns += "." + i.Namespace;
                GenerateFQTNewType(i, ns);
            }
        }

        private void GenerateFQTNewType(AstStruct astStruct, string ns)
        {
            GenerateFQTNewType(astStruct.AstMethodCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTNewType(astStruct.AstAccessorCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTNewType(astStruct.AstConstructorCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTNewType(astStruct.AstOperatorOverloadCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTNewType(astStruct.AstTypeConverterCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTNewType(astStruct.AstFieldCollection, ns, astStruct.FullQualifiedName, false);
        }

        private void GenerateFQTNewType(AstClass astClass, string ns)
        {
            GenerateFQTNewType(astClass.AstMethodCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTNewType(astClass.AstConstructorCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTNewType(astClass.AstOperatorOverloadCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTNewType(astClass.AstTypeConverterCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTNewType(astClass.AstAccessorCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTNewType(astClass.AstFieldCollection, ns, astClass.FullQualifiedName, true);
        }

        private void GenerateFQTNewType(AstFieldCollection astFieldCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstField astField in astFieldCollection)
            {
                GenerateFQTNewType(astField, ns, fullType, isClass);
            }
        }

        private void GenerateFQTNewType(AstField astField, string ns, string fullType, bool isClass)
        {
            if (astField.Initialization != null)
            {
                GenerateFQTNewType(astField.Initialization, ns, fullType, isClass);
            }

        }

        private void GenerateFQTNewType(AstAccessorCollection astAccessorCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstAccessor item in astAccessorCollection)
            {
                if (item.AstGetAccessor != null)
                {
                    GenerateFQTNewType(item.AstGetAccessor.AstBlock, ns, fullType, isClass);
                }
                if (item.AstSetAccessor != null)
                {
                    GenerateFQTNewType(item.AstSetAccessor.AstBlock, ns, fullType, isClass);
                }
            }
        }

        private void GenerateFQTNewType(AstTypeConverterCollection astTypeConverterCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstTypeConverter item in astTypeConverterCollection)
            {
                GenerateFQTNewType(item.AstBlock, ns, fullType, isClass);
            }
        }

        private void GenerateFQTNewType(AstOperatorOverloadCollection astOperatorOverloadCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstOperatorOverload item in astOperatorOverloadCollection)
            {
                GenerateFQTNewType(item.AstBlock, ns, fullType, isClass);
            }
        }

        private void GenerateFQTNewType(AstConstructorCollection astConstructorCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstConstructor astConstructor in astConstructorCollection)
            {
                GenerateFQTNewType(astConstructor.AstBlock, ns, fullType, isClass);
            }
        }

        private void GenerateFQTNewType(AstMethodCollection astMethodCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstMethod astMethod in astMethodCollection)
            {
                GenerateFQTNewType(astMethod.AstBlock, ns, fullType, isClass);
            }
        }

        private void GenerateFQTNewType(AstBlock astBlock, string ns, string fullType, bool isClass)
        {
            if (astBlock != null)
            {
                foreach (AstStatement astStatement in astBlock.AstStatementCollection)
                {
                    GenerateFQTNewType(astStatement, ns, fullType, isClass);
                }
            }
        }

        private void GenerateFQTNewType(AstStatement astStatement, string ns, string fullType, bool isClass)
        {
            if (astStatement is AstIfCondition)
            {
                AstIfCondition tempCond = (AstIfCondition)astStatement;
                GenerateFQTNewType(tempCond.Condition, ns, fullType, isClass);
                if (tempCond.AstStatement != null)
                {
                    if (tempCond.AstStatement is AstBlock)
                    {
                        AstBlock astBlockTemp = (AstBlock)tempCond.AstStatement;
                        GenerateFQTNewType(astBlockTemp, ns, fullType, isClass);
                    }
                    else
                    {
                        GenerateFQTNewType(tempCond.AstStatement, ns, fullType, isClass);
                    }
                }
                if (tempCond.AstStatementElse != null)
                {
                    if (tempCond.AstStatementElse is AstBlock)
                    {
                        AstBlock astBlockTemp = (AstBlock)tempCond.AstStatementElse;
                        GenerateFQTNewType(astBlockTemp, ns, fullType, isClass);
                    }
                    else
                    {
                        GenerateFQTNewType(tempCond.AstStatementElse, ns, fullType, isClass);
                    }
                }
            }
            else if(astStatement is AstBlock)
            {
                AstBlock astBlock = (AstBlock)astStatement;
                GenerateFQTNewType(astBlock, ns, fullType, isClass);
            }
            else if (astStatement is AstDoLoop)
            {
                AstDoLoop tempDoLoop = (AstDoLoop)astStatement;
                GenerateFQTNewType(tempDoLoop.Condition, ns, fullType, isClass);
                GenerateFQTNewType(tempDoLoop.AstStatement, ns, fullType, isClass);//added
            }
            else if (astStatement is AstForLoop)
            {
                AstForLoop tempForLoop = (AstForLoop)astStatement;
                GenerateFQTNewType(tempForLoop.Condition, ns, fullType, isClass);
                GenerateFQTNewType(tempForLoop.Body, ns, fullType, isClass);
                foreach (IAstExpression item in tempForLoop.IncrementExpressions)
                {
                    GenerateFQTNewType(item, ns, fullType, isClass);
                }
                foreach (AstStatement item in tempForLoop.Initializers)
                {
                    GenerateFQTNewType(item, ns, fullType, isClass);
                }
            }
            else if (astStatement is AstWhileLoop)
            {
                AstWhileLoop tempWhileLoop = (AstWhileLoop)astStatement;
                GenerateFQTNewType(tempWhileLoop.Condition, ns, fullType, isClass);
                GenerateFQTNewType(tempWhileLoop.AstStatement, ns, fullType, isClass);

            }
            else if (astStatement is AstLocalVariableDeclaration)
            {
                AstLocalVariableDeclaration tempLocalVar = (AstLocalVariableDeclaration)astStatement;
                GenerateFQTNewType(tempLocalVar.Initialization, ns, fullType, isClass);
            }
            else if (astStatement is AstReturn)
            {
                AstReturn tempAstRet = (AstReturn)astStatement;
                GenerateFQTNewType(tempAstRet.AstExpression, ns, fullType, isClass);
            }
            else if (astStatement is AstArrayInitialization)
            {
                AstArrayInitialization tempArrayInit = (AstArrayInitialization)astStatement;
                GenerateFQTNewType(tempArrayInit.AstExpression, ns, fullType, isClass);
                foreach (IAstExpression item in tempArrayInit.AstExpressionCollection)
                {
                    GenerateFQTNewType(item, ns, fullType, isClass);
                }
            }
            else if (astStatement is AstAssignmentStatement)
            {
                AstAssignmentStatement astAssign = (AstAssignmentStatement)astStatement;
                GenerateFQTNewType(astAssign.AstAssignmentExpression.RValue, ns, fullType, isClass);
            }
            else if (astStatement is AstMethodCall)
            {
                AstMethodCall astMethodCall = (AstMethodCall)astStatement;
                foreach (AstArgument item in astMethodCall.ArgumentCollection)
                {
                    GenerateFQTNewType(item.AstExpression, ns, fullType, isClass);
                }
            }
            else if (astStatement is AstBaseConstructorCall)
            {
                AstBaseConstructorCall constructCall = (AstBaseConstructorCall)astStatement;
                foreach (AstArgument item in constructCall.AstArgumentCollection)
                {
                    GenerateFQTNewType(item.AstExpression, ns, fullType, isClass);
                }
            }
            else if (astStatement is AstThisConstructorCall)
            {
                AstThisConstructorCall constructCall = (AstThisConstructorCall)astStatement;
                foreach (AstArgument item in constructCall.AstArgumentCollection)
                {
                    GenerateFQTNewType(item.AstExpression, ns, fullType, isClass);
                }
            }
            
           
        }

        private void GenerateFQTNewType(IAstExpression iAstExpression, string ns, string fullType, bool isClass)
        {
            if (iAstExpression is AstNewType)
            {
                AstNewType astNewTypeTemp = (AstNewType)iAstExpression;
                GenerateFQTNewType(astNewTypeTemp, ns, fullType, isClass);
                if (astNewTypeTemp.AstArrayInitialization != null)
                {
                    GenerateFQTNewType(astNewTypeTemp.AstArrayInitialization.AstExpression, ns, fullType, isClass);
                    foreach (IAstExpression item in astNewTypeTemp.AstArrayInitialization.AstExpressionCollection)
                    {
                        GenerateFQTNewType(item, ns, fullType, isClass);
                    }
                }
            }
            else if (iAstExpression is AstNull)
            {
                AstNull astNull = (AstNull) iAstExpression;
                astNull.FQT = fullType;
            }
            #region CheckBinaryExpressions
            else if (iAstExpression is AstAdditionExpression)
            {
                AstAdditionExpression astAddExpr = (AstAdditionExpression)iAstExpression;
                GenerateFQTNewType(astAddExpr.LeftOperand, ns, fullType, isClass);
                GenerateFQTNewType(astAddExpr.RightOperand, ns, fullType, isClass);
            }
            else if (iAstExpression is AstSubtractionExpression)
            {
                AstSubtractionExpression astSubExpr = (AstSubtractionExpression)iAstExpression;
                GenerateFQTNewType(astSubExpr.LeftOperand, ns, fullType, isClass);
                GenerateFQTNewType(astSubExpr.RightOperand, ns, fullType, isClass);
            }
            else if (iAstExpression is AstMuliplicationExpression)
            {
                AstMuliplicationExpression astMulExpr = (AstMuliplicationExpression)iAstExpression;
                GenerateFQTNewType(astMulExpr.LeftOperand,ns,fullType,isClass);
                GenerateFQTNewType(astMulExpr.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstDivisionExpression)
            {
                AstDivisionExpression astDivExpr = (AstDivisionExpression)iAstExpression;
                GenerateFQTNewType(astDivExpr.LeftOperand, ns, fullType, isClass);
                GenerateFQTNewType(astDivExpr.RightOperand, ns, fullType, isClass);
            }
            else if (iAstExpression is AstAndExpression)
            {
                AstAndExpression astAnd = (AstAndExpression)iAstExpression;
                GenerateFQTNewType(astAnd.LeftOperand, ns, fullType, isClass);
                GenerateFQTNewType(astAnd.RightOperand, ns, fullType, isClass);
            }
            else if (iAstExpression is AstEqualityExpression)
            {
                AstEqualityExpression astEq = (AstEqualityExpression)iAstExpression;
                GenerateFQTNewType(astEq.LeftOperand,ns,fullType,isClass);
                GenerateFQTNewType(astEq.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstGreaterThanExpression)
            {
                AstGreaterThanExpression astGteExpr = (AstGreaterThanExpression)iAstExpression;
                GenerateFQTNewType(astGteExpr.LeftOperand, ns, fullType, isClass);
                GenerateFQTNewType(astGteExpr.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstGreaterThanOrEqualExpression)
            {
                AstGreaterThanOrEqualExpression astGteOr = (AstGreaterThanOrEqualExpression)iAstExpression;
                GenerateFQTNewType(astGteOr.LeftOperand,ns,fullType,isClass);
                GenerateFQTNewType(astGteOr.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstInequalityExpression)
            {
                AstInequalityExpression astIneq = (AstInequalityExpression)iAstExpression;
                GenerateFQTNewType(astIneq.LeftOperand,ns,fullType,isClass);
                GenerateFQTNewType(astIneq.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstLesserThanExpression)
            {
                AstLesserThanExpression astLess = (AstLesserThanExpression)iAstExpression;
                GenerateFQTNewType(astLess.LeftOperand,ns,fullType,isClass);
                GenerateFQTNewType(astLess.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstLesserThanOrEqualExpression)
            {
                AstLesserThanOrEqualExpression astLessOr = (AstLesserThanOrEqualExpression)iAstExpression;
                GenerateFQTNewType(astLessOr.LeftOperand,ns,fullType,isClass);
                GenerateFQTNewType(astLessOr.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstOrExpression)
            {
                AstOrExpression astOrExpr = (AstOrExpression)iAstExpression;
                GenerateFQTNewType(astOrExpr.LeftOperand,ns,fullType,isClass);
                GenerateFQTNewType(astOrExpr.RightOperand,ns,fullType,isClass);
            }
            #endregion
            #region CheckMathAssignExpr
            else if (iAstExpression is AstAddAssignmentExpression)
            {
                AstAddAssignmentExpression astAddAssExpr = (AstAddAssignmentExpression)iAstExpression;
                GenerateFQTNewType(astAddAssExpr.RightOperand, ns, fullType, isClass);
            }
            else if (iAstExpression is AstSubtractAssignmentExpression)
            {
                AstSubtractAssignmentExpression astSubAss = (AstSubtractAssignmentExpression)iAstExpression;
                GenerateFQTNewType(astSubAss.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstMultiplyAssignmentExpression)
            {
                AstMultiplyAssignmentExpression astMulExpr = (AstMultiplyAssignmentExpression)iAstExpression;
                GenerateFQTNewType(astMulExpr.RightOperand,ns,fullType,isClass);
            }
            else if (iAstExpression is AstDivisionAssignmentExpression)
            {
                AstDivisionAssignmentExpression astDivAss = (AstDivisionAssignmentExpression)iAstExpression;
                GenerateFQTNewType(astDivAss.RightOperand, ns, fullType, isClass);
            }
            #endregion
            #region CheckUnary's
            else if (iAstExpression is AstUnaryMinus)
            {
                AstUnaryMinus astUnMinus = (AstUnaryMinus)iAstExpression;
                GenerateFQTNewType(astUnMinus.AstExpression, ns, fullType, isClass);
            }
            else if (iAstExpression is AstUnaryPlus)
            {
                AstUnaryPlus astUnPlus = (AstUnaryPlus)iAstExpression;
                GenerateFQTNewType(astUnPlus.AstExpression, ns, fullType, isClass);
            }
            else if (iAstExpression is AstNot)
            {
                AstNot astNot = (AstNot)iAstExpression;
                GenerateFQTNewType(astNot.AstExpression, ns, fullType, isClass);
            }
            else if (iAstExpression is AstPostDecrement)
            {
                AstPostDecrement astPost = (AstPostDecrement)iAstExpression;
                GenerateFQTNewType(astPost.AstExpression, ns, fullType, isClass);
            }
            else if (iAstExpression is AstPostIncrement)
            {
                AstPostIncrement astPostIn = (AstPostIncrement)iAstExpression;
                GenerateFQTNewType(astPostIn.AstExpression, ns, fullType, isClass);
            }
            else if (iAstExpression is AstPreDecrement)
            {
                AstPreDecrement astPreDec = (AstPreDecrement)iAstExpression;
                GenerateFQTNewType(astPreDec.AstExpression, ns, fullType, isClass);
            }
            else if (iAstExpression is AstPreIncrement)
            {
                AstPreIncrement astPreInc = (AstPreIncrement)iAstExpression;
                GenerateFQTNewType(astPreInc.AstExpression, ns, fullType, isClass);
            }
            #endregion
            #region GenerateString & IntegerTable
            else if (iAstExpression is AstStringConstant)
            {
                AstStringConstant astStringConst = (AstStringConstant)iAstExpression;
                if (astStringConst.ConstantValue != null && !StringTable.Contains(astStringConst.ConstantValue))
                {
                    int cnt = StringTable.Count;
                    StringTable.Add(astStringConst.ConstantValue, cnt++);
                }
            }
            else if (iAstExpression is AstIntegerConstant)
            {
                AstIntegerConstant astIntConst = (AstIntegerConstant)iAstExpression;
                if (astIntConst.Constant != null && !IntTable.Contains(astIntConst.ConstantValue))
                {
                    int cnt = IntTable.Count;
                    IntTable.Add(astIntConst.ConstantValue, cnt++);
                }
            }
            #endregion
        }

        private void GenerateFQTNewType(AstNewType astNewType, string ns, string fullType, bool isClass)
        {
            bool builtin = false;

            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(astNewType.Type);
            if (!string.IsNullOrEmpty(primitiveType))
            {
                builtin = true;
                astNewType.FullQualifiedType = primitiveType;
            }

            if (string.IsNullOrEmpty(ns))
            {
                bool found = false;

                foreach (string i in _usingDeclarative.Keys)
                {
                    string temp_full = i + "." + astNewType.Type;
                    if (!builtin)
                    {
                        if (ClassHTable.ContainsKey(temp_full) || StructHTable.ContainsKey(temp_full) || EnumHTable.ContainsKey(temp_full))
                        {
                            astNewType.FullQualifiedType = temp_full;
                            found = true;
                        }
                    }
                    else
                    {
                        if (ClassHTable.ContainsKey(astNewType.FullQualifiedType) || StructHTable.ContainsKey(astNewType.FullQualifiedType) || EnumHTable.ContainsKey(astNewType.FullQualifiedType))
                        {
                            // parameter.FullQualifiedType already contains the full name
                            found = true;
                        }
                    }
                }
                if (!found) // check if using fully qualified name
                {
                    string returnTypetoCheck = astNewType.FullQualifiedType ?? astNewType.Type;
                    if (ClassHTable.ContainsKey(returnTypetoCheck) || StructHTable.ContainsKey(returnTypetoCheck) || EnumHTable.ContainsKey(returnTypetoCheck))
                    {
                        astNewType.FullQualifiedType = returnTypetoCheck;
                        found = true;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type " + astNewType.Type + " not found (are you missing using declaratives)", astNewType.LineNumber, astNewType.ColumnNumber, astNewType.Path));
                        _hasError = true;
                    }
                }
            }
            else
            {
                string toFind = ns + "." + astNewType.Type;
                if (ClassHTable.ContainsKey(toFind) || StructHTable.ContainsKey(toFind) || EnumHTable.ContainsKey(toFind))
                {
                    astNewType.FullQualifiedType = toFind;
                    return;
                }
                int cutIndex = toFind.LastIndexOf("." + astNewType.Type);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFQTNewType(astNewType, "", fullType, isClass);
                else
                    GenerateFQTNewType(astNewType, temp.Substring(0, temp.LastIndexOf('.')), fullType, isClass);
            }
        }
      
        private Hashtable _stringTable = new Hashtable();
        public Hashtable StringTable
        {
            get { return _stringTable; }

        }
        private Hashtable _intTable = new Hashtable();
        public Hashtable IntTable
        {
            get { return _intTable; }
        }
    }
}
