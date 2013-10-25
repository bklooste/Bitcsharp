using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstMethodCall : AstStatement
    {

        public string Name;
        public AstArgumentCollection ArgumentCollection = new AstArgumentCollection();
        public bool IsMemberMethod = false;
        public MemberRefCollection MemberRefCollection = new MemberRefCollection();
        public Object OwnerObj = null;
        public AstMethod MemberMethod = null;

        public AstMethodCall(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstMethodCall(IParser parser) : base(parser) { }
        public AstMethodCall(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstMethodCall--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Name: {4}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                Name);
        }


        private string _AssociatedType;
        public override string AssociatedType
        {
            get { return _AssociatedType; }
            set { _AssociatedType = value; }
        }

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override void Walk(Walker walker)
        {
            // todo walker
        }
    }
}
