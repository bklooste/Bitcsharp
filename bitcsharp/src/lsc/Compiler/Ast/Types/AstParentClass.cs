using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstParentClass : AstNode, IAssociatedType
    {
        public string Name;
        public string FullQualifiedName;

        public AstParentClass(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstParentClass(IParser parser) : base(parser) { }
        public AstParentClass(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstParentClass--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Class Name: {4}{0}");
            sb.Append("Full Name: {5}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
               Name, FullQualifiedName);
        }

        public string AssociatedType
        {
            get { return FullQualifiedName; }
            set { FullQualifiedName = value; }
        }
    }
}
