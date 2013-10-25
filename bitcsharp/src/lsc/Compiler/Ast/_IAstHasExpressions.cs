
namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// Incidicates that <see cref="AstNode"/> contains multiple Expressions
    /// </summary>
    public interface IAstHasExpressions
    {
        /// <summary>
        /// Gets <see cref="System.Array"/> of Expressions contained
        /// in the <see cref="AstNode"/>.
        /// </summary>
        IAstExpression[] Expressions { get; }
    }
}
