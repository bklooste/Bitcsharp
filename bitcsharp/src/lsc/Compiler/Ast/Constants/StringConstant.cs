using System;
using System.Diagnostics;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstStringConstant : AstConstant
    {
        /// <summary>
        /// Creates a new <see cref="AstConstant"/> of String type.
        /// </summary>
        /// <param name="path">Full path of the source file containing the <see cref="AstNode"/>.</param>
        /// <param name="lineNumber">Starting line number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="columnNumber">Starting column number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="value">String value of the constant.</param>
        public AstStringConstant(
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
        public AstStringConstant(IParser parser, string value)
            : base(parser, value)
        {
        }

        public AstStringConstant(IParser parser) : base(parser) { }

        public AstStringConstant(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        /// <summary>
        /// Gets or sets <see cref="AstConstant"/> value as type of <see cref="String"/>.
        /// </summary>
        public string ConstantValue
        {
            [DebuggerStepThrough]
            get { return (string)base.Constant; }
            [DebuggerStepThrough]
            set { base.Constant = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstStringConstant--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("String Value: {4}");

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
            get { return "System.String"; }
            set { throw new System.ApplicationException("No Associted type"); }
        }
    }
}
