using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstReturn astReturn)
        {
            if (astReturn.AstExpression != null)
            {
                // todo if astReturn contains expression
                astReturn.AstExpression.EmitCode(this);

                Store s = new Store(LLVMModule)
                            {
                                Value = "%" + TempCount,
                                Pointer = "%retval"
                            };
                s.Type = LLVMTypeNamePtr(astReturn.AstExpression.AssociatedType);

                WriteLine(2, s.EmitCode());
                ++TempCount;
            }

            UnConditionalBranch gotoEnd = new UnConditionalBranch(LLVMModule)
            {
                Destination = "return"
            };

            System.Convert.ToInt32("1");
            WriteLine(2, gotoEnd.EmitCode());
            ++TempCount; // seems weird why llvm-as can't compile. ++TempCount to fix it.
        }
    }
}