/*
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.LLVM
{
    public partial class LLVMCodeGenerator
    {

        private void GenerateAstAdditionExpression(AstAdditionExpression astAdditionExpression)
        {
            #region generate left operand

            GenerateExpressionAndLoad(astAdditionExpression.LeftOperand);

            int leftResult = TempCount++;

            #endregion

            #region generate right operand

            GenerateExpressionAndLoad(astAdditionExpression.RightOperand);

            int rightOperand = TempCount++;

            #endregion

            #region add

            Add add = new Add(this)
                        {
                            Operand1 = "%" + leftResult,
                            Operand2 = "%" + rightOperand,
                            Result = "%" + (TempCount),
                            Type = LLVMTypeName(astAdditionExpression.AssociatedType)
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
