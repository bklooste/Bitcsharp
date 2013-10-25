using System.Diagnostics;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public abstract partial class CodeGenerator
    {
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebug(string str)
        {
            Write(str);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebug(int no)
        {
            Write(no);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebugLine(string str)
        {
            WriteLine(str);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebugLine(int no)
        {
            WriteLine(no);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebug(int indentTimes, string str)
        {
            WriteIndentSpace(indentTimes);
            Write(str);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebugLine(int indentTimes, string str)
        {
            Write(indentTimes, str);
            WriteLine();
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebugComment(string comment)
        {
            WriteComment(comment);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebugCommentLine(string comment)
        {
            WriteCommentLine(comment);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public virtual void WriteDebugComment(int indentTimes, string comment)
        {
            WriteIndentSpace(indentTimes);
            WriteComment(comment);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public void WriteDebugCommentLine(int indentTimes, int no)
        {
            WriteIndentSpace(indentTimes);
            WriteCommentLine(no);
        }
    }
}
