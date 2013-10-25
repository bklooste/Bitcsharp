using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        private void EmitCodeForObjectLayout(AstStruct astStruct)
        {
            //todo
            if (astStruct.FullQualifiedName == "System.Int32")
                return;
        }
    }
}