using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public abstract class ModulePart
    {
        public Module Module;

        public ModulePart(Module module)
        {
            Module = module;
        }

        //public ModulePart(LLVMSharpCodeGenerator llvmCodeGenerator)
        //    : this(llvmCodeGenerator.LLVMModule)
        //{
        //}
    }
}