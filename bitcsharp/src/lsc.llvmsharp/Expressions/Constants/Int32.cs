using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstIntegerConstant astIntegerConstant)
        {
            LLVM.Alloca a = new LLVM.Alloca(LLVMModule)
                                {
                                    Result = "%" + TempCount,
                                    Type = "i32"
                                };

            WriteLine(2, a.EmitCode());

            LLVM.Store s = new LLVM.Store(LLVMModule)
                               {
                                   Type = "i32",
                                   Value = astIntegerConstant.ConstantValue.ToString(),
                                   Pointer = "%" + TempCount
                               };

            WriteLine(2, s.EmitCode());


            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astIntegerConstant.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            TempCount++;

        }
    }
}