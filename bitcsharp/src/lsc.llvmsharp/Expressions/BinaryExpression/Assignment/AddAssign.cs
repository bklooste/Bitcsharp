using System;
using System.Collections.Generic;
using System.Text;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstAddAssignmentExpression astAddAssignmentExpression)
        {
            WriteIndentSpace(3);
            WriteInfoCommentLine(string.Format("{0} += {1}", astAddAssignmentExpression.AssociatedType,
                                               astAddAssignmentExpression.RightOperand.AssociatedType));

            astAddAssignmentExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astAddAssignmentExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;


            #region Add

            LLVM.Add a = new LLVM.Add(LLVMModule)
            {
                Operand1 = "%" + leftResult,
                Operand2 = "%" + rightResult,
                Result = "%" + (TempCount),
                Type = LLVMTypeName(astAddAssignmentExpression.AssociatedType)
            };

            WriteLine(2, a.EmitCode());

            LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astAddAssignmentExpression.AssociatedType)
            };

            WriteLine(2, alloc.EmitCode());


            LLVM.Store s = new LLVM.Store(LLVMModule)
            {
                Type = LLVMTypeName(astAddAssignmentExpression.AssociatedType),
                Value = "%" + (TempCount),
                Pointer = "%" + (TempCount + 1)
            };

            WriteLine(2, s.EmitCode());

            ++TempCount;

            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astAddAssignmentExpression.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            TempCount++;


            int addResult = TempCount++;

            #endregion




            #region Assignment

            switch (astAddAssignmentExpression.LeftOperand.AssociatedType)
            {
                case "System.Int32":
                    GenerateLeftOperandForAddAssignmentExpressionInt32(astAddAssignmentExpression, addResult);
                    break;
                case "System.Single":
                    GenerateLeftOperandForAddAssignmentExpressionFloat(astAddAssignmentExpression, addResult);
                    break;
                case "System.Boolean":
                    GenerateLeftOperandForAddAssignmentExpressionBoolean(astAddAssignmentExpression, addResult);
                    break;
            }

            #endregion
        }

        private void GenerateLeftOperandForAddAssignmentExpressionFloat(AstAddAssignmentExpression astAddAssignmentExpression, int addResult)
        {

            AstVariableReference v = (AstVariableReference)astAddAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + addResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astAddAssignmentExpression.LeftOperand.AssociatedType == "System.Single")
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
                    Value = "%" + addResult,
                    Pointer = loadPtr.Result,
                    Type = "float"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForAddAssignmentExpressionBoolean(AstAddAssignmentExpression astAddAssignmentExpression, int addResult)
        {

            AstVariableReference v = (AstVariableReference)astAddAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + addResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astAddAssignmentExpression.LeftOperand.AssociatedType == "System.Boolean")
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
                    Value = "%" + addResult,
                    Pointer = loadPtr.Result,
                    Type = "i8"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForAddAssignmentExpressionInt32(AstAddAssignmentExpression astAddAssignmentExpression, int addResult)
        {
            AstVariableReference v = (AstVariableReference)astAddAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                              {
                                  Value = "%" + addResult,
                                  Pointer = "%l_" + v.VariableName
                              };

                if (astAddAssignmentExpression.LeftOperand.AssociatedType == "System.Int32")
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
                                  Value = "%" + addResult,
                                  Pointer = loadPtr.Result,
                                  Type = "i32"
                              };
                WriteLine(2, storeActual.EmitCode());
            }
        }
    }
}
