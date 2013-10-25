using System;
using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public enum Condition
    {
        Equal,
        NotEqual,
        UnsignedGreaterThan,
        UnsignedGreaterOrEqual,
        UnsignedLessThan,
        UnsignedLessOrEqual,
        SignedGreaterThan,
        SigendGreaterOrEqual,
        SignedLessThan,
        SignedLessOrEqual
    }

    /// <summary>
    ///   &lt;result> = icmp &lt;cond> &lt;ty> &ltop1>, &lt;op2>
    /// </summary>
    public class ICmp : Instruction
    {
        public Condition Condition;
        public string Operand1;
        public string Operand2;
        public string Result;
        public string Type;

        public ICmp(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} = icmp {1} {2} {3}, {4}",
                            Result, ToString(Condition), Type, Operand1, Operand2);

            return sb.ToString();
        }

        public static ICmp ToBool(Module module, string result, string operandToConvert)
        {
            return new ICmp(module)
                       {
                           Result = result,
                           Condition = Condition.NotEqual,
                           Type = "i8",
                           Operand1 = operandToConvert,
                           Operand2 = "0"
                       };
        }

        public static string ToString(Condition condition)
        {
            switch (condition)
            {
                case Condition.Equal:
                    return "eq";
                case Condition.NotEqual:
                    return "ne";
                case Condition.UnsignedGreaterThan:
                    return "ugt";
                case Condition.UnsignedGreaterOrEqual:
                    return "uge";
                case Condition.UnsignedLessThan:
                    return "ult";
                case Condition.UnsignedLessOrEqual:
                    return "ule";
                case Condition.SignedGreaterThan:
                    return "sgt";
                case Condition.SigendGreaterOrEqual:
                    return "sge";
                case Condition.SignedLessThan:
                    return "slt";
                case Condition.SignedLessOrEqual:
                    return "sle";
                default:
                    throw new ArgumentOutOfRangeException("condition");
            }
        }
    }
}