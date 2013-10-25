using System;
using System.Collections;
using LLVMSharp.Compiler.CocoR;
using System.Text;

namespace LLVMSharp.Compiler.Ast
{
    public class AstAccessor : AstNode, IObjectMember
    {
        public string Name;
        public string ReturnType;
        public string FullyQualifiedType;
        public string Key;
        public AstGetAccessor AstGetAccessor = null;
        public AstSetAccessor AstSetAccessor = null;
        public string KeyTypeInfo;
        public AstAccessor(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstAccessor(IParser parser) : base(parser) { }
        public AstAccessor(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstAccessor--{0}{0}");
            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Accessor Name: {4}{0}");
            sb.Append("Accessor Type: {5}{0}");
            sb.Append("Accessor Fully Qualified Type: {6}{0}{0}");
            return string.Format(sb.ToString(),Environment.NewLine,base.Path, base.LineNumber,
                base.ColumnNumber,Name,ReturnType,FullyQualifiedType);
        }

        AstMemberModifierCollection _astMemberModifierCollection = new AstMemberModifierCollection();
        public AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return _astMemberModifierCollection; }
            set { _astMemberModifierCollection = value; }
        }
    }

    public class AstAccessorCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstAccessor))
                throw new ArgumentException("You can add type of only AstAccessor.");
            return base.Add(value);
        }

        public int Add(AstAccessor value)
        {
            return base.Add(value);
        }
    }
}
