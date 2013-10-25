using System;
using System.Collections.Generic;
using System.Text;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstSubtractAssignmentExpression astSubAssignmentExpression)
        {
            WriteIndentSpace(3);
            WriteInfoCommentLine(string.Format("{0} -= {1}", astSubAssignmentExpression.AssociatedType,
                                               astSubAssignmentExpression.RightOperand.AssociatedType));

            astSubAssignmentExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astSubAssignmentExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;


            #region Sub

            LLVM.Sub sub = new LLVM.Sub(LLVMModule)
            {
                Operand1 = "%" + leftResult,
                Operand2 = "%" + rightResult,
                Result = "%" + (TempCount),
                Type = LLVMTypeName(astSubAssignmentExpression.AssociatedType)
            };

            WriteLine(2, sub.EmitCode());

            LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astSubAssignmentExpression.AssociatedType)
            };

            WriteLine(2, alloc.EmitCode());


            LLVM.Store s = new LLVM.Store(LLVMModule)
            {
                Type = LLVMTypeName(astSubAssignmentExpression.AssociatedType),
                Value = "%" + (TempCount),
                Pointer = "%" + (TempCount + 1)
            };

            WriteLine(2, s.EmitCode());

            ++TempCount;

            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astSubAssignmentExpression.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            TempCount++;

            int subResult = TempCount++;
            
            #endregion

            #region Assignment

            switch (astSubAssignmentExpression.LeftOperand.AssociatedType)
            {
                case "System.Int32":
                    GenerateLeftOperandForSubAssignmentExpressionInt32(astSubAssignmentExpression, subResult);
                    break;
                case "System.Single":
                    GenerateLeftOperandForSubAssignmentExpressionFloat(astSubAssignmentExpression, subResult);
                    break;
                case "System.Boolean":
                    GenerateLeftOperandForSubAssignmentExpressionBoolean(astSubAssignmentExpression, subResult);
                    break;
            }

            #endregion
        }

        private void GenerateLeftOperandForSubAssignmentExpressionFloat(AstSubtractAssignmentExpression astSubAssignmentExpression, int subResult)
        {
            AstVariableReference v = (AstVariableReference)astSubAssignmentExpression.LeftOperand;
            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + subResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astSubAssignmentExpression.LeftOperand.AssociatedType == "System.Single")
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
                    Value = "%" + subResult,
                    Pointer = loadPtr.Result,
                    Type = "float"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForSubAssignmentExpressionBoolean(AstSubtractAssignmentExpression astSubAssignmentExpression, int subResult)
        {
            AstVariableReference v = (AstVariableReference)astSubAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + subResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astSubAssignmentExpression.LeftOperand.AssociatedType == "System.Boolean")
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
                    Value = "%" + subResult,
                    Pointer = loadPtr.Result,
                    Type = "i8"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForSubAssignmentExpressionInt32(AstSubtractAssignmentExpression astSubAssignmentExpression, int subResult)
        {
            AstVariableReference v = (AstVariableReference)astSubAssignmentExpression.LeftOperand;

             if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                              {
                                  Value = "%" + subResult,
                                  Pointer = "%l_" + v.VariableName
                              };

                if (astSubAssignmentExpression.LeftOperand.AssociatedType == "System.Int32")
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
                                             Value = "%" + subResult,
                                             Pointer = loadPtr.Result,
                                             Type = "i32"
                                         };
                 WriteLine(2, storeActual.EmitCode());
             }
        }        
    }
}
