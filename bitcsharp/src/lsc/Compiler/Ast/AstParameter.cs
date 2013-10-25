using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp.Compiler.Ast
{
    public class AstParameter : AstNode, IAssociatedType, ICodeGenerator
    {
        public AstParameterType ParameterType = null;
        public string Name;
        public string Type;
        public bool IsArray;
        public string FullQualifiedType;

        public AstParameter(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstParameter(IParser parser) : base(parser) { }
        public AstParameter(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstParameter--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Parameter Type: {4}{0}");
            sb.Append("Parameter Name: {5}{0}");
            sb.Append("Pass By: {6}{0}");

            sb.Append("Parameter Full Qualified Type: {7}");

            string passBy = "value - default";
            if (ParameterType is AstPassByRefParameterType)
                passBy = "reference - ref";

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                this.Type, this.Name, passBy,FullQualifiedType);
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

    public class AstParameterCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstParameter))
                throw new ArgumentException("You can add type of only AstParameter.");
            return base.Add(value);
        }

        public int Add(AstParameter value)
        {
            return base.Add(value);
        }
    }
}
