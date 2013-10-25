/*
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.LLVM
{
    public partial class LLVMCodeGenerator
    {

        private void GenerateAstConstant(AstConstant astConstant)
        {
            if (astConstant is AstIntegerConstant)
                GenerateAstIntegerConstant((AstIntegerConstant) astConstant);
        }

        private void GenerateAstIntegerConstant(AstIntegerConstant astIntegerConstant)
        {
            Alloca a = new Alloca(this)
            {
                Result = "%" + TempCount,
                Type = "i32"
            };
            WriteLine(2, a.GenerateCode());

            Store s = new Store(this)
            {
                Type = "i32",
                Value = astIntegerConstant.ConstantValue.ToString(),
                Pointer = "%" + TempCount
            };

            WriteLine(2, s.GenerateCode());
        }
    }
}
*/
