using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstMuliplicationExpression astMultiplicationExpression)
        {
            astMultiplicationExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;
           
            astMultiplicationExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            #region Mul
            
            LLVM.Mul mul = new LLVM.Mul(LLVMModule)
            {
                Operand1 = "%" + leftResult,
                Operand2 = "%" + rightResult,
                Result = "%" + (TempCount),
                Type = LLVMTypeName(astMultiplicationExpression.AssociatedType)
            };

            WriteLine(2, mul.EmitCode());

            LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
                        {
                            Result = "%" + (TempCount + 1),
                            Type = LLVMTypeName(astMultiplicationExpression.AssociatedType)
                        };

            WriteLine(2, alloc.EmitCode());


            LLVM.Store s = new LLVM.Store(LLVMModule)
                        {
                            Type = LLVMTypeName(astMultiplicationExpression.AssociatedType),
                            Value = "%" + (TempCount),
                            Pointer = "%" + (TempCount + 1)
                        };

            WriteLine(2, s.EmitCode());

            ++TempCount;

            #region load

            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astMultiplicationExpression.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            TempCount++;
            
            #endregion

            #endregion 
        }
    }
}