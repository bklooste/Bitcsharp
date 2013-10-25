using System;
using System.Collections;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstTypeConverter : AstNode, IObjectMember
    {
        public string ReturnType;
        public string FullQReturnType;
        public AstParameter AstParameter;
        public AstBlock AstBlock = null;
        public bool IsArray = false;
        public string Key;
        public string KeyTypeInfo;
        public AstTypeConverter(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstTypeConverter(IParser parser) : base(parser) { }
        public AstTypeConverter(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        AstMemberModifierCollection _astMemberModifierCollection = new AstMemberModifierCollection();
        public AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return _astMemberModifierCollection; }
            set { _astMemberModifierCollection = value; }
        }
    }

    public class AstTypeConverterCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstTypeConverter))
                throw new ArgumentException("You can add type of only AstTypeConverter.");
            return base.Add(value);
        }

        public int Add(AstTypeConverter value)
        {
            return base.Add(value);
        }
    }
}
