using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstPostDecrement astPostDecrement)
        {
            AstVariableReference varRef = (AstVariableReference)astPostDecrement.AstExpression;
            astPostDecrement.AstExpression.EmitCode(this);
            int loadedTempCount = TempCount++;
            switch (astPostDecrement.AssociatedType)
            {
                case "System.Int32":
                    GeneratePostDecrementInt32(varRef, loadedTempCount);
                    break;
                case "System.Single":
                    GeneratePostDecrementSingle(varRef, loadedTempCount);
                    break;

            }

        }


        private void GeneratePostDecrementSingle(AstVariableReference varRef, int loadedTempCount)
        {
            Alloca allocaTemp = new Alloca(LLVMModule)
            {
                Type = "float",
                Result = "%" + TempCount
            };
            WriteLine(2, allocaTemp.EmitCode());

            if (varRef.MemberRefCollection.Count == 0)
            {
                Store storeTemp = new Store(LLVMModule)
                {
                    Type = "float",
                    Value = "%" + loadedTempCount,
                    Pointer = allocaTemp.Result
                };
                WriteLine(2, storeTemp.EmitCode());

                Sub sub = new Sub(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "float",
                    Operand1 = "%" + loadedTempCount,
                    Operand2 = "1.0"
                };
                WriteLine(2, sub.EmitCode());
                ++TempCount;


                Store s = new Store(LLVMModule)
                {
                    Type = "float",
                    Value = sub.Result
                };

               // if (varRef.IsLocalVar())
                s.Pointer = "%l_" + varRef.VariableName;
                WriteLine(2, s.EmitCode());
                ++TempCount;


                Load l = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "float",
                    Pointer = allocaTemp.Result
                };

                WriteLine(2, l.EmitCode());

            }
            else // varRef.MemberRefCollection.Count > 0
            {
                WriteCommentLine(loadedTempCount);

                Alloca a = new Alloca(LLVMModule)
                {
                    Type = "float*",
                    Result = "%" + (TempCount + 1)
                };
                WriteLine(2, a.EmitCode());
                ++TempCount;

                Store s = new Store(LLVMModule)
                {
                    Value = "%" + (loadedTempCount - 1),
                    Pointer = "%" + TempCount,
                    Type = "float*"
                };
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load loadPtr = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "float*",
                    Pointer = a.Result
                };
                WriteLine(2, loadPtr.EmitCode());

                Store storeTemp = new Store(LLVMModule)
                {
                    Type = "float",
                    Value = "%" + loadedTempCount,
                    Pointer = allocaTemp.Result
                };
                WriteLine(2, storeTemp.EmitCode());

                Sub sub = new Sub(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "float",
                    Operand1 = "%" + loadedTempCount,
                    Operand2 = "1.0"
                };
                WriteLine(2, sub.EmitCode());
                ++TempCount;

                Store storeActual = new Store(LLVMModule)
                {
                    Value = sub.Result,
                    Pointer = loadPtr.Result,
                    Type = "float"
                };
                WriteLine(2, storeActual.EmitCode());
                ++TempCount;

                Load l = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "float",
                    Pointer = allocaTemp.Result
                };
                WriteLine(2, l.EmitCode());
            }
           
        }

        private void GeneratePostDecrementInt32(AstVariableReference varRef, int loadedTempCount)
        {
            Alloca allocaTemp = new Alloca(LLVMModule)
            {
                Type = "i32",
                Result = "%" + TempCount
            };
            WriteLine(2, allocaTemp.EmitCode());

            if (varRef.MemberRefCollection.Count == 0)
            {
                Store storeTemp = new Store(LLVMModule)
                {
                    Type = "i32",
                    Value = "%" + loadedTempCount,
                    Pointer = allocaTemp.Result
                };
                WriteLine(2, storeTemp.EmitCode());

                Sub sub = new Sub(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "i32",
                    Operand1 = "%" + loadedTempCount,
                    Operand2 = "1"
                };
                WriteLine(2, sub.EmitCode());
                ++TempCount;

                Store s = new Store(LLVMModule)
                {
                    Type = "i32",
                    Value = sub.Result
                };

               // if (varRef.IsLocalVar())
                s.Pointer = "%l_" + varRef.VariableName;
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load l = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "i32",
                    Pointer = allocaTemp.Result
                };
                WriteLine(2, l.EmitCode());
            }
            else // varRef.MemberRefCollection.Count > 0
            {
                WriteCommentLine(loadedTempCount);

                Alloca a = new Alloca(LLVMModule)
                {
                    Type = "i32*",
                    Result = "%" + (TempCount + 1)
                };
                WriteLine(2, a.EmitCode());
                ++TempCount;

                Store s = new Store(LLVMModule)
                {
                    Value = "%" + (loadedTempCount - 1),
                    Pointer = "%" + TempCount,
                    Type = "i32*"
                };
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load loadPtr = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "i32*",
                    Pointer = a.Result
                };
                WriteLine(2, loadPtr.EmitCode());

                Store storeTemp = new Store(LLVMModule)
                {
                    Type = "i32",
                    Value = "%" + loadedTempCount,
                    Pointer = allocaTemp.Result
                };
                WriteLine(2, storeTemp.EmitCode());

                Sub sub = new Sub(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "i32",
                    Operand1 = "%" + loadedTempCount,
                    Operand2 = "1"
                };
                WriteLine(2, sub.EmitCode());
                ++TempCount;

                Store storeActual = new Store(LLVMModule)
                {
                    Value = sub.Result,
                    Pointer = loadPtr.Result,
                    Type = "i32"
                };
                WriteLine(2, storeActual.EmitCode());
                ++TempCount;

                Load l = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "i32",
                    Pointer = allocaTemp.Result
                };
                WriteLine(2, l.EmitCode());
            }
        }

    }

}