using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    ///   &l;tresult> = zext &l;ty> &lt;value> to &lt;ty2>
    /// </summary>
    public abstract class ConversionOperation : Instruction
    {
        public string Result;
        public string Type;
        public string Value;
        public string Type2;

        public ConversionOperation(Module module)
            : base(module)
        {
        }

        public abstract string InstructionName { get; }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} = {1} {2} {3} to {4}", Result, InstructionName, Type, Value, Type2);

            return sb.ToString();
        }
    }
}