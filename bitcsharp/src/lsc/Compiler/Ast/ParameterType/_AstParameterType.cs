using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstParameterType : AstNode
    {
        public AstParameterType(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstParameterType(IParser parser) : base(parser) { }
        public AstParameterType(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }
    }
}
