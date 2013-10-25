using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using System.Collections;

namespace LLVMSharp.Compiler.Ast
{
    public class AstEnumMember : AstNode, IAssociatedType
    {
        public string Name;
        public string FullQualifiedName;

        public AstEnumMember(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstEnumMember(IParser parser) : base(parser) { }
        public AstEnumMember(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstEnumMember--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("EnumMember Name: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                this.Name);
        }

        public string AssociatedType
        {
            get { return FullQualifiedName; }
            set { FullQualifiedName = value; }
        }
    }

    public class AstEnumMemberCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstEnumMember))
                throw new ArgumentException("You can add type of only AstEnumMember.");
            return base.Add(value);
        }

        public int Add(AstEnumMember value)
        {
            return base.Add(value);
        }
    }
}
