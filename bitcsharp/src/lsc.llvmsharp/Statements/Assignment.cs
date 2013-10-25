using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstAssignmentStatement astAssignmentStatement)
        {
            astAssignmentStatement.AstAssignmentExpression.EmitCode(this);
            ++TempCount; // increment it coz astAssignment always loads the value to the latest temp
                         // increasing it allows the next statement to start with fresth temp count
        }
    }
}