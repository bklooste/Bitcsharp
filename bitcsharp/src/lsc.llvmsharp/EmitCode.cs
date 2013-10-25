namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode()
        {
            WriteCommentLine(" output-format = 'llvm-2.5'");

            EmitCodeForIntegerTable();
            EmitCodeForStringTable();
            EmitRuntimeLibraryObjects();

            EmitCodeForEnum();

            EmitCodeForObjectLayout();

            EmitCodeForEntryPoint();

            EmitCodeForMethods();

            EmitCodeForExternMethods();
            EmitLLVMMethods();

        }

        private void EmitRuntimeLibraryObjects()
        {
            WriteLine(1, "%struct.__llvmsharp_stringHeader = type { i32, i32 }");
            WriteLine(1, "%struct.__llvmsharp_gc_root = type { i8**, %struct.__llvmsharp_gc_root*, %struct.__llvmsharp_gc_root* }");
        }



    }
}