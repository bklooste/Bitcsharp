using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstContinue : AstStatement
    {
        public AstContinue(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber)
        {
        }

        public AstContinue(IParser parser)
            : base(parser)
        {
        }

        public override string AssociatedType
        {
            get { throw new ApplicationException("No Associated Type."); }
            set { throw new ApplicationException("No Associated Type."); }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("--AstContinue--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            //todo
        }

        public override void Walk(Walker walker)
        {
            // todo walker
        }
    }
}