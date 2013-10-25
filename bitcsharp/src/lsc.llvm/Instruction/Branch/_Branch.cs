

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    ///  Branch Instruction
    /// </summary>
    public abstract class Branch : Instruction
    {
        public Branch(Module module)
            : base(module)
        {
        }
    }
}
