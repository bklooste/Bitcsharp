using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstPostIncrement : AstStatement
    {
        public IAstExpression AstExpression;

        public AstPostIncrement(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstPostIncrement(IParser parser) : base(parser) { }
        public AstPostIncrement(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstPostIncrement--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        public override string AssociatedType
        {
            get { return AstExpression.AssociatedType; }
            set { throw new LLVMSharpException("You cannot assign AssociatedType"); }
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override void Walk(Walker walker)
        {
            // todo walker
        }
    }
}
