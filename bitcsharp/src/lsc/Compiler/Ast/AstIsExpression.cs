using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstIsExpression : AstNode, IAstExpression
    {
        public IAstExpression AstExpression = null;
        public string Type;
        public bool IsArray;

        public AstIsExpression(IParser parser)
            : base(parser) { }
        public AstIsExpression(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstIsExpression--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            return string.Format(sb.ToString(), System.Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
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
            get { return "System.Boolean"; }
            set { throw new LLVMSharpException("You cannot change the AssociatedType"); }
        }
    }
}