using System;
using System.Collections.Generic;
using System.Text;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstDivisionAssignmentExpression astDivAssignmentExpression)
        {
            WriteIndentSpace(3);
            WriteInfoCommentLine(string.Format("{0} /= {1}", astDivAssignmentExpression.AssociatedType,
                                               astDivAssignmentExpression.RightOperand.AssociatedType));

            astDivAssignmentExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astDivAssignmentExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            #region Div
            if(astDivAssignmentExpression.AssociatedType == "System.Int32")
            {
                LLVM.SDiv sdiv = new LLVM.SDiv(LLVMModule)
                {
                    Operand1 = "%" + leftResult,
                    Operand2 = "%" + rightResult,
                    Result = "%" + (TempCount),
                    Type = LLVMTypeName(astDivAssignmentExpression.AssociatedType)
                };

                WriteLine(2, sdiv.EmitCode());

                LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = LLVMTypeName(astDivAssignmentExpression.AssociatedType)
                };

                WriteLine(2, alloc.EmitCode());
            }
            else if(astDivAssignmentExpression.AssociatedType =="System.Single")
            {
                LLVM.FDiv fdiv = new LLVM.FDiv(LLVMModule)
                {
                    Operand1 = "%" + leftResult,
                    Operand2 = "%" + rightResult,
                    Result = "%" + (TempCount),
                    Type = LLVMTypeName(astDivAssignmentExpression.AssociatedType)
                };

                WriteLine(2, fdiv.EmitCode());

                LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = LLVMTypeName(astDivAssignmentExpression.AssociatedType)
                };

                WriteLine(2, alloc.EmitCode());
            }


            LLVM.Store s = new LLVM.Store(LLVMModule)
            {
                Type = LLVMTypeName(astDivAssignmentExpression.AssociatedType),
                Value = "%" + (TempCount),
                Pointer = "%" + (TempCount + 1)
            };

            WriteLine(2, s.EmitCode());

            ++TempCount;

            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astDivAssignmentExpression.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            ++TempCount;

            int divResult = TempCount++;

            #endregion

            #region Assignment

            switch (astDivAssignmentExpression.LeftOperand.AssociatedType)
            {
                case "System.Int32":
                    GenerateLeftOperandForDivAssignmentExpressionInt32(astDivAssignmentExpression, divResult);
                    break;
                case "System.Single":
                    GenerateLeftOperandForDivAssignmentExpressionFloat(astDivAssignmentExpression, divResult);
                    break;
                case "System.Boolean":
                    GenerateLeftOperandForDivAssignmentExpressionBoolean(astDivAssignmentExpression, divResult);
                    break;
            }

            #endregion
        }

        private void GenerateLeftOperandForDivAssignmentExpressionFloat(AstDivisionAssignmentExpression astDivAssignmentExpression, int divResult)
        {
            AstVariableReference v = (AstVariableReference)astDivAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + divResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astDivAssignmentExpression.LeftOperand.AssociatedType == "System.Single")
                    s.Type = "float";

                WriteLine(2, s.EmitCode());

                Load l = new Load(LLVMModule)
                {
                    Result = "%" + (TempCount),
                    Type = "float",
                    //since we already know this is going to be integer
                    Pointer = "%l_" + v.VariableName
                };

                WriteLine(2, l.EmitCode());
            }
            if (v.MemberRefCollection.Count > 0)
            {
                LoadAdr(v.MemberRefCollection);

                Alloca a = new Alloca(LLVMModule)
                {
                    Type = "float*",
                    Result = "%" + (TempCount + 1)
                };
                WriteLine(2, a.EmitCode());

                Store s = new Store(LLVMModule)
                {
                    Value = "%" + (TempCount - 1),
                    Pointer = "%" + (TempCount + 1),
                    Type = "float*"
                };
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load loadPtr = new Load(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "float*",
                    Pointer = a.Result
                };
                WriteLine(2, loadPtr.EmitCode());
                ++TempCount;

                Store storeActual = new Store(LLVMModule)
                {
                    Value = "%" + divResult,
                    Pointer = loadPtr.Result,
                    Type = "float"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForDivAssignmentExpressionBoolean(AstDivisionAssignmentExpression astDivAssignmentExpression, int divResult)
        {
            AstVariableReference v = (AstVariableReference)astDivAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + divResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astDivAssignmentExpression.LeftOperand.AssociatedType == "System.Boolean")
                    s.Type = "i8";

                WriteLine(2, s.EmitCode());

                Load l = new Load(LLVMModule)
                {
                    Result = "%" + (TempCount),
                    Type = "i8",
                    //since we already know this is going to be integer
                    Pointer = "%l_" + v.VariableName
                };

                WriteLine(2, l.EmitCode());
            }
            if (v.MemberRefCollection.Count > 0)
            {
                LoadAdr(v.MemberRefCollection);

                Alloca a = new Alloca(LLVMModule)
                {
                    Type = "i8*",
                    Result = "%" + (TempCount + 1)
                };
                WriteLine(2, a.EmitCode());

                Store s = new Store(LLVMModule)
                {
                    Value = "%" + (TempCount - 1),
                    Pointer = "%" + (TempCount + 1),
                    Type = "i8*"
                };
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load loadPtr = new Load(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "i8*",
                    Pointer = a.Result
                };
                WriteLine(2, loadPtr.EmitCode());
                ++TempCount;

                Store storeActual = new Store(LLVMModule)
                {
                    Value = "%" + divResult,
                    Pointer = loadPtr.Result,
                    Type = "i8"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForDivAssignmentExpressionInt32(AstDivisionAssignmentExpression astDivAssignmentExpression, int divResult)
        {
            AstVariableReference v = (AstVariableReference)astDivAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + divResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astDivAssignmentExpression.LeftOperand.AssociatedType == "System.Int32")
                    s.Type = "i32";

                WriteLine(2, s.EmitCode());

                Load l = new Load(LLVMModule)
                {
                    Result = "%" + (TempCount),
                    Type = "i32",
                    //since we already know this is going to be integer
                    Pointer = "%l_" + v.VariableName
                };

                WriteLine(2, l.EmitCode());
            }
            if (v.MemberRefCollection.Count > 0)
            {
                LoadAdr(v.MemberRefCollection);

                Alloca a = new Alloca(LLVMModule)
                {
                    Type = "i32*",
                    Result = "%" + (TempCount + 1)
                };
                WriteLine(2, a.EmitCode());

                Store s = new Store(LLVMModule)
                {
                    Value = "%" + (TempCount - 1),
                    Pointer = "%" + (TempCount + 1),
                    Type = "i32*"
                };
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load loadPtr = new Load(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "i32*",
                    Pointer = a.Result
                };
                WriteLine(2, loadPtr.EmitCode());
                ++TempCount;

                Store storeActual = new Store(LLVMModule)
                {
                    Value = "%" + divResult,
                    Pointer = loadPtr.Result,
                    Type = "i32"
                };
                WriteLine(2, storeActual.EmitCode());
            }
        }
    }
}
