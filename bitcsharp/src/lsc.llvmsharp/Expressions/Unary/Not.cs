using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstNot astNot)
        {
            astNot.AstExpression.EmitCode(this);

            ICmp toBool = ICmp.ToBool(LLVMModule, "%" + (TempCount + 1), "%" + TempCount);
            WriteLine(2, toBool.EmitCode());
            ++TempCount;

            Xor toBoolNot = new Xor(LLVMModule)
                                {
                                    Type = "i1",
                                    Operand1 = toBool.Result,
                                    Operand2 = "true",
                                    Result = "%" + (TempCount + 1)
                                };
            WriteLine(2, toBoolNot.EmitCode());
            ++TempCount;

            Zext toBoolNotToInt = new Zext(LLVMModule)
                                 {
                                     Result = "%" + (TempCount + 1),
                                     Type = "i1",
                                     Type2 = "i8",
                                     Value = toBoolNot.Result
                                 };
            WriteLine(2, toBoolNotToInt.EmitCode());
            ++TempCount;
        }
    }
}