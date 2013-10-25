using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstAsExpression : AstNode, IAstExpression
    {
        public IAstExpression AstExpression = null;
        public string Type;
        public string FullType;
        public bool IsArray;

        public AstAsExpression(IParser parser)
            : base(parser) { }
        public AstAsExpression(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstAsExpression--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            return string.Format(sb.ToString(), System.Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        public void Walk(Walker walker)
        {
            walker.Walk(this);
        }

        public void EmitCode(CodeGenerator cgen)
        {
            //todo
        }

        public string AssociatedType
        {
            get { return FullType; }
            set { FullType = value; }
        }
    }
}