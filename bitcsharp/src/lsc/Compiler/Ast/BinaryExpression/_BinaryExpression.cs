using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstBinaryExpression
        : AstNode, IAstExpression, IAstHasExpressions
    {

        public IAstExpression LeftOperand;
        public IAstExpression RightOperand;


        protected AstBinaryExpression(
            string path, int lineNumber, int columnNumber,
            IAstExpression leftOperand, IAstExpression rightOperand)
            : base(path, lineNumber, columnNumber)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        protected AstBinaryExpression(
            IParser parser,
            IAstExpression leftOperand, IAstExpression rightOperand)
            : this(parser.Scanner.FileName, parser.Token.LineNumber, parser.Token.ColumnNumber,
             rightOperand, leftOperand)
        {
        }

        protected AstBinaryExpression(IParser parser) : base(parser) { }
        protected AstBinaryExpression(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        #region IAstHasExpressions Members

        public IAstExpression[] Expressions
        {
            get { return new IAstExpression[] { LeftOperand, RightOperand }; }
        }

        #endregion

        public virtual string AssociatedType
        {
            get
            {
                if (LeftOperand.AssociatedType == RightOperand.AssociatedType)
                    return LeftOperand.AssociatedType;
                else
                    return "Unknown"; //RightOperand.AssociatedType; //take right for nested binary expression
            }
            set { }
        }
        public virtual bool IsTypeMatch
        {
            get
            {
                if (LeftOperand.AssociatedType == RightOperand.AssociatedType)
                    return true;
                else
                    return false;
            }
        }

        public abstract void EmitCode(CodeGenerator cgen);
        public abstract void Walk(Walker walker);
    }
}
