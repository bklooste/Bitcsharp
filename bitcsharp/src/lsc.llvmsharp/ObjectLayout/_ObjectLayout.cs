using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        private void EmitCodeForObjectLayout()
        {
            foreach (string i in Compiler.StructHashtable.Keys)
                EmitCodeForObjectLayout((AstStruct) Compiler.StructHashtable[i]);

            foreach (string i in Compiler.ClassHashtable.Keys)
                EmitCodeForObjectLayout((AstClass) Compiler.ClassHashtable[i]);
        }
    }
}