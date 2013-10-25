
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class I32Constant : Constant
    {
        //public I32Constant(Module module)
        //    : base(module)
        //{
        //}

        public int Int32Value
        {
            get { return (int)Value; }
            set { Value = value; }
        }

        public override string EmitCode()
        {
            return "i32 " + Value;
        }
    }
}
