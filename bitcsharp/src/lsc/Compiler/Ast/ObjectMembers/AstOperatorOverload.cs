using System;
using System.Collections;
using LLVMSharp.Compiler.CocoR;
using System.Text;

namespace LLVMSharp.Compiler.Ast
{
    public class AstOperatorOverload : AstNode, IObjectMember
    {
        public OverloadableOperand OverloadableOperand;
        public AstParameter AstParameter1 = null;
        public AstParameter AstParameter2 = null;
        public AstBlock AstBlock = null;
        public string ReturnType;
        public string FullQReturnType;
        public bool IsArray;
        public string Key;
        public string KeyTypeInfo;
        public AstOperatorOverload(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstOperatorOverload(IParser parser) : base(parser) { }
        public AstOperatorOverload(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        AstMemberModifierCollection _astMemberModifierCollection = new AstMemberModifierCollection();
        public AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return _astMemberModifierCollection; }
            set { _astMemberModifierCollection = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstOperatorOverload--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Overload Operand: {4}{0}{0}");
            sb.Append("Return Type: {5}{0}");
            sb.Append("FullQReturnType: {6}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                ToString(this.OverloadableOperand), this.ReturnType, this.FullQReturnType);
        }

        public static string ToString(OverloadableOperand overloadableOperand)
        {
            switch (overloadableOperand)
            {
                case OverloadableOperand.Plus: return "+";
                case OverloadableOperand.Minus: return "-";
                case OverloadableOperand.Not: return "!";
                case OverloadableOperand.Increment: return "++";
                case OverloadableOperand.Decrement: return "--";
                case OverloadableOperand.True: return "true";
                case OverloadableOperand.False: return "false";
                case OverloadableOperand.Multiplication: return "*";
                case OverloadableOperand.Division: return "/";
                case OverloadableOperand.Equality: return "==";
                case OverloadableOperand.NotEqual: return "!=";
                case OverloadableOperand.GreaterThan: return ">";
                case OverloadableOperand.LessThan: return "<";
                case OverloadableOperand.GreaterThanEqual: return ">=";
                case OverloadableOperand.LessThanEqual: return "<=";
                default:
                    throw new Exception("Invalid OverloadableOperand");
            }
        }
    }

    public class AstOperatorOverloadCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstOperatorOverload))
                throw new ArgumentException("You can add type of only AstOperatorOverload.");
            return base.Add(value);
        }

        public int Add(AstOperatorOverload value)
        {
            return base.Add(value);
        }
    }

    public enum OverloadableOperand
    {
        Plus,
        Minus,
        Not,
        Increment,
        Decrement,
        True,
        False,
        Multiplication,
        Division,
        Equality,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanEqual,
        LessThanEqual
    }

}
