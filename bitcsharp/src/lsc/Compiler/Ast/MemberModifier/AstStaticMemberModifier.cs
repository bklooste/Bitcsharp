using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstStaticMemberModifier : AstMemberModifier
    {
        public AstStaticMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstStaticMemberModifier(IParser parser) : base(parser) { }

        public AstStaticMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string Name
        {
            get { return "static"; }
        }
    }

}
