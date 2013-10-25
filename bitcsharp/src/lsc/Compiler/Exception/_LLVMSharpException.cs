using System;

namespace LLVMSharp.Compiler
{
    public class LLVMSharpException : Exception
    {
        public LLVMSharpException() { }

        public LLVMSharpException(string message) : base(message) { }
    }
}
