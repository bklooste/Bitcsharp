using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.UsingDirectives;

namespace LLVMSharp.Compiler.Walkers
{
    public class UsingDirectivesWalker
    {
        private readonly Walker _walker;
        private UsingDirective _ud;
        private bool _inNamespaceBlock;

        public UsingDirectivesWalker(LLVMSharpCompiler compiler)
        {
            _walker = new Walker(compiler);
            Init();
        }

        public void Walk()
        {
            _walker.Walk();
        }

        private void Init()
        {
            _walker.OnWalkAstSourceFile += Walker_OnWalkAstSourceFile;

            _walker.OnWalkAstUsingDirective += Walker_OnWalkAstUsingDirective;
            _walker.OnWalkAstNamespaceBlock += Walker_OnWalkAstNamespaceBlock;
            _walker.OnExitAstNamespaceBlock += Walker_OnExitAstNamespaceBlock;

            _walker.WalkAstClass += Walker_OnWalkAstClass;
            _walker.WalkAstStruct += Walker_OnWalkAstStruct;
            _walker.OnWalkAstEnum += Walker_OnWalkAstEnum;
            
            _walker.WalkAstLocalVarDecl += Walker_OnWalkAstLocalVarDecl;
        }

        #region UsingDirectives specific codes

        private void Walker_OnExitAstNamespaceBlock(AstNamespaceBlock astNamespaceBlock)
        {
            _ud.CloseScope();
            _inNamespaceBlock = false;
        }

        private void Walker_OnWalkAstNamespaceBlock(AstNamespaceBlock astNamespaceBlock)
        {
            _ud.OpenScope(astNamespaceBlock.Namespace);
            _inNamespaceBlock = true;
        }

        private void Walker_OnWalkAstUsingDirective(AstUsingDeclarative astUsingDeclarative)
        {
            _ud.Insert(astUsingDeclarative.Namespace);
        }

        private void Walker_OnWalkAstSourceFile(AstSourceFile astSourceFile)
        {
            _ud = new UsingDirective();
        }

        #endregion

        #region Assing UsingDirectives to AstTypes

        private void Walker_OnWalkAstEnum(AstEnum astEnum)
        {
            astEnum.UsingDirectives = _ud.Namespaces;
        }

        private void Walker_OnWalkAstStruct(AstStruct astStruct)
        {
            astStruct.UsingDirectives = _ud.Namespaces;
        }

        private void Walker_OnWalkAstClass(AstClass astClass)
        {
            astClass.UsingDirectives = _ud.Namespaces;
        }

        #endregion

        #region FQT

        void Walker_OnWalkAstLocalVarDecl(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            if (!FindFqtInCurrentContext(astLocalVariableDeclaration.Type, _ud.Namespaces,
                                         out astLocalVariableDeclaration.FullQualifiedType))
            {
                _walker.Compiler.Errors.Add(new ErrorInfo(ErrorType.SymenticError,
                                                          string.Format("Type {0} not found (are you missing using declaratives)", astLocalVariableDeclaration.Type),
                                                          astLocalVariableDeclaration.LineNumber, astLocalVariableDeclaration.ColumnNumber, astLocalVariableDeclaration.Path));
            }
            else
            {
                if (_walker.CurrentAstBlock != null)
                    _walker.CurrentAstBlock.AstLocalVarDeclarationCollection.Add(astLocalVariableDeclaration);
            }

        }

        public bool FindFqtInCurrentContext(string type, string[] usingDirectives, out string fqt)
        {
            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(type);

            if (!string.IsNullOrEmpty(primitiveType))
            {
                fqt = primitiveType;
                return _walker.Compiler.TypeExist(primitiveType);
            }

            return FindTypeInNamespace(type, usingDirectives, out fqt);
        }

        private bool FindTypeInNamespace(string type, string[] usingDirectives, out string fqt)
        {
            string front = string.Empty;
            if (_inNamespaceBlock)
                front = _ud.Namespaces[0];

            string toFind;

            int cutIndex;
            while (!string.IsNullOrEmpty(front))
            {
                toFind = front + "." + type;
                if (_walker.Compiler.TypeExist(toFind))
                {
                    fqt = toFind;
                    return true;
                }
                cutIndex = front.LastIndexOf("."); // start trimming
                if (cutIndex != -1)
                    front = front.Substring(0, cutIndex);
                else
                    front = string.Empty;
            }

            if (_walker.Compiler.TypeExist(type)) // mite be the type itself is fqt
            {
                fqt = type;
                return true;
            }

            // finally look at the using directives
            foreach (string ns in usingDirectives)
            {
                toFind = ns + "." + type;
                if (_walker.Compiler.TypeExist(toFind))
                {
                    fqt = toFind;
                    return true;
                }
            }

            // if not found after lookin at all the posibilities
            fqt = string.Empty;
            return false;
        }

        #endregion


    }
}