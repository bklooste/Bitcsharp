using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class Alloca : Instruction
    {
        public string Result;
        public string Type;

        public Alloca(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Result);
            sb.Append(" = alloca ");
            sb.Append(Type);

            return sb.ToString();
        }
    }
}
