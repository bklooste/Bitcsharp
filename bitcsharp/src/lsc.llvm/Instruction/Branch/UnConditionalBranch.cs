namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    /// br label &lt;dest>
    /// </summary>
    public class UnConditionalBranch : Branch
    {
        public string Destination;

        public UnConditionalBranch(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            return string.Format("br label %{0}", Destination);
        }
    }
}