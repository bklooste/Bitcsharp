
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    ///   &lt;result> = xor &lt;ty> &ltop1>, &lt;op2>
    /// </summary>
    public class Zext : ConversionOperation
    {
        public Zext(Module module)
            : base(module)
        {
        }

        public override string InstructionName
        {
            get { return "zext"; }
        }
    }
}