using System;
using System.Diagnostics;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstIntegerConstant : AstConstant
    {
        /// <summary>
        /// Creates a new <see cref="AstConstant"/> of 32 bit integer.
        /// </summary>
        /// <param name="path">Full path of the source file containing the <see cref="AstNode"/>.</param>
        /// <param name="lineNumber">Starting line number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="columnNumber">Starting column number in the source code for the <see cref="AstNode"/>.</param>
        /// <param name="value">Integer value of the constant.</param>
        public AstIntegerConstant(
            string path, int lineNumber, int columnNumber,
            int value)
            : base(path, lineNumber, columnNumber, value)
        {
        }

        public AstIntegerConstant(IParser parser) : base(parser) { }

        public AstIntegerConstant(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        /// <summary>
        /// Gets or sets <see cref="AstConstant"/> value as type of <see cref="int"/>.
        /// </summary>
        public int ConstantValue
        {
            [DebuggerStepThrough]
            get { return (int)base.Constant; }
            [DebuggerStepThrough]
            set { base.Constant = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstIntegerConstant--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Integer Value: {4}");

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
            get { return "System.Int32"; }
            set { throw new System.ApplicationException("No Associted type"); }
        }
    }
}
