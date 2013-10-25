using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp.Compiler.Ast
{
    public class AstArgument : AstNode, IAssociatedType, ICodeGenerator
    {
        public IAstExpression AstExpression;

        public AstArgument(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber)
        {
        }

        public AstArgument(IParser parser) : base(parser)
        {
        }

        public AstArgument(IParser parser, bool useLookAhead) : base(parser, useLookAhead)
        {
        }

        #region IAssociatedType Members

        public string AssociatedType
        {
            get { return AstExpression.AssociatedType; }
            set { throw new ApplicationException("No Associted type"); }
        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("--AstArgument--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        public void EmitCode(CodeGenerator cgen)
        {
            AstExpression.EmitCode(cgen);
        }
    }

    public class AstArgumentCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstArgument))
                throw new ArgumentException("You can add type of only AstArgument.");
            return base.Add(value);
        }

        public int Add(AstArgument value)
        {
            return base.Add(value);
        }
    }
}