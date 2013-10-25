
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class Add : BinaryOperation
    {
        public Add(Module module)
            : base(module)
        {
        }
        
        public override string InstructionName
        {
            get { return "add"; }
        }
    }
}
