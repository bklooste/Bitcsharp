using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstSubtractAssignmentExpression : AstAssignmentExpression
    {
        public AstSubtractAssignmentExpression(
            string path, int lineNumber, int columnNumber,
            IAstExpression leftOperand, IAstExpression rightOperand)
            : base(path, lineNumber, columnNumber, leftOperand, rightOperand)
        {
        }

        public AstSubtractAssignmentExpression(
            IParser parser,
            IAstExpression leftOperand, IAstExpression rightOperand)
            : base(parser, leftOperand, rightOperand)
        {
        }

        public AstSubtractAssignmentExpression(IParser parser)
            : base(parser) { }

        public AstSubtractAssignmentExpression(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstSubtractAssignmentExpression--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Operator Type: -=");

            return string.Format(sb.ToString(), System.Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        //public override string AssociatedType
        //{
        //    get
        //    {
        //        if (LValue.AssociatedType == RValue.AssociatedType)
        //            return LValue.AssociatedType;
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