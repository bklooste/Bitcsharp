using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstAssignmentExpression : AstBinaryExpression
    {
        public IAstExpression LValue { get { return LeftOperand; } set{ LeftOperand = value;} }
        public IAstExpression RValue { get { return RightOperand; } set { RightOperand = value; } }

        public AstAssignmentExpression(
            string path, int lineNumber, int columnNumber,
            IAstExpression leftOperand, IAstExpression rightOperand)
            : base(path, lineNumber, columnNumber, leftOperand, rightOperand)
        {
        }

        public AstAssignmentExpression(
            IParser parser,
            IAstExpression leftOperand, IAstExpression rightOperand)
            : base(parser, leftOperand, rightOperand)
        {
        }

        public AstAssignmentExpression(IParser parser)
            : base(parser) { }

        public AstAssignmentExpression(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string AssociatedType
        {
            get
            {
                if (LValue.AssociatedType == RValue.AssociatedType)
                    return LValue.AssociatedType;
                else
                    return LValue.AssociatedType;
            }
            set { }
        }

        public override bool IsTypeMatch
        {
            get
            {
                if (LValue.AssociatedType == RValue.AssociatedType)
                    return true;
                else
                    return false;
            }
        }
    }
}