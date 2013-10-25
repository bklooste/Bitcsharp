﻿using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstBaseConstructorCall : AstConstructorCall
    {

        public AstBaseConstructorCall(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstBaseConstructorCall(IParser parser) : base(parser) { }
        public AstBaseConstructorCall(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstBaseConstructorCall--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");
            sb.Append("base");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber);
        }


        public override string AssociatedType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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
