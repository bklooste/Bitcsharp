using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstVariableReference : AstStatement
    {
        public string VariableName;
        public bool IsMemberReference = false;
        public MemberRefCollection MemberRefCollection = new MemberRefCollection();
        public bool IsArray = false;
        public bool IsConst = false;
        public object OwnerObj = null;

        public IAstExpression Indexer;
        public AstVariableReference(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstVariableReference(IParser parser) : base(parser) { }
        public AstVariableReference(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstVariableReference--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Variable Name: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                VariableName);
        }

        string _associatedType;
        public override string AssociatedType
        {
            get
            {
                if (_associatedType != null)
                    return _associatedType;
                else
                {
                    return "Unknown";
                }
            }
            set { _associatedType = value; }

        }

        public override void EmitCode(CodeGenerator cgen)
        {
            cgen.EmitCode(this);
        }

        public override void Walk(Walker walker)
        {
            // todo walker
        }

        [Obsolete]
        public bool IsLocalVar()
        {
            if (OwnerObj is AstLocalVariableDeclaration)
                return true;
            else
            {
                return false;
            }
        }
    }

    public class MemberRefCollection : ArrayList
    {
        public override int Add(object value)
        {
            //to do: check for specific Object
            //if (!(value is string))
            //    throw new ArgumentException("You can add type of only string.");
            return base.Add(value);
        }

        public int Add(string value)
        {
            return base.Add(value);
        }
    }
}
