using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstDivisionExpression astDivisionExpression)
        {
            astDivisionExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astDivisionExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;
            if (astDivisionExpression.AssociatedType == "System.Int32")
            {
                #region Div

                LLVM.SDiv sdiv = new LLVM.SDiv(LLVMModule)
                {
                    Operand1 = "%" + leftResult,
                    Operand2 = "%" + rightResult,
                    Result = "%" + (TempCount),
                    Type = LLVMTypeName(astDivisionExpression.AssociatedType)
                };

                WriteLine(2, sdiv.EmitCode());

                LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "i32"
                };

                WriteLine(2, alloc.EmitCode());


                LLVM.Store s = new LLVM.Store(LLVMModule)
                {
                    Type = "i32",
                    Value = "%" + (TempCount),
                    Pointer = "%" + (TempCount + 1)
                };

                WriteLine(2, s.EmitCode());

                ++TempCount;

                #region load

                LLVM.Load l = new LLVM.Load(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = LLVMTypeName(astDivisionExpression.AssociatedType),
                    Pointer = "%" + TempCount
                };

                WriteLine(2, l.EmitCode());

                ++TempCount;

                #endregion

                #endregion
            }
            else if (astDivisionExpression.AssociatedType == "System.Single")
            {
                #region Fdiv
                LLVM.FDiv fdiv = new LLVM.FDiv(LLVMModule)
                {
                    Operand1 = "%" + leftResult,
                    Operand2 = "%" + rightResult,
                    Result = "%" + (TempCount),
                    Type = LLVMTypeName(astDivisionExpression.AssociatedType)
                };

                WriteLine(2, fdiv.EmitCode());

                LLVM.Alloca alloc = new LLVM.Alloca(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "float"
                };

                WriteLine(2, alloc.EmitCode());


                LLVM.Store s = new LLVM.Store(LLVMModule)
                {
                    Type = "float",
                    Value = "%" + (TempCount),
                    Pointer = "%" + (TempCount + 1)
                };

                WriteLine(2, s.EmitCode());

                ++TempCount;
                #endregion

                #region load

                LLVM.Load l = new LLVM.Load(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = LLVMTypeName(astDivisionExpression.AssociatedType),
                    Pointer = "%" + TempCount
                };

                WriteLine(2, l.EmitCode());

                ++TempCount;

                #endregion
            }
        }
    }
}