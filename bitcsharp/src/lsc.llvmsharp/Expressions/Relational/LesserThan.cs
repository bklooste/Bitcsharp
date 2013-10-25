using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstLesserThanExpression astLesserThanExpression)
        {
            switch (astLesserThanExpression.RightOperand.AssociatedType)
            {
                case "System.Int32":
                    EmitCodeForInt32(astLesserThanExpression);
                    break;
                case "System.Single":
                    EmitCodeForFloat(astLesserThanExpression);
                    break;
                //todo: cgen of less than for enum
            }
        }

        private void EmitCodeForFloat(AstLesserThanExpression astLesserThanExpression)
        {
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "ltSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "ltSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "ltEnd_" + LoopCount };
            Alloca a = new Alloca(LLVMModule)
            {
                Type = "i8",
                Result = "%iftmp." + LoopCount
            };
            UnConditionalBranch gotoEnd = new UnConditionalBranch(LLVMModule)
            {
                Destination = labelEnd.Name
            };
            ++LoopCount;
            #endregion

            WriteLine(2, a.EmitCode());

            astLesserThanExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astLesserThanExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            FCmp cmp = new FCmp(LLVMModule)
            {
                Type = "float",
                Condition = FCondition.OrderedLessThan,
                Operand1 = "%" + leftResult,
                Operand2 = "%" + rightResult,
                Result = "%" + TempCount
            };

            WriteLine(2, cmp.EmitCode());

            ConditionalBranch b = new ConditionalBranch(LLVMModule)
            {
                Condition = cmp.Result,
                IfTrue = labelSetTrue.Name,
                IfFalse = labelSetFalse.Name
            };
            WriteLine(2, b.EmitCode());

            #region Set False
            WriteLine(1, labelSetFalse.EmitCode());
            Store storeFalse = new Store(LLVMModule)
            {
                Type = "i8",
                Value = "0",
                Pointer = a.Result
            };
            WriteLine(2, storeFalse.EmitCode());
            WriteLine(2, gotoEnd.EmitCode());
            #endregion

            #region Set True
            WriteLine(1, labelSetTrue.EmitCode());
            Store storeTrue = new Store(LLVMModule)
            {
                Type = "i8",
                Value = "1",
                Pointer = a.Result
            };
            WriteLine(2, storeTrue.EmitCode());
            WriteLine(2, gotoEnd.EmitCode());
            #endregion

            #region End
            WriteLine(1, labelEnd.EmitCode());
            ++TempCount;
            Load l = new Load(LLVMModule)
            {
                Type = "i8",
                Pointer = a.Result,
                Result = "%" + TempCount
            };
            WriteLine(2, l.EmitCode());
            #endregion
        }

        private void EmitCodeForInt32(AstLesserThanExpression astLesserThanExpression)
        {
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "ltSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "ltSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "ltEnd_" + LoopCount };
            Alloca a = new Alloca(LLVMModule)
            {
                Type = "i8",
                Result = "%iftmp." + LoopCount
            };
            UnConditionalBranch gotoEnd = new UnConditionalBranch(LLVMModule)
            {
                Destination = labelEnd.Name
            };
            ++LoopCount;
            #endregion

            WriteLine(2, a.EmitCode());

            astLesserThanExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astLesserThanExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            ICmp cmp = new ICmp(LLVMModule)
                           {
                               Type = "i32",
                               Condition = Condition.SignedLessThan,
                               Operand1 = "%" + leftResult,
                               Operand2 = "%" + rightResult,
                               Result = "%" + TempCount
                           };

            WriteLine(2, cmp.EmitCode());

            ConditionalBranch b = new ConditionalBranch(LLVMModule)
            {
                Condition = cmp.Result,
                IfTrue = labelSetTrue.Name,
                IfFalse = labelSetFalse.Name
            };
            WriteLine(2, b.EmitCode());

            #region Set False
            WriteLine(1, labelSetFalse.EmitCode());
            Store storeFalse = new Store(LLVMModule)
            {
                Type = "i8",
                Value = "0",
                Pointer = a.Result
            };
            WriteLine(2, storeFalse.EmitCode());
            WriteLine(2, gotoEnd.EmitCode());
            #endregion

            #region Set True
            WriteLine(1, labelSetTrue.EmitCode());
            Store storeTrue = new Store(LLVMModule)
            {
                Type = "i8",
                Value = "1",
                Pointer = a.Result
            };
            WriteLine(2, storeTrue.EmitCode());
            WriteLine(2, gotoEnd.EmitCode());
            #endregion

            #region End
            WriteLine(1, labelEnd.EmitCode());
            ++TempCount;
            Load l = new Load(LLVMModule)
            {
                Type = "i8",
                Pointer = a.Result,
                Result = "%" + TempCount
            };
            WriteLine(2, l.EmitCode());
            #endregion
        }
    }
}