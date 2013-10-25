using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstArrayInitialization : AstStatement
    {
        public AstExpressionCollection AstExpressionCollection = new AstExpressionCollection();
        public IAstExpression AstExpression;
        public AstArrayInitialization(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstArrayInitialization(IParser parser) : base(parser) { }

        public AstArrayInitialization(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstArrayInitialization--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        public override string AssociatedType
        {
            get { return AstExpression.AssociatedType; }
            set { throw new LLVMSharpException("You cannot assign AssociatedType."); }
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            //todo
        }

        public override void Walk(Walker walker)
        {
            // todo
        }
    }
}
