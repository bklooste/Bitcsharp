using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstIndexer : AstNode, IAstExpression
    {
        public IAstExpression Expression;

        public AstIndexer(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstIndexer(IParser parser) : base(parser) { }
        public AstIndexer(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstIndexer--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            //sb.Append("Attribute: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,this.Expression.ToString());
        }

        public void Walk(Walker walker)
        {
            //todo walker
        }

        public void EmitCode(CodeGenerator cgen)
        {
            //todo
        }


        public string AssociatedType
        {
            get { return ""; }
            set { throw new System.ApplicationException("No Associted type"); }
        }
    }
}

