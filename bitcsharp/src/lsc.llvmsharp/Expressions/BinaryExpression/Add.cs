using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstAdditionExpression astAdditionExpression)
        {
            astAdditionExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astAdditionExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            if (astAdditionExpression.AssociatedType == "System.String")
            {
                ConcatenateStrings(leftResult, rightResult, astAdditionExpression);
                return;
            }

            #region Add

            LLVM.Add add = new LLVM.Add(LLVMModule)
            {
                Operand1 = "%" + leftResult,
                Operand2 = "%" + rightResult,
                Result = "%" + (TempCount),
                Type = LLVMTypeName(astAdditionExpression.AssociatedType)
            };

            WriteLine(2, add.EmitCode());

            LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
                        {
                            Result = "%" + (TempCount + 1),
                            Type = LLVMTypeName(astAdditionExpression.AssociatedType)
                        };

            WriteLine(2, alloc.EmitCode());


            LLVM.Store s = new LLVM.Store(LLVMModule)
                        {
                            Type = LLVMTypeName(astAdditionExpression.AssociatedType),
                            Value = "%" + (TempCount),
                            Pointer = "%" + (TempCount + 1)
                        };

            WriteLine(2, s.EmitCode());

            ++TempCount;

            #region load

            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astAdditionExpression.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            TempCount++;

            #endregion

            #endregion
        }

        private void ConcatenateStrings(int leftResult, int rightResult, AstAdditionExpression astAdditionExpression)
        {
            WriteCommentLine(leftResult + " " + rightResult);

            Call c = new Call(LLVMModule, 2)
                         {
                             FunctionName = "@__llvmsharp_System_String_concat",
                             Result = "%" + TempCount,
                             ReturnType = LLVMTypeNamePtr("System.String")
                         };

            c.Arguments[0] = "%struct.__llvmsharp_stringHeader* %" + leftResult;
            c.Arguments[1] = "%struct.__llvmsharp_stringHeader* %" + rightResult;
            WriteLine(2, c.EmitCode());
        }
    }
}