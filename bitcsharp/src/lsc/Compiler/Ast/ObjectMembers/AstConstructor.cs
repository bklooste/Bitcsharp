using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstConstructor : AstNode, IObjectMember
    {
        public string Name;
        public string Key;
        public AstParameterCollection Parameters = new AstParameterCollection();
        public AstBlock AstBlock = null;
        public AstConstructorCall AstConstructorCall = null;
        public string FullQName;
        public string KeyTypeInfo;
        public AstConstructor(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstConstructor(IParser parser) : base(parser) { }
        public AstConstructor(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        AstMemberModifierCollection _astMemberModifierCollection = new AstMemberModifierCollection();
        public AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return _astMemberModifierCollection; }
            set { _astMemberModifierCollection = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstConstructor--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Name: {4}{0}");
            sb.Append("FullQName: {5}{0}{0}");
            sb.Append("Member Modifiers: {6}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                Name, FullQName,_astMemberModifierCollection.ToString());
        }
    }

    public class AstConstructorCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstConstructor))
                throw new ArgumentException("You can add type of only AstConstructor.");
            return base.Add(value);
        }

        public int Add(AstConstructor value)
        {
            return base.Add(value);
        }
    }
}
