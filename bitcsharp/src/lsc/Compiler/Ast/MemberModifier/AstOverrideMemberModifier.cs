using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstOverrideMemberModifier : AstMemberModifier
    {
        public AstOverrideMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstOverrideMemberModifier(IParser parser) : base(parser) { }

        public AstOverrideMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string Name
        {
            get { return "override"; }
        }
    }

}
