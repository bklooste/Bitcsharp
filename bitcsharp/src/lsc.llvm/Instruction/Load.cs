using System.Text;
using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    ///  &lt;result> = load &lt;type>* &lt;pointer>
    /// </summary>
    public class Load : Instruction
    {
        public string Result;
        public string Type;
        public string Pointer;

        public Load(Module module)
            : base(module)
        {
        }

        //public Load(LLVMSharpCodeGenerator llvmCodeGenerator)
        //    : this(llvmCodeGenerator.LLVMModule)
        //{
        //}

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Result);
            sb.Append(" = load ");
            sb.Append(Type);
            sb.Append("* ");
            sb.Append(Pointer);

            return sb.ToString();
        }
    }
}
