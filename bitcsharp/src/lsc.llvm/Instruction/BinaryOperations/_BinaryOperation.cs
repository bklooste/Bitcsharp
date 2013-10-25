using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public abstract class BinaryOperation : Instruction
    {
        public string Result;
        public string Operand1;
        public string Operand2;
        public string Type;

        public abstract string InstructionName { get; }

        public BinaryOperation(Module module)
            : base(module)
        {
        }


        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} = {1} {2} {3},{4}", Result, InstructionName, Type, Operand1, Operand2);

            return sb.ToString();
        }
    }
}
