using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstBreak : AstStatement
    {
        public AstBreak(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstBreak(IParser parser)
            : base(parser) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstBreak--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        public override string AssociatedType
        {
            get { throw new ArgumentException("break statements doesn't contain AssociatedTypes."); }
            set { throw new System.ApplicationException("No Associted type"); }
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
