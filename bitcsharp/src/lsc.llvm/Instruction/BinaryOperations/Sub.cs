
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class Sub : BinaryOperation
    {
        public Sub(Module module)
            : base(module)
        {
        }
        
        public override string InstructionName
        {
            get { return "sub"; }
        }
    }
}
