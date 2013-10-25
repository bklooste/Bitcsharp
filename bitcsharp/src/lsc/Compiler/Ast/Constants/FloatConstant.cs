using System;
using System.Diagnostics;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstRealConstant : AstConstant
    {
        /// <summary>
        /// Creates a new <see cref="AstConstant"/> of 32-bit floating point value.
        /// </summary>
        /// <param name="path">Full path of he source file containing the <see cref="AstNode"/>.</param>
        /// <param name="lineNumber">Starting line number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="columnNumber">Starting column number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="value">32-bit floating value of the constant.</param>
        public AstRealConstant(
            string path, int lineNumber, int columnNumber,
            float value)
            : base(path, lineNumber, columnNumber, value)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AstConstant"/> of 32 bit integer.
        /// </summary>
        /// <param name="parser">The parser object.</param>
        /// <param name="value">32-bit floating value of the constant.</param>
        public AstRealConstant(IParser parser, float value)
            : base(parser, value)
        {
        }

        public AstRealConstant(IParser parser) : base(parser) { }

        public AstRealConstant(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        /// <summary>
        /// Gets or sets <see cref="AstConstant"/> value as type of <see cref="float"/>.
        /// </summary>
        public double ConstantValue
        {
            [DebuggerStepThrough]
            get { return (double)base.Constant; }
            [DebuggerStepThrough]
            set { base.Constant = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstRealConstant--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Real Value: {4}");
            
            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                ConstantValue);
        }

        public override void Walk(Walker walker)
        {
            //todo walker
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override string AssociatedType
        {
            get { return "System.Single"; }
            set { throw new System.ApplicationException("No Associted type"); }
        }
    }
}
