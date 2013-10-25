using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstPrivateMemberModifier : AstMemberModifier
    {
        public AstPrivateMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstPrivateMemberModifier(IParser parser) : base(parser) { }

        public AstPrivateMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string Name
        {
            get { return "private"; }
        }
    }

}
