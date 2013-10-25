using System.Diagnostics;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// Represents any type of constants.
    /// </summary>
    public abstract class AstConstant :
        AstNode, IAstExpression
    {
        public object Constant;

        /// <summary>
        /// Creates a new Constant <see cref="AstNode"/>.
        /// </summary>
        /// <param name="path">Full path of the source file containing the <see cref="AstNode"/>.</param>
        /// <param name="lineNumber">Starting line number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="columnNumber">Starting column number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="constant">Value of constant.</param>
        public AstConstant(
            string path, int lineNumber, int columnNumber,
            object constant)
            : base(path, lineNumber, columnNumber)
        {
            Constant = constant;
        }

        /// <summary>
        /// Creates a new Constant <see cref="AstNode"/>.
        /// </summary>
        /// <param name="parser">The parser object.</param>
        /// <param name="constant">The value of the constant.</param>
        public AstConstant(IParser parser, object constant) :
            base(parser)
        {
            Constant = constant;
        }

        public AstConstant(IParser parser) : base(parser) { }

        public AstConstant(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        /// <summary>
        /// Returns a string representation of <see cref="AstConstant"/>.
        /// </summary>
        /// <returns>A string representation of AstConstant.</returns>
        [DebuggerStepThrough]
        public override string ToString()
        {
            return Constant == null ? "--null--" : Constant.ToString();
        }

        public abstract void Walk(Walker walker);

        public abstract void EmitCode(CodeGenerator cgen);

        public abstract string AssociatedType { get; set; }
    }
}