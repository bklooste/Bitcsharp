using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstNull astNull)
        {
            Alloca a = new Alloca(LLVMModule)
                         {
                             Type = LLVMTypeName(astNull.FQT) + "*",
                             Result = "%" + TempCount
                         };
            WriteLine(2, a.EmitCode());

            Store storeNull = Store.StoreNull(LLVMModule, a.Type, a.Result);
            ++TempCount;

            WriteLine(2, storeNull.EmitCode());

            Load l = new Load(LLVMModule)
                       {
                           Pointer = a.Result,
                           Type = a.Type,
                           Result = "%" + TempCount
                       };
            WriteLine(2, l.EmitCode());
        }
    }
}