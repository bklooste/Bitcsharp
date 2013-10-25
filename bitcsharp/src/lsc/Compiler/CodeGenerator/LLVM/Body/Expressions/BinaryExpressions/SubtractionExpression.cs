/*
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.LLVM
{
    public partial class LLVMCodeGenerator
    {

        private void GenerateAstSubtractionExpression(AstSubtractionExpression astSubtractionExpression)
        {
            #region generate left operand

            GenerateExpressionAndLoad(astSubtractionExpression.LeftOperand);

            int leftResult = TempCount++;

            #endregion

            #region generate right operand

            GenerateExpressionAndLoad(astSubtractionExpression.RightOperand);

            int rightOperand = TempCount++;

            #endregion

            #region sub

            Sub add = new Sub(this)
                        {
                            Operand1 = "%" + leftResult,
                            Operand2 = "%" + rightOperand,
                            Result = "%" + (TempCount),
                            Type = LLVMTypeName(astSubtractionExpression.AssociatedType)
                        };

            WriteLine(2, add.GenerateCode());

            Alloca alloc = new Alloca(this)
            {
                Result = "%" + (TempCount + 1),
                Type = "i32"
            };
            WriteLine(2, alloc.GenerateCode());

            Store s = new Store(this)
            {
                Type = "i32",
                Value = "%" + (TempCount),
                Pointer = "%" + (TempCount + 1)
            };

            WriteLine(2, s.GenerateCode());

            ++TempCount;
            #endregion
        }

    }
}
*/
