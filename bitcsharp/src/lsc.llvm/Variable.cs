using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class Variable : ModulePart, ILLVMCodeGenerator
    {

        public Variable(Module module)
            : base(module)
        {
            module.VariableCollection.Add(this);
        }

        //public Variable(LLVMSharpCodeGenerator llvmCodeGenerator)
        //    : this(llvmCodeGenerator.LLVMModule)
        //{
        //}

        public bool IsGlobal;
        public bool IsConstant;
        public string Name;
        public LinkageType LinkageType = LinkageType.Internal;
        public Constant Value;

        public string EmitCode()
        {
            StringBuilder sb = new StringBuilder();
            if (IsGlobal) sb.Append("@");
            sb.Append(Name);
            sb.Append(" = ");
            sb.Append(Helpers.ToString(LinkageType) + " " ?? "");
            if (IsConstant) sb.Append("constant ");
            sb.Append(Value.EmitCode());
            return sb.ToString();
        }
    }

    public class VariableCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is Variable))
            {
                throw new ArgumentException("Value must be of Type Variable");
            }

            return base.Add(value);
        }
    }
}
