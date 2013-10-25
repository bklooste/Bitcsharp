using System;
using System.Collections;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// Incidactes that an <see cref="AstNode"/> is an expression.
    /// </summary>
    public interface IAstExpression : IAssociatedType, ICodeGenerator, IWalker
    {
    }

    public class AstExpressionCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is IAstExpression))
                throw new ArgumentException("You can add type of only IAstExpression.");
            return base.Add(value);
        }

        public int Add(IAstExpression value)
        {
            return base.Add(value);
        }
    }
}