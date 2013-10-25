using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstMemberReference : AstNode, IAstExpression
    {
        public string Type;
        public string FullQualifiedType;
        public AstConstant AstConstant;

        public AstMemberReference(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstMemberReference(IParser parser) : base(parser) { }
        public AstMemberReference(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstMemberReference--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Attribute: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                Type);
        }

        public void Walk(Walker walker)
        {
            //todo walker
        }

        public void EmitCode(CodeGenerator cgen)
        {
            //todo
        }


        public string AssociatedType
        {
            get { return FullQualifiedType; }
            set { FullQualifiedType = value; }
        }
    }
}

