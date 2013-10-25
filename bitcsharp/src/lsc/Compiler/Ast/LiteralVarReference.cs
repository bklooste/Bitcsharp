using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstLiteralVarReference : AstNode, IAstExpression
    {
        public string VariableName;
        public AstConstant AstConstant;
        public IAstExpression AstExpression;

        public AstLiteralVarReference(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstLiteralVarReference(IParser parser) : base(parser) { }
        public AstLiteralVarReference(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstLiteralVarReference--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Variable Name: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                VariableName);
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
            get { 
                //return AstExpression.AssociatedType;
                return "";    
            }
            set { throw new System.ApplicationException("No Associted type"); }
        }
    }
}
