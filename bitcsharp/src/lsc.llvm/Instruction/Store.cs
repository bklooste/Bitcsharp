using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    /// <summary>
    /// store &lt;ty> &lt;value>, &lt;ty>* &lt;pointer>
    /// </summary>
    public class Store : Instruction
    {
        public string Type;
        public string Value;
        public string Pointer;

        public Store(Module module)
            : base(module)
        {
        }

        //public Store(LLVMSharpCodeGenerator llvmCodeGenerator)
        //    : this(llvmCodeGenerator.LLVMModule)
        //{
        //}

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("store ");
            sb.Append(Type);
            sb.Append(" ");
            sb.Append(Value);
            sb.Append(", ");
            sb.Append(Type);
            sb.Append("* ");
            sb.Append(Pointer);

            return sb.ToString();
        }

        public static Store StoreNull(Module module, string type, string pointer)
        {
            Store s = new Store(module)
                          {
                              Type = type,
                              Value = "null",
                              Pointer = pointer
                          };
            return s;
        }
    }
}
