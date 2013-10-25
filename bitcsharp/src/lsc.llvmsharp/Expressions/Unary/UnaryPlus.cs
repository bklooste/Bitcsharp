using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstUnaryPlus astUnaryPlus)
        {
            //Unary + operators are predefined for all numeric types. 
            // The result of a unary + operation on a numeric type is just the value of the operand.
            // http://msdn.microsoft.com/en-us/library/k1a63xkz(VS.100).aspx
            astUnaryPlus.AstExpression.EmitCode(this);
        }
    }
}