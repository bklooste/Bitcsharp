using System.Diagnostics;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public abstract partial class CodeGenerator
    {
        [DebuggerStepThrough]
        public virtual void WriteIndentSpace()
        {
            Write(INDENT_SPACE);
        }

        [DebuggerStepThrough]
        public virtual void WriteIndentSpace(int times)
        {
            for (int i = 0; i < times; i++)
                WriteIndentSpace();
        }
    }
}
