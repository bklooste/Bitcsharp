using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstEqualityExpression astEqualityExpression)
        {
            switch (astEqualityExpression.LeftOperand.AssociatedType)
            {
                case "System.Int32":
                    EmitCodeForInt32(astEqualityExpression);
                    break;
                case "System.Boolean":
                    EmitCodeForBool(astEqualityExpression);
                    break;
                case "System.String":
                    EmitCodeForString(astEqualityExpression);
                    break;
                case "System.Single":
                    EmitCodeForFloat(astEqualityExpression);
                    break;
            }
        }

        private void EmitCodeForFloat(AstEqualityExpression astEqualityExpression)
        {
            //throw new System.NotImplementedException();
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "eqSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "eqSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "eqEnd_" + LoopCount };
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

            astEqualityExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astEqualityExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            FCmp cmp = new FCmp(LLVMModule)
                           {
                               Type = "float",
                               Condition = FCondition.OrderedEqual,
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
        

        private void EmitCodeForString(AstEqualityExpression astEqualityExpression)
        {
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "eqSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "eqSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "eqEnd_" + LoopCount };
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

            astEqualityExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;
            WriteCommentLine("left end");

            astEqualityExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;
            WriteCommentLine("right end");

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

            Store s = new Store(LLVMModule)
                        {
                            Type = "i8",
                            Value = "%" + TempCount,
                            Pointer = a.Result
                        };
            WriteLine(2,s.EmitCode());
            ++TempCount;

            Load l = new Load(LLVMModule)
            {
                Type = "i8",
                Pointer = a.Result,
                Result = "%" + TempCount
            };
            WriteLine(2, l.EmitCode());
        }

        private void EmitCodeForBool(AstEqualityExpression astEqualityExpression)
        {
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "eqSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "eqSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "eqEnd_" + LoopCount };
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

            astEqualityExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astEqualityExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            ICmp cmp = new ICmp(LLVMModule)
            {
                Type = "i8",
                Condition = Condition.Equal,
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

        private void EmitCodeForInt32(AstEqualityExpression astEqualityExpression)
        {
            #region Declarations
            Label labelSetFalse = new Label(LLVMModule) { Name = "eqSetFalse_" + LoopCount };
            Label labelSetTrue = new Label(LLVMModule) { Name = "eqSetTrue_" + LoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "eqEnd_" + LoopCount };
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

            astEqualityExpression.LeftOperand.EmitCode(this);
            int leftResult = TempCount++;

            astEqualityExpression.RightOperand.EmitCode(this);
            int rightResult = TempCount++;

            ICmp cmp = new ICmp(LLVMModule)
                           {
                               Type = "i32",
                               Condition = Condition.Equal,
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