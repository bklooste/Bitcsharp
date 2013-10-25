using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstSetAccessor : AstNode
    {

        public AstBlock AstBlock = null;

        public AstSetAccessor(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstSetAccessor(IParser parser) : base(parser) { }
        public AstSetAccessor(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

    }
}
