using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    /// ret &lt;type> &lt;value> 
    /// </summary>
    public class Return : Instruction
    {
        public string Type;
        public string Value;

        public Return(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("ret {0} {1}", Type, Value);

            return sb.ToString();
        }
    }
}
