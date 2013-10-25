using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstProtectedMemberModifier : AstMemberModifier
    {
        public AstProtectedMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstProtectedMemberModifier(IParser parser) : base(parser) { }

        public AstProtectedMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string Name
        {
            get { return "protected"; }
        }
    }

}
