using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public abstract partial class CodeGenerator
    {
        public abstract void EmitCode(AstMethodCall astMethodCall);

        public abstract void EmitCode(AstLocalVariableDeclaration astLocalVariableDeclaration);

        public abstract void EmitCode(AstAdditionExpression astAdditionExpression);
        public abstract void EmitCode(AstSubtractionExpression astSubtractionExpression);
        public abstract void EmitCode(AstMuliplicationExpression astMultiplicationExpression);

        public abstract void EmitCode(AstStringConstant astStringConstant);
        public abstract void EmitCode(AstIntegerConstant astIntegerConstant);

        public abstract void EmitCode(AstVariableReference astVariableReference);

        public abstract void EmitCode(AstSimpleAssignmentExpression astSimpleAssignmentExpression);

        public abstract void EmitCode(AstBooleanConstant astBooleanConstant);

        public abstract void EmitCode(AstIfCondition astIfCondition);
        public abstract void EmitCode(AstDoLoop astDoLoop);
        public abstract void EmitCode(AstForLoop astForLoop);

        public abstract void EmitCode(AstWhileLoop astWhileLoop);
        public abstract void EmitCode(AstUnaryPlus astUnaryPlus);
        public abstract void EmitCode(AstUnaryMinus astUnaryMinus);
        public abstract void EmitCode(AstDivisionExpression astDivisionExpression);

        public abstract void EmitCode(AstNot astNot);

        public abstract void EmitCode(AstPreIncrement astPreIncrement);
        public abstract void EmitCode(AstPreDecrement astPreDecrement);
        public abstract void EmitCode(AstPostIncrement astPostIncrement);
        public abstract void EmitCode(AstPostDecrement astPostDecrement);

        public abstract void EmitCode(AstAssignmentStatement astAssignmentStatement);

        public abstract void EmitCode(AstAndExpression astAndExpression);
        public abstract void EmitCode(AstOrExpression astOrExpression);
        public abstract void EmitCode(AstRealConstant astRealConstant);
        public abstract void EmitCode(AstEqualityExpression astEqualityExpression);
        public abstract void EmitCode(AstInequalityExpression astInequalityExpression);
        public abstract void EmitCode(AstAddAssignmentExpression astAddAssignmentExpression);
        public abstract void EmitCode(AstDivisionAssignmentExpression astDivisionAssignmentExpression);
        public abstract void EmitCode(AstMultiplyAssignmentExpression astMultiplyAssignmentExpression);
        public abstract void EmitCode(AstSubtractAssignmentExpression astSubtractAssignmentExpression);

        public abstract void EmitCode(AstLesserThanExpression astLesserThanExpression);
        public abstract void EmitCode(AstLesserThanOrEqualExpression astLesserThanOrEqualExpression);
        public abstract void EmitCode(AstGreaterThanExpression astGreaterThanExpression);
        public abstract void EmitCode(AstGreaterThanOrEqualExpression astGreaterThanOrEqualExpression);

        public abstract void EmitCode(AstReturn astReturn);

        public abstract void EmitCode(AstNull astNull);

        public abstract void EmitCode(AstNewType astNewType);

        public abstract void EmitCode(AstStatementCollection astStatementCollection);

        public abstract void EmitCode(AstBlock astBlock);
    }
}