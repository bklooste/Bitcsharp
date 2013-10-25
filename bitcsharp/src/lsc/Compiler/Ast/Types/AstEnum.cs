using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstEnum : AstType
    {
        public AstEnumType AstEnumType;
        public AstEnumMemberCollection AstEnumMemberCollection = new AstEnumMemberCollection();

        public AstEnum(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstEnum(IParser parser) : base(parser) { }
        public AstEnum(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstEnum--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Enum Name: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                base.Name);
        }

        AstMemberModifierCollection _astMemberModifierCollection = new AstMemberModifierCollection();
        public override AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return _astMemberModifierCollection; }
            set { _astMemberModifierCollection = value; }
        }

        public override void CheckEntryPoint(LLVMSharpCompiler compiler)
        {
            // do nothing
        }

        public override void Walk(Walker walker)
        {
            walker.Walk(this);
        }
    }
}
