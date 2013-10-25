using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstUnaryMinus astUnaryMinus)
        {
            astUnaryMinus.AstExpression.EmitCode(this);

            if (astUnaryMinus.AssociatedType == "System.Int32")
            {
                Sub s = new Sub(LLVMModule)
                {
                    Operand1 = "0",
                    Operand2 = "%" + TempCount,
                    Result = "%" + (TempCount + 1),
                    Type = "i32"
                };
                WriteLine(2, s.EmitCode());
            }

            else if (astUnaryMinus.AssociatedType == "System.Single")
            {
                Sub s = new Sub(LLVMModule)
                {
                    Operand1 = "0.0",
                    Operand2 = "%" + TempCount,
                    Result = "%" + (TempCount + 1),
                    Type = "float"
                };
                WriteLine(2, s.EmitCode());
            }           

            ++TempCount;
            
        }
    }
}