using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.Walkers
{
    public interface IRootWalker
    {
        void Walk(Walker walker);
    }

    public interface IWalker
    {
        void Walk(Walker walker);
    }

    #region Walk Delegates

    public delegate void OnWalk();
    public delegate void OnWalkAstProgram(AstProgram astProgram);
    public delegate void OnWalkAstSourceFile(AstSourceFile astSourceFile);
    public delegate void OnWalkAstUsingDirective(AstUsingDeclarative astUsingDeclarative);
    public delegate void OnWalkAstNamespaceBlock(AstNamespaceBlock astNamespaceBlock);

    #region AstTypes
    public delegate void OnWalkAstClass(AstClass astClass);
    public delegate void OnWalkAstStruct(AstStruct astStruct);
    public delegate void OnWalkAstEnum(AstEnum astEnum);
    #endregion

    public delegate void OnWalkAstConstructor(AstConstructor astConstructor);

    public delegate void OnWalkAstMethod(AstMethod astMethod);
    public delegate void OnWalkAstBlock(AstBlock astBlock);

    public delegate void OnWalkAstParameterCollection(AstParameterCollection astParameterCollection);
    public delegate void OnWalkAstParameter(AstParameter astParameter);
    public delegate void OnWalkAstArgumentCollection(AstArgumentCollection astArgumentCollection);
    public delegate void OnWalkAstArgument(AstArgument astArgument);

    public delegate void OnWalkAstOperatorOverload(AstOperatorOverload astOperatorOverload);
    public delegate void OnWalkAstTypeConverter(AstTypeConverter astTypeConverter);


    #region AstStatement
    public delegate void OnWalkAstLocalVarDecl(AstLocalVariableDeclaration astLocalVariableDeclaration);
    public delegate void OnWalkAstAssignmentStatement(AstAssignmentStatement astAssignmentStatement);

    public delegate void OnWalkAstAccessor(AstAccessor astAccessor);

    #region Loops

    public delegate void OnWalkAstWhileLoop(AstWhileLoop astWhileLoop);
    public delegate void OnWalkAstDoLoop(AstDoLoop astDoLoop);
    public delegate void OnWalkAstForLoop(AstForLoop astForLoop);

    #endregion

    public delegate void OnWalkAstIfCondition(AstIfCondition astIfCondition);

    #endregion

    #region IAstExpressions

    public delegate void OnWalkAstIntegerConstant(AstIntegerConstant astIntegerConstant);
    public delegate void OnWalkAstMethodCall(AstMethodCall astMethodCall);
    public delegate void OnWalkAstSimpleAssignmentExpression(AstSimpleAssignmentExpression astSimpleAssignmentExpression);
    public delegate void OnWalkAstVarRef(AstVariableReference astVariableReference);

    public delegate void OnWalkAstAdditionExpression(AstAdditionExpression astAdditionExpression);
    public delegate void OnWalkAstAddAssignmentExpression(AstAddAssignmentExpression addAssignmentExpression);

    #endregion


    #endregion

    public class Walker
    {
        private readonly LLVMSharpCompiler _compiler;
        public LLVMSharpCompiler Compiler { get { return _compiler; } }

        public AstMethod CurrentAstMethod;
        public AstConstructor CurrentAstConstructor;
        public AstAccessor CurrentAstAccessor;
        public int? ParameterIndex;
        public int? ArgumentIndex;
        public AstMethodCall CurrentAstMethodCall;
        public AstOperatorOverload CurrentAstOperatorOverload;
        public AstTypeConverter CurrentAstTypeConverter;
        public AstEnum CurrentAstEnum;
        public AstStruct CurrentAstStruct;
        public AstBlock CurrentAstBlock;

        public Walker(LLVMSharpCompiler compiler)
        {
            _compiler = compiler;
        }

        public virtual void Walk()
        {
            CurrentAstMethod = null;
            CurrentAstConstructor = null;
            ParameterIndex = null;
            ArgumentIndex = null;
            CurrentAstMethodCall = null;

            OnWalkStart();
            Walk(_compiler.AstProgram);
            OnWalkEnd();
        }

        #region Walk methods

        protected virtual void Walk(AstProgram astProgram)
        {
            OnWalkAstProgram(_compiler.AstProgram);

            foreach (AstSourceFile astSourceFile in astProgram.SourceFiles)
            {
                OnWalkAstSourceFile(astSourceFile);

                Walk(astSourceFile.AstUsingDeclarativeCollection);
                Walk(astSourceFile.AstTypeCollection);
                Walk(astSourceFile.AstNamespaceBlockCollection);

                OnExitAstSourceFile(astSourceFile);
            }
        }

        protected virtual void Walk(AstNamespaceBlockCollection astNamespaceBlockCollection)
        {
            foreach (AstNamespaceBlock astNamespaceBlock in astNamespaceBlockCollection)
            {
                OnWalkAstNamespaceBlock(astNamespaceBlock);

                Walk(astNamespaceBlock.AstTypeCollection);
                Walk(astNamespaceBlock.AstNamespaceBlockCollection);

                OnExitAstNamespaceBlock(astNamespaceBlock);
            }
        }

        protected virtual void Walk(AstUsingDeclarativeCollection astUsingDeclarativeCollection)
        {
            foreach (AstUsingDeclarative astUsingDeclarative in astUsingDeclarativeCollection)
                OnWalkAstUsingDirective(astUsingDeclarative);
        }

        #region AstType

        protected virtual void Walk(AstTypeCollection astTypeCollection)
        {
            foreach (AstType astType in astTypeCollection)
                astType.Walk(this);
        }

        public virtual void Walk(AstClass astClass)
        {
            OnWalkAstClass(astClass);

            Walk(astClass.AstConstructorCollection);
            Walk(astClass.AstMethodCollection);
            Walk(astClass.AstAccessorCollection);
            Walk(astClass.AstOperatorOverloadCollection);
            Walk(astClass.AstTypeConverterCollection);

            OnExitAstClass(astClass);
        }


        public void Walk(AstStruct astStruct)
        {
            CurrentAstStruct = astStruct;

            OnWalkAstStruct(astStruct);
            Walk(astStruct.AstConstructorCollection);
            Walk(astStruct.AstMethodCollection);
            Walk(astStruct.AstAccessorCollection);
            Walk(astStruct.AstOperatorOverloadCollection);
            Walk(astStruct.AstTypeConverterCollection);
            OnWalkAstStruct(astStruct);

            CurrentAstStruct = null;
        }

        private void Walk(AstTypeConverterCollection astTypeConverterCollection)
        {
            foreach (AstTypeConverter astTypeConverter in astTypeConverterCollection)
                Walk(astTypeConverter);
        }

        private void Walk(AstTypeConverter astTypeConverter)
        {
            CurrentAstTypeConverter = astTypeConverter;

            OnWalkAstTypeConverter(astTypeConverter);

            Walk(astTypeConverter.AstBlock);

            OnExitAstTypeConverter(astTypeConverter);

            CurrentAstTypeConverter = null;
        }

        private void Walk(AstOperatorOverloadCollection astOperatorOverloadCollection)
        {
            foreach (AstOperatorOverload astOperatorOverload in astOperatorOverloadCollection)
                Walk(astOperatorOverload);
        }

        private void Walk(AstOperatorOverload astOperatorOverload)
        {
            CurrentAstOperatorOverload = astOperatorOverload;
            OnWalkAstOperatorOverload(astOperatorOverload);

            Walk(astOperatorOverload.AstBlock);

            OnExitAstOperatorOverload(astOperatorOverload);
            CurrentAstOperatorOverload = null;
        }

        private void Walk(AstAccessorCollection astAccessorCollection)
        {
            foreach (AstAccessor astAccessor in astAccessorCollection)
                Walk(astAccessor);
        }

        private void Walk(AstAccessor astAccessor)
        {
            CurrentAstAccessor = astAccessor;

            OnWalkAstAccessor(astAccessor);
            if (astAccessor.AstGetAccessor != null)
            { Walk(astAccessor.AstGetAccessor.AstBlock); }
            if (astAccessor.AstSetAccessor != null)
            {
                Walk(astAccessor.AstSetAccessor.AstBlock);
            }

            OnExitAstAccessor(astAccessor);

            CurrentAstAccessor = null;
        }

        private void Walk(AstConstructorCollection astConstructorCollection)
        {
            foreach (AstConstructor astConstructor in astConstructorCollection)
                Walk(astConstructor);
        }

        public virtual void WalkExit(AstClass astClass)
        {
            ExitAstClass(astClass);
        }


        public virtual void Walk(AstEnum astEnum)
        {
            CurrentAstEnum = astEnum;

            OnWalkAstEnum(astEnum);
            OnExitAstEnum(astEnum);

            CurrentAstEnum = null;
        }

        #endregion

        protected virtual void Walk(AstMethodCollection astMethodCollection)
        {
            foreach (AstMethod astMethod in astMethodCollection)
                Walk(astMethod);
        }

        public virtual void Walk(AstMethod astMethod)
        {
            CurrentAstMethod = astMethod;

            OnWalkAstMethod(astMethod);

            Walk(astMethod.Parameters);

            Walk(astMethod.AstBlock);

            OnExitAstMethod(astMethod);

            CurrentAstMethod = null;
        }


        public virtual void Walk(AstConstructor astConstructor)
        {
            CurrentAstConstructor = astConstructor;

            OnWalkAstConstructor(astConstructor);

            Walk(astConstructor.Parameters);

            Walk(astConstructor.AstBlock);

            OnExitAstConstructor(astConstructor);

            CurrentAstConstructor = null;
        }

        private void Walk(AstParameterCollection astParameterCollection)
        {
            OnWalkAstParameterCollection(astParameterCollection);

            foreach (AstParameter astParameter in astParameterCollection)
                OnWalkAstParameter(astParameter);

            OnExitAstParameterCollection(astParameterCollection);
        }

        public virtual void Walk(AstBlock astBlock)
        {
            CurrentAstBlock = astBlock;
            OnWalkAstBlock(astBlock);
            if (astBlock != null)
            {
                if (astBlock.AstStatementCollection != null)
                    Walk(astBlock.AstStatementCollection);
            }
            OnExitAstBlock(astBlock);
            CurrentAstBlock = null;
        }

        protected virtual void Walk(AstStatementCollection astStatementCollection)
        {
            foreach (AstStatement astStatement in astStatementCollection)
            {
                if (astStatement != null)
                    astStatement.Walk(this);
            }
        }

        #region AstStatements
        public virtual void Walk(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            OnWalkAstLocalVarDecl(astLocalVariableDeclaration);

            Walk(astLocalVariableDeclaration.Initialization);

            OnExitAstLocalVarDecl(astLocalVariableDeclaration);
        }

        public virtual void Walk(IAstExpression iAstExpression)
        {
            if (iAstExpression != null)
            {
                iAstExpression.Walk(this);
            }
        }

        public virtual void Walk(AstIfCondition astIfCondtion)
        {
            OnWalkAstIfCondition(astIfCondtion);

            Walk(astIfCondtion.Condition);
            Walk(astIfCondtion.AstStatement);
            Walk(astIfCondtion.AstStatementElse);

            OnExitAstIfCondition(astIfCondtion);
        }

        public virtual void Walk(AstForLoop astForLoop)
        {
            OnWalkAstForLoop(astForLoop);

            Walk(astForLoop.Initializers);
            Walk(astForLoop.Body);

            OnExitAstForLoop(astForLoop);
        }

        public virtual void Walk(AstDoLoop astDoLoop)
        {
            OnWalkAstDoLoop(astDoLoop);

            Walk(astDoLoop.AstStatement);
            Walk(astDoLoop.Condition);

            OnExitAstDoLoop(astDoLoop);
        }

        public virtual void Walk(AstWhileLoop astWhileLoop)
        {
            OnWalkAstWhileLoop(astWhileLoop);

            Walk(astWhileLoop.Condition);
            Walk(astWhileLoop.AstStatement);

            OnExitAstWhileLoop(astWhileLoop);
        }

        public virtual void Walk(AstAdditionExpression astAdditionExpression)
        {
            OnWalkAstAdditionExpression(astAdditionExpression);
            OnExitAstAdditionExpression(astAdditionExpression);
        }

        public virtual void Walk(AstAddAssignmentExpression astAddAssignmentExpression)
        {
            OnWalkAddAssingmentExpression(astAddAssignmentExpression);
            OnExitAddAssingmentExpression(astAddAssignmentExpression);
        }

        public virtual void Walk(AstVariableReference astVariableReference)
        {
            OnWalkAstVarRef(astVariableReference);
            OnExitAstVarRef(astVariableReference);
        }

        public virtual void Walk(AstAssignmentStatement assignmentStatement)
        {
            OnWalkAstAssignmentStatement(assignmentStatement);

            Walk(assignmentStatement.AstAssignmentExpression);

            OnExitAstAssignmentStatement(assignmentStatement);
        }

        public virtual void Walk(AstSimpleAssignmentExpression astSimpleAssignmentExpression)
        {
            OnWalkAstSimpleAssignmentExpression(astSimpleAssignmentExpression);
            // note: dont walk in left and right allow the user to do tat coz any mite come first
            OnExitAstSimpleAssignmentExpression(astSimpleAssignmentExpression);
        }

        public virtual void Walk(AstIntegerConstant astIntegerConstant)
        {
            OnWalkAstIntegerConstant(astIntegerConstant);
            OnExitAstIntegerConstant(astIntegerConstant);
        }

        public virtual void Walk(AstMethodCall astMethodCall)
        {
            AstMethodCall last = CurrentAstMethodCall;
            CurrentAstMethodCall = astMethodCall;

            OnWalkAstMethodCall(astMethodCall);
            Walk(astMethodCall.ArgumentCollection);
            OnExitAstMethodCall(astMethodCall);

            CurrentAstMethodCall = last;
        }

        private void Walk(AstArgumentCollection astArgumentCollection)
        {
            OnWalkAstArgumentCollection(astArgumentCollection);
            foreach (AstArgument astArgument in astArgumentCollection)
                Walk(astArgument);
            OnExitAstArgumentCollection(astArgumentCollection);
        }

        private void Walk(AstArgument astArgument)
        {
            OnWalkAstArgument(astArgument);

            Walk(astArgument.AstExpression);

            OnExitAstArgument(astArgument);
        }

        #region Loops

        #region WhileLoop

        public event OnWalkAstWhileLoop WalkAstWhileLoop = delegate { };

        public virtual void OnWalkAstWhileLoop(AstWhileLoop astWhileLoop)
        {
            WalkAstWhileLoop(astWhileLoop);
        }

        public event OnWalkAstWhileLoop ExitAstWhileLoop = delegate { };

        public virtual void OnExitAstWhileLoop(AstWhileLoop astWhileLoop)
        {
            ExitAstWhileLoop(astWhileLoop);
        }

        #endregion

        #region DoLoop

        public event OnWalkAstDoLoop WalkAstDoLoop = delegate { };

        public virtual void OnWalkAstDoLoop(AstDoLoop astDoLoop)
        {
            WalkAstDoLoop(astDoLoop);
        }

        public event OnWalkAstDoLoop ExitAstDoLoop = delegate { };

        public virtual void OnExitAstDoLoop(AstDoLoop astDoLoop)
        {
            ExitAstDoLoop(astDoLoop);
        }

        #endregion

        #region ForLoop

        public event OnWalkAstForLoop WalkAstForLoop = delegate { };

        public virtual void OnWalkAstForLoop(AstForLoop astForLoop)
        {
            WalkAstForLoop(astForLoop);
        }

        public event OnWalkAstForLoop ExitAstForLoop = delegate { };

        public virtual void OnExitAstForLoop(AstForLoop astForLoop)
        {
            ExitAstForLoop(astForLoop);
        }

        #endregion

        #endregion

        #region IfCondition

        public event OnWalkAstIfCondition WalkAstIfCondition = delegate { };

        public virtual void OnWalkAstIfCondition(AstIfCondition astIfCondition)
        {
            WalkAstIfCondition(astIfCondition);
        }

        public event OnWalkAstIfCondition ExitAstIfCondition = delegate { };

        public virtual void OnExitAstIfCondition(AstIfCondition astIfCondition)
        {
            ExitAstIfCondition(astIfCondition);
        }

        #endregion

        #region Accessor

        public event OnWalkAstAccessor WalkAstAccessor = delegate { };

        public virtual void OnWalkAstAccessor(AstAccessor astAccessor)
        {
            WalkAstAccessor(astAccessor);
        }

        public event OnWalkAstAccessor ExitAstAccessor = delegate { };

        public virtual void OnExitAstAccessor(AstAccessor astAccessor)
        {
            ExitAstAccessor(astAccessor);
        }

        #endregion

        #region OperatorOverload

        public event OnWalkAstOperatorOverload WalkAstOperatorOverload = delegate { };

        public virtual void OnWalkAstOperatorOverload(AstOperatorOverload astOperatorOverload)
        {
            WalkAstOperatorOverload(astOperatorOverload);
        }

        public event OnWalkAstOperatorOverload ExitAstOperatorOverload = delegate { };

        public virtual void OnExitAstOperatorOverload(AstOperatorOverload astOperatorOverload)
        {
            ExitAstOperatorOverload(astOperatorOverload);
        }

        #endregion

        #region TypeConverter

        public event OnWalkAstTypeConverter WalkAstTypeConverter = delegate { };

        public virtual void OnWalkAstTypeConverter(AstTypeConverter astTypeConverter)
        {
            WalkAstTypeConverter(astTypeConverter);
        }

        public event OnWalkAstTypeConverter ExitAstTypeConverter = delegate { };

        public virtual void OnExitAstTypeConverter(AstTypeConverter astTypeConverter)
        {
            ExitAstTypeConverter(astTypeConverter);
        }

        #endregion

        #endregion

        #endregion

        #region Events

        public event OnWalk WalkStart = delegate { };
        public virtual void OnWalkStart()
        {
            WalkStart();
        }

        public event OnWalk WalkEnd = delegate { };
        public virtual void OnWalkEnd()
        {
            WalkEnd();
        }

        public event OnWalkAstProgram WalkAstProgram = delegate { };
        public virtual void OnWalkAstProgram(AstProgram astProgarm)
        {
            WalkAstProgram(astProgarm);
        }

        public event OnWalkAstSourceFile OnWalkAstSourceFile = delegate { };
        public event OnWalkAstSourceFile OnExitAstSourceFile = delegate { };
        public event OnWalkAstUsingDirective OnWalkAstUsingDirective = delegate { };
        public event OnWalkAstNamespaceBlock OnWalkAstNamespaceBlock = delegate { };
        public event OnWalkAstNamespaceBlock OnExitAstNamespaceBlock = delegate { };

        #region AstTypes
        public event OnWalkAstClass WalkAstClass = delegate { };
        public virtual void OnWalkAstClass(AstClass astClass)
        {
            WalkAstClass(astClass);
        }

        public event OnWalkAstClass ExitAstClass = delegate { };
        public virtual void OnExitAstClass(AstClass astClass)
        {
            ExitAstClass(astClass);
        }

        #region Struct

        public event OnWalkAstStruct WalkAstStruct = delegate { };

        public virtual void OnWalkAstStruct(AstStruct astStruct)
        {
            WalkAstStruct(astStruct);
        }

        public event OnWalkAstStruct ExitAstStruct = delegate { };

        public virtual void OnExitAstStruct(AstStruct astStruct)
        {
            ExitAstStruct(astStruct);
        }

        #endregion

        public event OnWalkAstEnum OnWalkAstEnum = delegate { };
        public event OnWalkAstEnum OnExitAstEnum = delegate { };
        #endregion

        #region Method

        public event OnWalkAstMethod WalkAstMethod = delegate { };

        public virtual void OnWalkAstMethod(AstMethod astMethod)
        {
            WalkAstMethod(astMethod);
        }

        public event OnWalkAstMethod ExitAstMethod = delegate { };

        public virtual void OnExitAstMethod(AstMethod astMethod)
        {
            ExitAstMethod(astMethod);
        }
        #endregion

        #region Constructor

        public event OnWalkAstConstructor WalkAstConstructor = delegate { };

        public virtual void OnWalkAstConstructor(AstConstructor astConstructor)
        {
            WalkAstConstructor(astConstructor);
        }

        public event OnWalkAstConstructor ExitAstConstructor = delegate { };

        public virtual void OnExitAstConstructor(AstConstructor astConstructor)
        {
            ExitAstConstructor(astConstructor);
        }

        #endregion

        public event OnWalkAstParameterCollection WalkAstParameterCollection = delegate { };
        public virtual void OnWalkAstParameterCollection(AstParameterCollection astParameterCollection)
        {
            ParameterIndex = 0;
            WalkAstParameterCollection(astParameterCollection);
        }

        public event OnWalkAstParameterCollection ExitAstParameterCollection = delegate { };
        public virtual void OnExitAstParameterCollection(AstParameterCollection astParameterCollection)
        {
            ExitAstParameterCollection(astParameterCollection);
            ParameterIndex = null;
        }

        public event OnWalkAstParameter WalkAstParameter = delegate { };
        public virtual void OnWalkAstParameter(AstParameter astParameter)
        {
            WalkAstParameter(astParameter);
            ++ParameterIndex;
        }

        #region Block

        public event OnWalkAstBlock WalkAstBlock = delegate { };

        public virtual void OnWalkAstBlock(AstBlock astBlock)
        {
            WalkAstBlock(astBlock);
        }

        public event OnWalkAstBlock ExitAstBlock = delegate { };

        public virtual void OnExitAstBlock(AstBlock astBlock)
        {
            ExitAstBlock(astBlock);
        }

        #endregion

        #region AstStatements

        #region AstLocalVariableDeclaration

        public event OnWalkAstLocalVarDecl WalkAstLocalVarDecl = delegate { };
        public virtual void OnWalkAstLocalVarDecl(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            WalkAstLocalVarDecl(astLocalVariableDeclaration);
        }

        public event OnWalkAstLocalVarDecl ExitAstLocalVarDecl = delegate { };
        public virtual void OnExitAstLocalVarDecl(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            ExitAstLocalVarDecl(astLocalVariableDeclaration);
        }

        #endregion

        #region AstSimpleAssignment

        public event OnWalkAstAssignmentStatement WalkAstAssignmentStatement = delegate { };
        public virtual void OnWalkAstAssignmentStatement(AstAssignmentStatement astAssignmentStatement)
        {
            WalkAstAssignmentStatement(astAssignmentStatement);
        }

        public event OnWalkAstAssignmentStatement ExitAstAssignmentStatement = delegate { };
        public virtual void OnExitAstAssignmentStatement(AstAssignmentStatement astAssignmentStatement)
        {
            ExitAstAssignmentStatement(astAssignmentStatement);
        }

        #endregion

        #endregion

        #region Expressions

        #region AstIntegerConstant

        public event OnWalkAstIntegerConstant WalkAstIntegerConstant = delegate { };
        public virtual void OnWalkAstIntegerConstant(AstIntegerConstant astIntegerConstant)
        {
            WalkAstIntegerConstant(astIntegerConstant);
        }

        public event OnWalkAstIntegerConstant ExitAstIntegerConstant = delegate { };
        public virtual void OnExitAstIntegerConstant(AstIntegerConstant astIntegerConstant)
        {
            ExitAstIntegerConstant(astIntegerConstant);
        }

        #endregion

        #region AstMethodCall

        public event OnWalkAstMethodCall WalkAstMethodCall = delegate { };
        public virtual void OnWalkAstMethodCall(AstMethodCall astMethodCall)
        {
            WalkAstMethodCall(astMethodCall);
        }

        public event OnWalkAstMethodCall ExitAstMethodCall = delegate { };
        public virtual void OnExitAstMethodCall(AstMethodCall astMethodCall)
        {
            ExitAstMethodCall(astMethodCall);
        }

        #endregion

        #region AstArgumentCollection

        public event OnWalkAstArgumentCollection WalkAstArgumentCollection = delegate { };
        public virtual void OnWalkAstArgumentCollection(AstArgumentCollection astArgumentCollection)
        {
            ArgumentIndex = 0;
            WalkAstArgumentCollection(astArgumentCollection);
        }

        public event OnWalkAstArgumentCollection ExitAstArgumentCollection = delegate { };
        public virtual void OnExitAstArgumentCollection(AstArgumentCollection astArgumentCollection)
        {
            ExitAstArgumentCollection(astArgumentCollection);
            ArgumentIndex = null;
        }

        #endregion

        #region AstArgument

        public event OnWalkAstArgument WalkAstArgument = delegate { };
        public virtual void OnWalkAstArgument(AstArgument astArgument)
        {
            WalkAstArgument(astArgument);
        }

        public event OnWalkAstArgument ExitAstArgument = delegate { };
        public virtual void OnExitAstArgument(AstArgument astArgument)
        {
            ExitAstArgument(astArgument);
        }

        #endregion

        #region AstSimpleAssignment

        public event OnWalkAstSimpleAssignmentExpression WalkAstSimpleAssignmentExpression = delegate { };
        public virtual void OnWalkAstSimpleAssignmentExpression(AstSimpleAssignmentExpression AstSimpleAssignmentExpression)
        {
            WalkAstSimpleAssignmentExpression(AstSimpleAssignmentExpression);
        }

        public event OnWalkAstSimpleAssignmentExpression ExitAstSimpleAssignmentExpression = delegate { };
        public virtual void OnExitAstSimpleAssignmentExpression(AstSimpleAssignmentExpression AstSimpleAssignmentExpression)
        {
            ExitAstSimpleAssignmentExpression(AstSimpleAssignmentExpression);
        }

        #endregion

        #region AstVariableReference

        public event OnWalkAstVarRef WalkAstVarRef = delegate { };
        public virtual void OnWalkAstVarRef(AstVariableReference astVariableReference)
        {
            WalkAstVarRef(astVariableReference);
        }

        public event OnWalkAstVarRef ExitAstVarRef = delegate { };
        public virtual void OnExitAstVarRef(AstVariableReference astVariableReference)
        {
            ExitAstVarRef(astVariableReference);
        }

        #endregion

        #region AstAddAssignmentExpression

        public event OnWalkAstAddAssignmentExpression WalkAstAddAssingmentExpression = delegate { };
        public virtual void OnWalkAddAssingmentExpression(AstAddAssignmentExpression astAddAssignmentExpression)
        {
            WalkAstAddAssingmentExpression(astAddAssignmentExpression);
        }

        public event OnWalkAstAddAssignmentExpression ExitAstAddAssingmentExpression = delegate { };
        public virtual void OnExitAddAssingmentExpression(AstAddAssignmentExpression astAddAssignmentExpression)
        {
            ExitAstAddAssingmentExpression(astAddAssignmentExpression);
        }

        #endregion

        #region AstAssignmentExpression

        public event OnWalkAstAdditionExpression WalkAstAdditionExpression = delegate { };
        public virtual void OnWalkAstAdditionExpression(AstAdditionExpression astAdditionExpression)
        {
            WalkAstAdditionExpression(astAdditionExpression);
        }

        public event OnWalkAstAdditionExpression ExitAstAdditionExpression = delegate { };
        public virtual void OnExitAstAdditionExpression(AstAdditionExpression astAdditionExpression)
        {
            ExitAstAdditionExpression(astAdditionExpression);
        }

        #endregion

        #endregion


        #endregion



    }
}
