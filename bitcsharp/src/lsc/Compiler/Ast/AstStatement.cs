using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using System.Collections;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstStatement : AstNode, IAstExpression
    {
        public AstStatement(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstStatement(IParser parser) : base(parser) { }
        public AstStatement(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public abstract string AssociatedType { get; set; }

        public abstract void EmitCode(CodeGenerator cgen);

        public abstract void Walk(Walker walker);
    }

    public class AstStatementCollection : ArrayList, ICodeGenerator
    {
        public override int Add(object value)
        {
            if (!(value is AstStatement))
                throw new ArgumentException("You can add type of only AstStatement.");
            return base.Add(value);
        }

        public int Add(AstStatement value)
        {
            return base.Add(value);
        }

        public void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }
    }
}
