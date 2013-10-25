using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.Semantic;

namespace LLVMSharp.Compiler.Ast
{
    public class AstMethod : AstNode, IObjectMember
    {

        public string Name;
        public string ReturnType;
        public string FullQReturnType;
        public AstParameterCollection Parameters = new AstParameterCollection();
        public AstLocalVariableCollection LocalVariables = new AstLocalVariableCollection();
        public AstBlock AstBlock = null;
        public string key;
        public string KeyTypeInfo;
        public SymbolTableNode MethodSymbolTable;

        public AstMethod(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstMethod(IParser parser) : base(parser) { }
        public AstMethod(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        AstMemberModifierCollection _astMemberModifierCollection = new AstMemberModifierCollection();
        public AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return _astMemberModifierCollection; }
            set { _astMemberModifierCollection = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstMethod--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Name: {4}{0}");
            sb.Append("Member Modifiers: {5}{0}");
            sb.Append("Return Type: {6}{0}");
            sb.Append("Full Qualified Return Type: {7}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                Name, _astMemberModifierCollection.ToString(), ReturnType, FullQReturnType);
        }
    }

    public class AstMethodCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstMethod))
                throw new ArgumentException("You can add type of only AstMethod.");
            return base.Add(value);
        }

        public int Add(AstMethod value)
        {
            return base.Add(value);
        }
    }

    public class AstLocalVariableCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstLocalVariableDeclaration))
                throw new ArgumentException("You can add type of only AstLocalVariableDeclaration.");
            return base.Add(value);
        }

        public int Add(AstLocalVariableDeclaration value)
        {
            return base.Add(value);
        }
    }

}
