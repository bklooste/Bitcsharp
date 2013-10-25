using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstPublicMemberModifier : AstMemberModifier
    {
        public AstPublicMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstPublicMemberModifier(IParser parser) : base(parser) { }

        public AstPublicMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string Name
        {
            get { return "public"; }
        }
    }

}
