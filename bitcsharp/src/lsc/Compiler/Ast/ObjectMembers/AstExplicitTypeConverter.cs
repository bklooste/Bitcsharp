using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstExplicitTypeConverter : AstTypeConverter
    {
        public AstExplicitTypeConverter(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstExplicitTypeConverter(IParser parser) : base(parser) { }
        public AstExplicitTypeConverter(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstExplicitTypeConverter--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("From: {4}{0}");
            sb.Append("To: {5}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                base.AstParameter.Type, base.ReturnType);
        }
    }
}
