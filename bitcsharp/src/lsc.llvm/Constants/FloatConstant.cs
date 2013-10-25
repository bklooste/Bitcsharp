
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class FloatConstant : Constant
    {
        
        public float FloatValue
        {
            get { return (float)Value; }
            set { Value = value; }
        }

        public override string EmitCode()
        {
            return "float " + Value;
        }
    }
}
