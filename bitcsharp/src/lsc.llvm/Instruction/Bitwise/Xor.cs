
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    ///   &lt;result> = xor &lt;ty> &ltop1>, &lt;op2>
    /// </summary>
    public class Xor : BitwiseOperation
    {
        public Xor(Module module)
            : base(module)
        {
        }

        public override string InstructionName
        {
            get { return "xor"; }
        }
    }
}