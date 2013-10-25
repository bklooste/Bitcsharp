using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstField : AstNode, IObjectMember
    {
        public string Name;
        public string Type;
        public string FullQualifiedType;
        public bool IsConstant = false;
        public IAstExpression Initialization = null;
        public bool IsArray = false;
        public int Index; // for cgen faster access

        public AstField(
             string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstField(IParser parser) : base(parser) { }
        public AstField(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstField--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Name: {4}{0}");
            sb.Append("Type: {5}{0}");
            sb.Append("Full Qualified Type: {6}{0}");

            sb.Append("Member Modifiers: {7}{0}");

            sb.Append("Is Constant: {8}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                Name, this.Type, FullQualifiedType, _astMemberModifierCollection.ToString(), IsConstant.ToString());
        }

        AstMemberModifierCollection _astMemberModifierCollection = new AstMemberModifierCollection();
        public AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return _astMemberModifierCollection; }
            set { _astMemberModifierCollection = value; }
        }

    }

    public class AstFieldCollection : ArrayList,
        IObjectMember /* IObjectMember is implemented just to for being in StructMember */
    {
        public override int Add(object value)
        {
            if (!(value is AstField))
                throw new ArgumentException("You can add type of only AstField.");
            return base.Add(value);
        }

        public int Add(AstField value)
        {
            return base.Add(value);
        }

        public AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return null; }
            set
            {
                /* Assignning value to AstMemberModifierCollection in AstFieldCollection has no effect.
                 * Please assign it to AstField. 
                 */
            }
        }
    }
}
