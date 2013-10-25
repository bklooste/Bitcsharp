
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    /// ret void
    /// </summary>
    public class ReturnVoid : Return
    {

        public ReturnVoid(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            return "ret void";
        }
    }
}
