using System;
using System.Collections.Generic;
using System.Text;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstMultiplyAssignmentExpression astMulAssignmentExpression)
        {
            WriteIndentSpace(3);
            WriteInfoCommentLine(string.Format("{0} *= {1}", astMulAssignmentExpression.AssociatedType,
                                               astMulAssignmentExpression.RightOperand.AssociatedType));

            astMulAssignmentExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astMulAssignmentExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            #region Mul

            LLVM.Mul mul = new LLVM.Mul(LLVMModule)
            {
                Operand1 = "%" + leftResult,
                Operand2 = "%" + rightResult,
                Result = "%" + (TempCount),
                Type = LLVMTypeName(astMulAssignmentExpression.AssociatedType)
            };

            WriteLine(2, mul.EmitCode());

            LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astMulAssignmentExpression.AssociatedType)
            };

            WriteLine(2, alloc.EmitCode());


            LLVM.Store s = new LLVM.Store(LLVMModule)
            {
                Type = LLVMTypeName(astMulAssignmentExpression.AssociatedType),
                Value = "%" + (TempCount),
                Pointer = "%" + (TempCount + 1)
            };

            WriteLine(2, s.EmitCode());

            ++TempCount;

            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astMulAssignmentExpression.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            TempCount++;

            int mulResult = TempCount++;

            #endregion


            #region Assignment

            switch (astMulAssignmentExpression.LeftOperand.AssociatedType)
            {
                case "System.Int32":
                    GenerateLeftOperandForMulAssignmentExpressionInt32(astMulAssignmentExpression, mulResult);
                    break;
                case "System.Single":
                    GenerateLeftOperandForMulAssignmentExpressionFloat(astMulAssignmentExpression,mulResult);
                    break;
                case "System.Boolean":
                    GenerateLeftOperandForMulAssignmentExpressionBoolean(astMulAssignmentExpression, mulResult);
                    break;
            }

            #endregion
        }

        private void GenerateLeftOperandForMulAssignmentExpressionFloat(AstMultiplyAssignmentExpression astMulAssignmentExpression, int mulResult)
        {
            AstVariableReference v = (AstVariableReference)astMulAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + mulResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astMulAssignmentExpression.LeftOperand.AssociatedType == "System.Single")
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
                    Value = "%" + mulResult,
                    Pointer = loadPtr.Result,
                    Type = "float"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForMulAssignmentExpressionBoolean(AstMultiplyAssignmentExpression astMulAssignmentExpression, int mulResult)
        {
            AstVariableReference v = (AstVariableReference)astMulAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + mulResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astMulAssignmentExpression.LeftOperand.AssociatedType == "System.Boolean")
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
                    Value = "%" + mulResult,
                    Pointer = loadPtr.Result,
                    Type = "i8"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForMulAssignmentExpressionInt32(AstMultiplyAssignmentExpression astMulAssignmentExpression, int mulResult)
        {
            AstVariableReference v = (AstVariableReference)astMulAssignmentExpression.LeftOperand;

           if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                              {
                                  Value = "%" + mulResult,
                                  Pointer = "%l_" + v.VariableName
                              };

                if (astMulAssignmentExpression.LeftOperand.AssociatedType == "System.Int32")
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
                                           Value = "%" + mulResult,
                                           Pointer = loadPtr.Result,
                                           Type = "i32"
                                       };
               WriteLine(2, storeActual.EmitCode());
           }
        }

        
    }
}
