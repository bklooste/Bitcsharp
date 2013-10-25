
using LLVMSharp.Compiler.CodeGenerators.LLVM;
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    /// Returns the quotient of its two operands
    /// </summary>
    public class FDiv : BinaryOperation
    {
        public FDiv(Module module)
            : base(module)
        {
        }

        //public FDiv(LLVMCodeGenerator llvmCodeGenerator)
        //    : this(llvmCodeGenerator.LLVMModule)
        //{
        //}
        
        public override string InstructionName
        {
            get { return "fdiv"; }
        }
    }
}
