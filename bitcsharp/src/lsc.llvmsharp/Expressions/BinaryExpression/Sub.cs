using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstSubtractionExpression astSubtractionExpression)
        {
            astSubtractionExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astSubtractionExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            #region Sub

            LLVM.Sub sub = new LLVM.Sub(LLVMModule)
            {
                Operand1 = "%" + leftResult,
                Operand2 = "%" + rightResult,
                Result = "%" + (TempCount),
                Type = LLVMTypeName(astSubtractionExpression.AssociatedType)
            };

            WriteLine(2, sub.EmitCode());

            LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
                        {
                            Result = "%" + (TempCount + 1),
                            Type = LLVMTypeName(astSubtractionExpression.AssociatedType)
                        };

            WriteLine(2, alloc.EmitCode());


            LLVM.Store s = new LLVM.Store(LLVMModule)
                        {
                            Type = LLVMTypeName(astSubtractionExpression.AssociatedType),
                            Value = "%" + (TempCount),
                            Pointer = "%" + (TempCount + 1)
                        };

            WriteLine(2, s.EmitCode());

            ++TempCount;

            #region load

            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astSubtractionExpression.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            ++TempCount;

            #endregion

            #endregion
        }
    }
}