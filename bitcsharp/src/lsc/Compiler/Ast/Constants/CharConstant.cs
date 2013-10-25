using System;
using System.Diagnostics;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp.Compiler.Ast
{
    public class AstCharConstant : AstConstant
    {
        /// <summary>
        /// Creates a new <see cref="AstConstant"/> of String type.
        /// </summary>
        /// <param name="path">Full path of the source file containing the <see cref="AstNode"/>.</param>
        /// <param name="lineNumber">Starting line number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="columnNumber">Starting column number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="value">String value of the constant.</param>
        public AstCharConstant(
            string path, int lineNumber, int columnNumber,
            string value)
            : base(path, lineNumber, columnNumber, value)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AstConstant"/> of String type..
        /// </summary>
        /// <param name="parser">The parser object.</param>
        /// <param name="value">String value of the constant.</param>
        public AstCharConstant(IParser parser, string value)
            : base(parser, value)
        {
        }

        public AstCharConstant(IParser parser) : base(parser) { }

        public AstCharConstant(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        /// <summary>
        /// Gets or sets <see cref="AstConstant"/> value as type of <see cref="String"/>.
        /// </summary>
        public char ConstantValue
        {
            [DebuggerStepThrough]
            get { return (char)base.Constant; }
            [DebuggerStepThrough]
            set { base.Constant = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstCharConstant--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Char Value: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                ConstantValue);
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            //todo
        }

        public override string AssociatedType
        {
            get { return "System.Char"; }
            set { throw new System.ApplicationException("No Associted type"); }
        }
    }
}
