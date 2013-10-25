using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstOrExpression astOrExpression)
        {
            #region Declarations
            Label labelNext = new Label(LLVMModule) { Name = "orNext_" + LoopCount };
            Label labelSetFalse = new Label(LLVMModule) { Name = "orSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "orSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "orEnd_" + LoopCount };
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

            astOrExpression.LeftOperand.EmitCode(this);
            ICmp toBoolLeft = ICmp.ToBool(LLVMModule, "%" + (TempCount + 1), "%" + TempCount);
            ++TempCount;
            WriteLine(2, toBoolLeft.EmitCode());
            ++TempCount;

            ConditionalBranch b = new ConditionalBranch(LLVMModule)
            {
                Condition = toBoolLeft.Result,
                IfTrue = labelSetTrue.Name,
                IfFalse = labelNext.Name
            };
            WriteLine(2, b.EmitCode());

            WriteLine(1, labelNext.EmitCode());

            astOrExpression.RightOperand.EmitCode(this);
            ICmp toBoolRigth = ICmp.ToBool(LLVMModule, "%" + (TempCount + 1), "%" + TempCount);
            WriteLine(2, toBoolRigth.EmitCode());
            ++TempCount;
            ConditionalBranch b2 = new ConditionalBranch(LLVMModule)
            {
                Condition = toBoolRigth.Result,
                IfTrue = labelSetTrue.Name,
                IfFalse = labelSetFalse.Name
            };
            WriteLine(2, b2.EmitCode());

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