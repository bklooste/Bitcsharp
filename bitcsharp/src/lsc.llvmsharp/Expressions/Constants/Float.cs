using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;
using System;
using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstRealConstant astRealConstant)
        {
            LLVM.Alloca a = new LLVM.Alloca(LLVMModule)
                                {
                                    Result = "%" + TempCount,
                                    Type = "float"
                                };

            WriteLine(2, a.EmitCode());

            LLVM.Store s = new LLVM.Store(LLVMModule)
                               {
                                   Type = "float",                                   
                                   Value = HexFloatFormat(astRealConstant.ConstantValue),
                                   Pointer = "%" + TempCount
                               };

            WriteLine(2, s.EmitCode());


            LLVM.Load l = new LLVM.Load(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = LLVMTypeName(astRealConstant.AssociatedType),
                Pointer = "%" + TempCount
            };

            WriteLine(2, l.EmitCode());

            TempCount++;

        }

        private string HexFloatFormat(double dbl)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0x");
            float flt = Convert.ToSingle(dbl.ToString());
            long result = BitConverter.DoubleToInt64Bits(flt);
            sb.Append(result.ToString("X"));
            return sb.ToString();
        }
    }
}