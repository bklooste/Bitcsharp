using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstAdditionExpression : AstBinaryExpression
    {
        public AstAdditionExpression(
            string path, int lineNumber, int columnNumber,
            IAstExpression leftOperand, IAstExpression rightOperand)
            : base(path, lineNumber, columnNumber, leftOperand, rightOperand)
        {
        }

        public AstAdditionExpression(
            IParser parser,
            IAstExpression leftOperand, IAstExpression rightOperand)
            : base(parser, leftOperand, rightOperand)
        {
        }

        public AstAdditionExpression(IParser parser)
            : base(parser) { }
        public AstAdditionExpression(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstAdditionExpression--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Operator Type: Addition +");

            return string.Format(sb.ToString(), System.Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        //public override string AssociatedType
        //{
        //    get
        //    {
        //        if (base.LeftOperand.AssociatedType == base.RightOperand.AssociatedType)
        //            return base.LeftOperand.AssociatedType;
        //        else
        //            return null;
        //    }
        //    set { throw new System.NotImplementedException(); }
        //}

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override void Walk(Walker walker)
        {
            //todo walker
        }
    }
}