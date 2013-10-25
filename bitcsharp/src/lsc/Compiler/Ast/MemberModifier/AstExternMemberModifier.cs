using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstExternMemberModifier : AstMemberModifier
    {
        public AstExternMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstExternMemberModifier(IParser parser) : base(parser) { }

        public AstExternMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string Name
        {
            get { return "extern"; }
        }
    }

}
