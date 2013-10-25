using System.Text;
using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    /// call
    /// </summary>
    public class Call : Instruction
    {
        public string Result;
        public string ReturnType;
        public string FunctionName;
        public string[] Arguments;

        public bool IsSignExt;
        public bool ThrowsException;

        public Call(Module module, int argsCount)
            : base(module)
        {
            Arguments = new string[argsCount];
        }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();

            if (ReturnType == "void")
                sb.AppendFormat("call ");
            else
            {
                sb.Append(Result);
                sb.AppendFormat(" = call ");
            }

            if (IsSignExt)
                sb.Append("signext ");

            sb.Append(ReturnType);
            sb.Append(" ");
            sb.Append(FunctionName);

            sb.Append("(");

            for (int i = 0; i < Arguments.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append(Arguments[i]);
            }

            sb.Append(")");

            if (!ThrowsException)
                sb.Append(" nounwind");

            return sb.ToString();
        }
    }
}
