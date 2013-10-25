using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstConstructorCall : AstStatement
    {
        public AstArgumentCollection AstArgumentCollection = new AstArgumentCollection();

        public AstConstructorCall(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstConstructorCall(IParser parser) : base(parser) { }
        public AstConstructorCall(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override void Walk(Walker walker)
        {
            // todo walker
        }
    }
}
