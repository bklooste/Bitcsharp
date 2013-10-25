

using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    /// br i1 &lt;cond>, label &lt;iftrue>, &label &lt;iffalse>
    /// </summary>
    public class ConditionalBranch : Branch
    {
        public string Condition;
        public string IfTrue;
        public string IfFalse;

        public ConditionalBranch(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("br i1 {0}, label %{1}, label %{2}", Condition, IfTrue, IfFalse);

            return sb.ToString();
        }
    }
}
