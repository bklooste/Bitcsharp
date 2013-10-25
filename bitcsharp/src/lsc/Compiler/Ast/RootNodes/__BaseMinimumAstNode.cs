using System.Diagnostics;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// Base AST Node class.
    /// </summary>
    [DebuggerStepThrough]
    public abstract class BaseMinimumAstNode
    {
        /// <summary>
        /// Create a new BaseMinimum AST Node.
        /// </summary>
        protected BaseMinimumAstNode(){}

        /// <summary>
        /// Creates a new BaseMinimum AST Node.
        /// </summary>
        /// <param name="parser">The parser object.</param>
        protected BaseMinimumAstNode(IParser parser) { }

        /// <summary>
        /// Returns a string representation of <see cref="BaseMinimumAstNode"/>.
        /// </summary>
        /// <returns>A string representation of SimpleAstNode.</returns>
        public override string ToString()
        {
            return string.Format("--Base Minimum Node--");
        }
    }
}
