using System;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstForLoop : AstLoop
    {
        public AstStatementCollection Initializers = new AstStatementCollection();
        public IAstExpression Condition = null;
        public AstExpressionCollection IncrementExpressions = new AstExpressionCollection();
        public AstStatement Body = null;

        public AstForLoop(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstForLoop(IParser parser)
            : base(parser) { }

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override void Walk(Walker walker)
        {
            walker.Walk(this);
        }
    }
}
