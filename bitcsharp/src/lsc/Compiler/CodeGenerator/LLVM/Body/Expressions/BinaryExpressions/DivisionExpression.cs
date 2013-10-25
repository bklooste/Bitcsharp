/*
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.LLVM
{
    public partial class LLVMCodeGenerator
    {

        private void GenerateAstDivisionExpression(AstDivisionExpression astDivisionExpression)
        {
            #region generate left operand

            GenerateExpressionAndLoad(astDivisionExpression.LeftOperand);

            int leftResult = TempCount++;

            #endregion

            #region generate right operand

            GenerateExpressionAndLoad(astDivisionExpression.RightOperand);

            int rightOperand = TempCount++;

            #endregion

            #region div

            SDiv add = new SDiv(this)
                        {
                            Operand1 = "%" + leftResult,
                            Operand2 = "%" + rightOperand,
                            Result = "%" + (TempCount),
                            Type = LLVMTypeName(astDivisionExpression.AssociatedType)
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
