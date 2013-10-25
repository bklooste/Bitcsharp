using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstLocalVariableDeclaration : AstStatement
    {
        public string Type;
        public string FullQualifiedType;
        public string Name;
        public IAstExpression Initialization;
        public bool IsConstant = false;
        public bool IsArray = false;

        public AstLocalVariableDeclaration(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstLocalVariableDeclaration(IParser parser) : base(parser) { }
        public AstLocalVariableDeclaration(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstLocalVariableDeclaration--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Variable Name: {4}{0}");
            sb.Append("Variable Type: {5}{0}");
            sb.Append("Full Qualified Variable Type: {6}{0}{0}");

            sb.Append("IsConstant: {7}{0}");


            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                this.Name, this.Type, this.FullQualifiedType, this.IsConstant);
        }

        public override string AssociatedType
        {
            get { return FullQualifiedType; }
            set { FullQualifiedType = value; }
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override void Walk(Walker walker)
        {
            walker.Walk(this);
        }
    }
}
