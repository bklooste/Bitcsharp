using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstVirtualMemberModifier : AstMemberModifier
    {
        public AstVirtualMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstVirtualMemberModifier(IParser parser) : base(parser) { }

        public AstVirtualMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string Name
        {
            get { return "virtual"; }
        }
    }

}
