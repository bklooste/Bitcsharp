

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    ///  &lt;label>:
    /// </summary>
    public class Label : Instruction
    {
        public string Name;

        public Label(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            return string.Format("{0}:", Name);
        }
    }
}
