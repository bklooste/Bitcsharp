using System;
using System.Diagnostics;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public abstract partial class CodeGenerator
    {
        [DebuggerStepThrough]
        public virtual void Write(string str)
        {
            Writer.Write(str);
        }

        [DebuggerStepThrough]
        public virtual void WriteLine()
        {
            Write(NewLine);
        }

        [DebuggerStepThrough]
        public virtual void WriteLine(string str)
        {
            Write(str);
            WriteLine();
        }
        
        [DebuggerStepThrough]
        public virtual void Write(int indentTimes, string str)
        {
            WriteIndentSpace(indentTimes);
            Write(str);
        }

        [DebuggerStepThrough]
        public virtual void WriteLine(int indentTimes, string str)
        {
            Write(indentTimes, str);
            WriteLine();
        }

        [DebuggerStepThrough]
        public virtual void Write(int no)
        {
            Write(no.ToString());
        }

        [DebuggerStepThrough]
        public virtual void WriteLine(int no)
        {
            WriteLine(no.ToString());
        }
    }
}
