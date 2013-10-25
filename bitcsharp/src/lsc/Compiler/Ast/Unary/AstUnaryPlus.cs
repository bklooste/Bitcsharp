﻿using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstUnaryPlus : AstNode, IAstExpression
    {
        public IAstExpression AstExpression;

        public AstUnaryPlus(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstUnaryPlus(IParser parser) : base(parser) { }
        public AstUnaryPlus(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstUnaryPlus--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
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
            get { return AstExpression.AssociatedType; }
            set { throw new LLVMSharpException("You cannot assign AssociatedType"); }
        }
    }
}
