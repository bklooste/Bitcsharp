
namespace LLVMSharp.Compiler.CodeGenerators
{
    public interface ICodeGenerator
    {
        void EmitCode(CodeGenerator cgen);
    }
}