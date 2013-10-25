using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstBooleanConstant astBooleanConstant)
        {
            LLVM.Alloca a = new LLVM.Alloca(LLVMModule)
            {
                Result = "%" + TempCount,
                Type = "i8"
            };

            WriteLine(2, a.EmitCode());

            LLVM.Store s = new LLVM.Store(LLVMModule)
            {
                Type = "i8",
                Value = astBooleanConstant.ConstantValue ? "1" : "0",
                Pointer = "%" + TempCount
            };

            WriteLine(2, s.EmitCode());

            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astBooleanConstant.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            TempCount++;
        }
    }
}