using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp.Compiler.Ast
{
    public class AstThisConstructorCall : AstConstructorCall
    {

        public AstThisConstructorCall(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstThisConstructorCall(IParser parser) : base(parser) { }
        public AstThisConstructorCall(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstThisConstructorCall--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");
            sb.Append("this");

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
    }

//    class AstThisConstructorCallImpl : AstThisConstructorCall
//    {
//    }
}
