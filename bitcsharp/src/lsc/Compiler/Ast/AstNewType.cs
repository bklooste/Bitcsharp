using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstNewType : AstNode, IAstExpression
    {
        public string Type;
        public string FullQualifiedType;
        public AstArgumentCollection AstArgumentCollection = new AstArgumentCollection();
        public AstArrayInitialization AstArrayInitialization = null;
        public AstIndexer AstIndexer = null;
        public bool IsArray;

        public AstNewType(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstNewType(IParser parser) : base(parser) { }

        public AstNewType(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstNewType--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");
            sb.Append("Type: {4}{0}");
            sb.Append("FullQualifiedType: {5}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,Type,FullQualifiedType);
        }

        public void Walk(Walker walker)
        {
            //todo walker
        }

        public void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public string AssociatedType
        {
            get { return FullQualifiedType; }
            set
            {
                FullQualifiedType = value;
            }
        }

    }
}
