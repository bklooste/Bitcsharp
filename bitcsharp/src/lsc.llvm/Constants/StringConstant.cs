using System;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class StringConstant : Constant
    {
        //public StringConstant(Module module)
        //    : base(module)
        //{
        //}

        //public object Value;

        //public string StringValue
        //{
        //    get { return Value.ToString(); }
        //    set { Value = value; }
        //}

        //public int Length
        //{
        //    get { return Value.ToString().Length; }
        //}

        //public override string GenerateCode()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("[");
        //    sb.Append(Length);
        //    sb.Append(" x i8] c\"");
        //    sb.Append(StringValue.Replace("\n", "\\0A"));
        //    sb.Append("\\00\"");
        //    return sb.ToString();
        //}
        public override string EmitCode()
        {
            throw new NotImplementedException();
        }
    }
}
