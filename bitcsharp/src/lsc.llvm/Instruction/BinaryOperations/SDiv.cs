namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
     //<summary>
     //Returns the quotient of its two operands
     //</summary>
    public class SDiv : BinaryOperation
    {
        public SDiv(Module module)
            : base(module)
        {
        }

        public override string InstructionName
        {
            get { return "sdiv"; }
        }
    }
}

