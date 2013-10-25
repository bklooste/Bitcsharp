using System.Text;
using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    ///  &lt;result> = BitCast &lt;type>* &lt;pointer>
    /// </summary>
    public class BitCast : Instruction
    {
        public string Result;
        public string Type1;
        public string Value;
        public string Type2;

        public BitCast(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Result);
            sb.Append(" = bitcast ");
            sb.Append(Type1);
            sb.Append(" ");
            sb.Append(Value);
            sb.Append(" to ");
            sb.Append(Type2);

            return sb.ToString();
        }
    }
}
