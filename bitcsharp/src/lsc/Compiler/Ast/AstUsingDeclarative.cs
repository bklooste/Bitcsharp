using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstUsingDeclarative : AstNode
    {
        public AstUsingDeclarative(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstUsingDeclarative(IParser parser) : base(parser) { }

        public string Namespace;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstUsingDeclarative--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Namespace: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                Namespace);
        }

    }

    public class AstUsingDeclarativeCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstUsingDeclarative))
                throw new ArgumentException("You can add type of only AstUsingDeclarative.");
            return base.Add(value);
        }

        public int Add(AstUsingDeclarative value)
        {
            return base.Add(value);
        }
    }
}
