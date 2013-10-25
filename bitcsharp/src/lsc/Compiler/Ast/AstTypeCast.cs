using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstTypeCast : AstNode, IAstExpression
    {
        public string Type;
        public IAstExpression AstExpression;
        public string VarName;
        public bool IsArray;

        public AstTypeCast(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstTypeCast(IParser parser) : base(parser) { }
        public AstTypeCast(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstTypeCast--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Type Name: {4}{0}{0}");
            sb.Append("Var Name: {5}");
            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                Type, VarName);
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
            get { return Type; }
            set { Type = value; }
        }
    }
}
