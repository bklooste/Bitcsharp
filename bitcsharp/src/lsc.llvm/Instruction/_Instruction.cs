
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public abstract class Instruction : ModulePart, ILLVMCodeGenerator
    {
        public Instruction(Module module)
            : base(module)
        {
        }

        //public Instruction(LLVMSharpCodeGenerator llvmCodeGenerator)
        //    : this(llvmCodeGenerator.LLVMModule)
        //{
        //}

        public abstract string EmitCode();
    }
}