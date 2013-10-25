using System;
using System.Collections;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstType : AstNode, IAstNamespaceMember, IObjectMember, IAssociatedType, IEntryPoint
    {
        public string Name;
        public AstTypeModifierCollection AstTypeModifierCollection = new AstTypeModifierCollection();
        public string FullQualifiedName;


        public AstType(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstType(IParser parser) : base(parser) { }

        public AstType(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public abstract AstMemberModifierCollection AstMemberModifierCollection { get; set; }

        public string AssociatedType
        {
            get { return FullQualifiedName; }
            set { FullQualifiedName = value; }
        }

        public abstract void CheckEntryPoint(LLVMSharpCompiler compiler);
        public abstract void Walk(Walker walker);

        public string[] UsingDirectives;
    }


    public class AstTypeCollection : ArrayList, IEntryPoint
    {
        public override int Add(object value)
        {
            if (!(value is AstType))
                throw new ArgumentException("You can add type of only AstType.");
            return base.Add(value);
        }

        public int Add(AstType value)
        {
            return base.Add(value);
        }

        public void CheckEntryPoint(LLVMSharpCompiler compiler)
        {
            foreach (AstType astType in this)
                astType.CheckEntryPoint(compiler);
        }
    }
}
