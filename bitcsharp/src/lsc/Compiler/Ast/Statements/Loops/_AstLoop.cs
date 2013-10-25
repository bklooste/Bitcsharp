using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstLoop : AstStatement
    {
        protected AstLoop(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber)
        {
        }

        protected AstLoop(IParser parser) : base(parser) { }

        public override string AssociatedType
        {
            get { throw new System.ApplicationException("AstLoop doesn't contain any Associated Type"); }
            set { throw new System.ApplicationException("AstLoop doesn't contain any Associated Type"); }
        }

        public abstract override void Walk(Walker walker);
    }
}
