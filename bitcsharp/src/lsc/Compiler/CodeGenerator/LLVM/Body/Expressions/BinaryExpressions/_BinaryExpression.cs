/*
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.LLVM
{
    public partial class LLVMCodeGenerator
    {
        protected void GenerateAstBinaryExpression(AstBinaryExpression astBinaryExpression)
        {
            if (astBinaryExpression is AstAdditionExpression)
                GenerateAstAdditionExpression((AstAdditionExpression)astBinaryExpression);
            else if (astBinaryExpression is AstSubtractionExpression)
                GenerateAstSubtractionExpression((AstSubtractionExpression)astBinaryExpression);
            else if (astBinaryExpression is AstMuliplicationExpression)
                GenerateAstMultiplicationExpression((AstMuliplicationExpression)astBinaryExpression);
            else if (astBinaryExpression is AstDivisionExpression)
                GenerateAstDivisionExpression((AstDivisionExpression) astBinaryExpression);
        }

        protected void GenerateExpressionAndLoad(IAstExpression iastExprssion)
        {
            GenerateExpression(iastExprssion);

            Load l = new Load(this)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(iastExprssion.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.GenerateCode());

            TempCount++;
        }
    }
}
*/
