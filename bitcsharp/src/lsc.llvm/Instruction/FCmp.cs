using System;
using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public enum FCondition
    {
        False,
        OrderedEqual,
        OrderedGreaterThan,
        OrderedGreaterThanEqual,
        OrderedLessThan,
        OrderedLessThanEqual,
        OrderedNotEqual,
        Ordered,
        UnorderedEqual,
        UnorderedGreaterThan,
        UnorderedLessThan,
        UnorderedLessThanEqual,
        UnorderedNotEqual,
        Unordered,
        True
    }

    /// <summary>
    ///   &lt;result> = icmp &lt;cond> &lt;ty> &ltop1>, &lt;op2>
    /// </summary>
    public class FCmp : Instruction
    {
        public FCondition Condition;
        public string Operand1;
        public string Operand2;
        public string Result;
        public string Type;

        public FCmp(Module module)
            : base(module)
        {
        }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} = fcmp {1} {2} {3}, {4}",
                            Result, ToString(Condition), Type, Operand1, Operand2);

            return sb.ToString();
        }

        public static FCmp ToBool(Module module, string result, string operandToConvert)
        {
            return new FCmp(module)
                       {
                           Result = result,
                           Condition = FCondition.UnorderedNotEqual,
                           Type = "i8",
                           Operand1 = operandToConvert,
                           Operand2 = "0"
                       };
        }

        public static string ToString(FCondition condition)
        {
            switch (condition)
            {
                case FCondition.False:
                    return "false";
                case FCondition.Ordered:
                    return "ord";
                case FCondition.OrderedEqual:
                    return "oeq";
                case FCondition.OrderedGreaterThan:
                    return "ogt";
                case FCondition.OrderedGreaterThanEqual:
                    return "oge";
                case FCondition.OrderedLessThan:
                    return "olt";
                case FCondition.OrderedLessThanEqual:
                    return "ole";
                case FCondition.OrderedNotEqual:
                    return "one";
                case FCondition.True:
                    return "true";
                case FCondition.Unordered:
                    return "uno";
                case FCondition.UnorderedEqual:
                    return "ueq";
                case FCondition.UnorderedGreaterThan:
                    return "ugt";
                case FCondition.UnorderedLessThan:
                    return "ult";
                case FCondition.UnorderedLessThanEqual:
                    return "ule";
                case FCondition.UnorderedNotEqual:
                    return "une"; 
                default:
                    throw new ArgumentOutOfRangeException("condition");
            }
        }
    }
}