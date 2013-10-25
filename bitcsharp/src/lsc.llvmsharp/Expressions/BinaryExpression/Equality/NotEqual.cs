using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstInequalityExpression astInequalityExpression)
        {
            // same as equality code but just choose condition as NotEqual
            // for string its the same but at at end we do NOT ! of @__llvmsharp_String_compare(strHdr1,strHdr2)
            switch (astInequalityExpression.LeftOperand.AssociatedType)
            {
                case "System.Int32":
                    EmitCodeForInt32(astInequalityExpression);
                    break;
                case "System.Boolean":
                    EmitCodeForBool(astInequalityExpression);
                    break;
                case "System.String":
                    EmitCodeForString(astInequalityExpression);
                    break;
                case"System.Single":
                    EmitCodeForFloat(astInequalityExpression);
                    break;
            }
        }

        private void EmitCodeForFloat(AstInequalityExpression astInequalityExpression)
        {
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "neqSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "neqSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "neqEnd_" + LoopCount };
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

            astInequalityExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astInequalityExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            FCmp cmp = new FCmp(LLVMModule)
            {
                Type = "float",
                Condition = FCondition.UnorderedNotEqual,
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

        private void EmitCodeForString(AstInequalityExpression astInequalityExpression)
        {
            #region Declarations
            Label labelEnd = new Label(LLVMModule) { Name = "neqEnd_" + LoopCount };
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

            astInequalityExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astInequalityExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            Call c = new Call(LLVMModule, 2)
            {
                FunctionName = "@__llvmsharp_String_compare",
                ReturnType = "i8",
                Result = "%" + TempCount,
                IsSignExt = true
            };
            c.Arguments[0] = "%struct.__llvmsharp_stringHeader* %" + leftResult;
            c.Arguments[1] = "%struct.__llvmsharp_stringHeader* %" + rightResult;

            WriteLine(2, c.EmitCode());

            ICmp toBool = ICmp.ToBool(LLVMModule, "%" + (TempCount + 1), "%" + TempCount);
            WriteLine(2, toBool.EmitCode());
            ++TempCount;

            Xor toBoolNot = new Xor(LLVMModule)
            {
                Type = "i1",
                Operand1 = toBool.Result,
                Operand2 = "true",
                Result = "%" + (TempCount + 1)
            };
            WriteLine(2, toBoolNot.EmitCode());
            ++TempCount;

            Zext toBoolNotToInt = new Zext(LLVMModule)
            {
                Result = "%" + (TempCount + 1),
                Type = "i1",
                Type2 = "i8",
                Value = toBoolNot.Result
            };
            WriteLine(2, toBoolNotToInt.EmitCode());
            ++TempCount;

            Store s = new Store(LLVMModule)
            {
                Type = "i8",
                Value = "%" + TempCount,
                Pointer = a.Result
            };
            WriteLine(2, s.EmitCode());
            ++TempCount;

            Load l = new Load(LLVMModule)
            {
                Type = "i8",
                Pointer = a.Result,
                Result = "%" + TempCount
            };
            WriteLine(2, l.EmitCode());
        }

        private void EmitCodeForBool(AstInequalityExpression astInequalityExpression)
        {
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "neqSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "neqSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "neqEnd_" + LoopCount };
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

            astInequalityExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astInequalityExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            ICmp cmp = new ICmp(LLVMModule)
            {
                Type = "i8",
                Condition = Condition.NotEqual,
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

        private void EmitCodeForInt32(AstInequalityExpression astInequalityExpression)
        {
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "neqSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "neqSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "neqEnd_" + LoopCount };
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

            astInequalityExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astInequalityExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            ICmp cmp = new ICmp(LLVMModule)
                           {
                               Type = "i32",
                               Condition = Condition.NotEqual,
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