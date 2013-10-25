using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstNamespaceBlock : AstNode, IAstNamespaceMember, IEntryPoint
    {
        public string Namespace;
        public AstNamespaceBlockCollection AstNamespaceBlockCollection = new AstNamespaceBlockCollection();
        public AstTypeCollection AstTypeCollection = new AstTypeCollection();

        public AstNamespaceBlock(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstNamespaceBlock(IParser parser) : base(parser) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstNamespaceBlock--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Namespace: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                Namespace);
        }


        public void CheckEntryPoint(LLVMSharpCompiler compiler)
        {
            AstNamespaceBlockCollection.CheckEntryPoint(compiler);
            AstTypeCollection.CheckEntryPoint(compiler);
        }
    }

    public class AstNamespaceBlockCollection : ArrayList, IEntryPoint
    {
        public override int Add(object value)
        {
            if (!(value is AstNamespaceBlock))
                throw new ArgumentException("You can add type of only AstNamespaceBlock.");
            return base.Add(value);
        }

        public int Add(AstNamespaceBlock value)
        {
            return base.Add(value);
        }

        public void CheckEntryPoint(LLVMSharpCompiler compiler)
        {
            foreach (AstNamespaceBlock astNamespaceBlock in this)
                astNamespaceBlock.CheckEntryPoint(compiler);
        }
    }
}
