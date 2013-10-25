using System;
using System.Collections;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    public class AstTypeModifier : AstNode
    {
        public AstTypeModifier(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstTypeModifier(IParser parser) : base(parser) { }
        public AstTypeModifier(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }
        
    }

    public class AstTypeModifierCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstTypeModifier))
                throw new ArgumentException("You can add type of only AstTypeModifier.");
            return base.Add(value);
        }

        public int Add(AstTypeModifier value)
        {
            return base.Add(value);
        }
        
        private bool _validated = false;
        private bool _validationResult = false;
        public bool Validate(ErrorList error)
        {
            if (_validated)
                return _validationResult;

            int ctrPublic = 0, ctrPrivate = 0, ctrProtected = 0, ctrSealed = 0;
            foreach (AstTypeModifier typeMod in this)
            {
                if(typeMod is AstPrivateTypeModifier)ctrPrivate++;
                if (typeMod is AstPublicTypeModifier) ctrPublic++;
                if (typeMod is AstProtectedTypeModifier) ctrProtected++;
                if (typeMod is AstSealedTypeModifier) ctrSealed++;
                    
            }
            if(this.Count>0)
            {
                AstTypeModifier firstTypeModifier = (AstTypeModifier)this[0];

                if ((ctrPublic + ctrPrivate + ctrProtected) > 1)
                {
                    error.Add(new ErrorInfo(ErrorType.SymenticError, "More than one protection modifier",
                                                firstTypeModifier.LineNumber, firstTypeModifier.ColumnNumber,
                                                firstTypeModifier.Path));
                    _validated = true;
                    return _validationResult = false;
                }
                if (ctrSealed > 1)
                {
                    error.Add(new ErrorInfo(ErrorType.SymenticError, "Duplicate sealed modifier",
                                                firstTypeModifier.LineNumber, firstTypeModifier.ColumnNumber,
                                                firstTypeModifier.Path));
                }
                if (ctrSealed == 1)
                {
                    _IsSealed = true;
                }
                if (ctrPrivate == 1)
                {
                    _IsPrivate = true;
                }
                else if(ctrProtected==1)
                {
                    _IsProtected = true;
                }
                else if (ctrPublic == 1)
                {
                    _IsPublic = true;
                }


            }

            _validated = true;
            return _validationResult = true;
        }
        private bool _IsPublic;
        public bool IsPublic
        {
            get { return _IsPublic; }
        }
        private bool _IsProtected;
        public bool IsProtected
        {
            get { return _IsProtected; }
        }
        private bool _IsPrivate;
        public bool IsPrivate
        {
            get { return _IsPrivate; }
        }
        private bool _IsSealed;
        public bool IsSealed
        {
            get { return _IsSealed; }
        }
    }    
}
