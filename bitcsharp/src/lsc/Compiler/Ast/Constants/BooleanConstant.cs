using LLVMSharp.Compiler.CocoR;
using System.Diagnostics;
using System.Text;
using System;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstBooleanConstant : AstConstant
    {
        /// <summary>
        /// Creates a new <see cref="AstConstant"/> of Boolean type.
        /// </summary>
        /// <param name="path">Full path of the source file containing the <see cref="AstNode"/>.</param>
        /// <param name="lineNumber">Starting line number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="columnNumber">Starting column number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="value">Boolean value of the constant.</param>
        public AstBooleanConstant(
            string path, int lineNumber, int columnNumber,
            bool value)
            : base(path, lineNumber, columnNumber, value)
        {
        }

        public AstBooleanConstant(IParser parser) : base(parser) { }

        public AstBooleanConstant(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        /// <summary>
        /// Gets or sets <see cref="AstConstant"/> value as type of <see cref="bool"/>.
        /// </summary>
        public bool ConstantValue
        {
            [DebuggerStepThrough]
            get { return (bool)base.Constant; }
            [DebuggerStepThrough]
            set { base.Constant = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstBooleanConstant--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Boolean Value: {4}");

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
            get { return "System.Boolean"; }
            set { throw new System.ApplicationException("No Associted type"); }
        }
    }
}
