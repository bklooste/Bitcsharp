using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstSealedMemberModifier : AstMemberModifier
    {
        public AstSealedMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstSealedMemberModifier(IParser parser) : base(parser) { }

        public AstSealedMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string Name
        {
            get { return "sealed"; }
        }
    }

}
