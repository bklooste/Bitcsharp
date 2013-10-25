using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstNull : AstNode, IAstExpression
    {
        public string FQT;

        public AstNull(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstNull(IParser parser) : base(parser) { }

        public AstNull(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstNull--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}");
            sb.Append("Type: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                FQT);
        }

        public void Walk(Walker walker)
        {
            //todo walker
        }

        public void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public string AssociatedType
        {
            get { return "System.Object"; }
            set { throw new LLVMSharpException("You cannot assign AssociatedType"); }
        }

    }
}
