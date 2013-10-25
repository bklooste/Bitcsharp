
namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        private void EmitCodeForEntryPoint()
        {
            switch (Compiler.Options.Target)
            {
                case Target.Exe:
                    GenerateEntryPointForExe();
                    break;
                case Target.Library:
                    throw new LLVMSharpException("Target type library not supported.");
                case Target.WinExe:
                    throw new LLVMSharpException("Target type WinExe not supported.");
            }
            WriteLine();
        }

        private void GenerateEntryPointForExe()
        {
            // todo: make it better
            WriteInfoCommentLine(" Entry Point");
            WriteLine(1, "define i32 @main(i32 %argc, i8** %argv) {");
            WriteLine(1, "entry:");
            WriteLine(2, "%argc_addr = alloca i32");
            WriteLine(2, "%argv_addr = alloca i8**");
            WriteLine(2, "%retval = alloca i32");
            WriteLine(2, "%0 = alloca i32");
            WriteLine(2, "store i32 %argc, i32* %argc_addr");
            WriteLine(2, "store i8** %argv, i8*** %argv_addr");
            WriteLine(2, "%1 = load i32* %argc_addr");
            WriteLine(2, "%2 = load i8*** %argv_addr");
            WriteLine(2, "call void @__llvmsharp_init(i32 %1, i8** %2)");

            #region CallMain Method of C#

            Write(2, "call ");
            Write(LLVMTypeName(Compiler.EntryPoint.FullQReturnType));
            Write(" @");
            Write(PREFIX);
            Write(Mangler.Instance.MangleName(Compiler.EntryPointAstType.FullQualifiedName));
            //Write(Mangler.Instance.MangleName(Compiler.EntryPointFQType));
            Write(Mangler.Instance.MangleMethod(Compiler.EntryPoint, false));
            WriteLine("()");

            #endregion

            WriteLine(2, "store i32 0, i32* %0, align 4");
            WriteLine(2, "%3 = load i32* %0, align 4");
            WriteLine(2, "store i32 %3, i32* %retval");
            WriteLine(2, "br label %return");
            WriteLine();
            WriteLine(1, "return:");
            WriteLine(2, "%retval1 = load i32* %retval");
            WriteLine(2, "ret i32 %retval1");
            WriteLine(1, "}");


            WriteLine();
            WriteLine(1, "declare void @__llvmsharp_init(i32, i8**)");
        }

    }
}