

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public abstract class Constant : ILLVMCodeGenerator
    {
        public object Value;

        public abstract string EmitCode();
    }
}
