using System;
using System.Collections;
using LLVMSharp.Compiler.CocoR;
using System.Text;

namespace LLVMSharp.Compiler.Ast
{
    public abstract class AstMemberModifier : AstNode
    {
        public abstract string Name { get; }

        public AstMemberModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstMemberModifier(IParser parser) : base(parser) { }

        public AstMemberModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

    }

    public enum ProtectionModifierType
    {
        Private,
        Protected,
        Public
    }


    public class AstMemberModifierCollection : ArrayList
    {
        public AstMemberModifierCollection()
        {
            ProtectionModifierType = Ast.ProtectionModifierType.Private;
        }

        public override int Add(object value)
        {
            if (!(value is AstMemberModifier))
                throw new ArgumentException("You can add type of only AstMemberModifier.");
            return base.Add(value);
        }

        public int Add(AstMemberModifier value)
        {
            return base.Add(value);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (AstMemberModifier item in this)
            {
                sb.Append(item.Name);
                sb.Append(" ");
            }
            return sb.ToString();
        }

        private bool _validated = false;
        private bool _validationResult = false;
        public bool Validate(ErrorList errorList)
        {
            if (_validated)
                return _validationResult;

            int ctrPublic = 0, ctrPrivate = 0, ctrProtected = 0, statMemb = 0, extMemb = 0;
            int ctrOverride = 0, ctrSealed = 0, ctrVirtual = 0;
            foreach (AstMemberModifier modifier in this)
            {
                if (modifier is AstPublicMemberModifier) ++ctrPublic;
                if (modifier is AstPrivateMemberModifier) ++ctrPrivate;
                if (modifier is AstProtectedMemberModifier) ++ctrProtected;
                if (modifier is AstStaticMemberModifier) ++statMemb;
                if (modifier is AstExternMemberModifier) ++extMemb;
                if (modifier is AstOverrideMemberModifier) ++ctrOverride;
                if (modifier is AstSealedMemberModifier) ++ctrSealed;
                if (modifier is AstVirtualMemberModifier) ++ctrVirtual;
            }

            if (this.Count > 0)
            {
                AstMemberModifier firstMemberModifier = (AstMemberModifier)this[0];

                if ((ctrPublic + ctrPrivate + ctrProtected) > 1)
                {
                    errorList.Add(new ErrorInfo(ErrorType.SymenticError, "More than one protection modifier",
                                                firstMemberModifier.LineNumber, firstMemberModifier.ColumnNumber,
                                                firstMemberModifier.Path));
                    _validated = true;
                    return _validationResult = false;
                }
                if (statMemb > 1)
                {
                    errorList.Add(new ErrorInfo(ErrorType.SymenticError, "Duplicate static modifier", firstMemberModifier.LineNumber, firstMemberModifier.ColumnNumber,
                                                 firstMemberModifier.Path));
                    _validated = true;

                    return _validationResult = false;
                }
                if (extMemb > 1)
                {
                    errorList.Add(new ErrorInfo(ErrorType.SymenticError, "Duplicate extern modifier", firstMemberModifier.LineNumber, firstMemberModifier.ColumnNumber,
                                                 firstMemberModifier.Path));
                    _validated = true;
                    return _validationResult = false;
                }
                if (ctrOverride > 1)
                {
                    errorList.Add(new ErrorInfo(ErrorType.SymenticError, "Duplicate override modifier", firstMemberModifier.LineNumber, firstMemberModifier.ColumnNumber,
                                                 firstMemberModifier.Path));
                    _validated = true;
                    return _validationResult = false;
                }
                if (ctrSealed > 1)
                {
                    errorList.Add(new ErrorInfo(ErrorType.SymenticError, "Duplicate sealed modifier", firstMemberModifier.LineNumber, firstMemberModifier.ColumnNumber,
                                                 firstMemberModifier.Path));
                    _validated = true;
                    return _validationResult = false;
                }
                if (ctrVirtual > 1)
                {
                    errorList.Add(new ErrorInfo(ErrorType.SymenticError, "Duplicate virtual modifier", firstMemberModifier.LineNumber, firstMemberModifier.ColumnNumber,
                                firstMemberModifier.Path));
                    _validated = true;
                    return _validationResult = false;
                }
            }
            if (extMemb == 1)
            { _IsExtern = true; }
            if (ctrOverride == 1)
            { _IsOverriden = true; }
            if (ctrVirtual == 1)
            { _IsVirtual = true; }
            if (statMemb == 1)
            { _IsStatic = true; }
            if (ctrSealed == 1)
            { _IsSealed = true; }
            if (ctrPublic == 1)
            {
                _IsPublic = true;
                ProtectionModifierType = Ast.ProtectionModifierType.Public; 
            }
            else if (ctrProtected == 1)
            {
                _IsProtected = true;
                ProtectionModifierType = Ast.ProtectionModifierType.Protected; 
            }
            else if (ctrPrivate == 1 || (ctrPublic==0 && ctrProtected==0))
            {
                _IsPrivate = true;
                ProtectionModifierType = Ast.ProtectionModifierType.Private; 
            }

            _validated = true;
            return _validationResult = true;

        }

        public void ValidateOpOverload(ErrorList errorList)
        {
            if (this.Count > 0)
            {
                AstMemberModifier firstMemberModifier = (AstMemberModifier)this[0];
                int cntPublic = 0; int cntStatic = 0;
                foreach (AstMemberModifier item in this)
                {
                    if (item is AstStaticMemberModifier) cntStatic++;
                    if (item is AstPublicMemberModifier) cntPublic++;

                }
                if (cntPublic != 1 || cntStatic != 1)
                {
                    errorList.Add(new ErrorInfo(ErrorType.SymenticError, "User-Defined operator must be declared 'public' and 'static'.", firstMemberModifier.LineNumber, firstMemberModifier.ColumnNumber, firstMemberModifier.Path));
                    
                }
            }
        }
        public ProtectionModifierType ProtectionModifierType { get; set; }

        private bool _IsExtern;
        public bool IsExtern
        {
            get { return _IsExtern; }
        }
        private bool _IsStatic;
        public bool IsStatic
        {
            get { return _IsStatic; }
        }
        private bool _IsVirtual;
        public bool IsVirtual
        {
            get { return _IsVirtual; }
        }
        private bool _IsOverriden;
        public bool IsOverriden
        {
            get { return _IsOverriden; }
        }
        private bool _IsSealed;
        public bool IsSealed
        {
            get { return _IsSealed; }
        }
        private bool _IsPrivate;
        public bool IsPrivate
        {
            get { return _IsPrivate; }
        }
        private bool _IsProtected;
        public bool IsProtected
        {
            get { return _IsProtected; }
        }
        private bool _IsPublic;
        public bool IsPublic
        {
            get { return _IsPublic; }
        }



    }
}
