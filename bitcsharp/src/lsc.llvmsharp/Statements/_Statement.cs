using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstStatementCollection astStatementCollection)
        {
            foreach (AstStatement i in astStatementCollection)
            {
                if (i != null)
                {
                    i.EmitCode(this);

                    if (i is AstPostIncrement || i is AstPostDecrement)
                        ++TempCount;
                    else if (i is AstMethodCall)
                    {
                        AstMethodCall x = (AstMethodCall)i;
                        if (x.AssociatedType != "System.Void")
                            ++TempCount;
                    }
                }
            }
        }
    }
}