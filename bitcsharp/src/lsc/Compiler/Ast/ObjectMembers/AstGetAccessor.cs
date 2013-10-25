using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstGetAccessor : AstNode
    {
        public AstBlock AstBlock = null;

        public AstGetAccessor(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstGetAccessor(IParser parser) : base(parser) { }
        public AstGetAccessor(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

    }
}
