using System;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstConditional : AstStatement
    {
        public IAstExpression Condition = null;

        public AstConditional(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstConditional(IParser parser) : base(parser) { }
        public AstConditional(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string AssociatedType
        {
            get { return Condition.AssociatedType; }
            set { throw new System.ApplicationException("No Associated Type."); }
        }

        public abstract override void Walk(Walker walker);
    }
}
