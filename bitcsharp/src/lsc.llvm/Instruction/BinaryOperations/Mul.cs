
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class Mul : BinaryOperation
    {
        public Mul(Module module)
            : base(module)
        {
        }
        
        public override string InstructionName
        {
            get { return "mul"; }
        }
    }
}

