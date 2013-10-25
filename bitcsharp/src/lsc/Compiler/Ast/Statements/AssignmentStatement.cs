using System;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstAssignmentStatement : AstStatement
    {
        public AstAssignmentExpression AstAssignmentExpression = null;

        public AstAssignmentStatement(IParser parser)
            : base(parser) { }

        public AstAssignmentStatement(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string AssociatedType
        {
            get
            {
                return AstAssignmentExpression.AssociatedType;
            }
            set
            {
            }
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override void Walk(Walker walker)
        {
            // todo
        }

        public bool IsTypeMatch
        {
            get
            {
                return AstAssignmentExpression.IsTypeMatch;
            }
        }
    }
}