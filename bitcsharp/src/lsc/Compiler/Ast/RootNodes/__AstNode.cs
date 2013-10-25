using System.Diagnostics;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// Base AST Node class.
    /// </summary>
    [DebuggerStepThrough]
    public abstract class AstNode : BaseMinimumAstNode
    {
        /// <summary>
        /// Full path of the source code containing the <see cref="AstNode"/>.
        /// </summary>
        public string Path;

        /// <summary>
        /// Starting line number in the source code for the <see cref="AstNode"/>.
        /// </summary>
        public int LineNumber;

        /// <summary>
        /// Starting Column number in the source code for the <see cref="AstNode"/>.
        /// </summary>
        public int ColumnNumber;

        /// <summary>
        /// Create a new AST Node.
        /// </summary>
        /// <param name="path">Full path of the source code.</param>
        /// <param name="lineNumber">Starting line number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="columnNumber">Starting column number in the source code for the <see cref="AstNode"/>.</param>
        public AstNode(string path, int lineNumber, int columnNumber)
        {
            Path = path;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        /// <summary>
        /// Creates a new AST Node.
        /// </summary>
        /// <param name="parser">The parser object.</param>
        /// <remarks>
        /// This is to make our work easier, so we don't have to manually
        /// provide the path, lineNumber and columnNumber everytime.
        /// </remarks>
        public AstNode(IParser parser) :
            this(parser.Scanner.FileName, parser.LookAhead.LineNumber, parser.LookAhead.ColumnNumber)
        {
        }

        /// <summary>
        /// Creates a new AST Node.
        /// </summary>
        /// <param name="parser">The parser object.</param>
        /// <param name="useLookAhead">Whether to use Lookahead or current token to determine the line number and column numbers</param>
        /// <remarks>
        /// This is to make our work easier, so we don't have to manually
        /// provide the path, lineNumber and columnNumber everytime.
        /// </remarks>
        public AstNode(IParser parser, bool useLookAhead)
        {
            Path = parser.Scanner.FileName;

            if (useLookAhead)
            {
                LineNumber = parser.LookAhead.LineNumber;
                ColumnNumber = parser.LookAhead.ColumnNumber;
            }
            else
            {
                LineNumber = parser.Token.LineNumber;
                ColumnNumber = parser.Token.ColumnNumber;
            }
        }

        /// <summary>
        /// Returns a string representation of <see cref="AstNode"/>.
        /// </summary>
        /// <returns>A string representation of AstNode.</returns>
        public override string ToString()
        {
            return string.Format("{0} L:{1} C:{2}", Path, LineNumber, ColumnNumber);
        }
    }
}
