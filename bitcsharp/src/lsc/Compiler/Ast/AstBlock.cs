using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;
using System.Collections;

namespace LLVMSharp.Compiler.Ast
{
    public class AstBlock : AstStatement
    {
        public AstStatementCollection AstStatementCollection = new AstStatementCollection();

        public ArrayList AstLocalVarDeclarationCollection = new ArrayList();

        public AstBlock(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstBlock(IParser parser) : base(parser) { }
        public AstBlock(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstBlock--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override void Walk(Walker walker)
        {
            walker.Walk(this);
        }

        public override string AssociatedType
        {
            get { throw new System.ApplicationException("No Associted type"); }
            set { throw new System.ApplicationException("No Associted type"); }
        }
    }
}
