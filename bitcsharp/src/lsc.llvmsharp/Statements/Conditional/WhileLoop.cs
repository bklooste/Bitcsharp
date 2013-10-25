using System;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        /// <remarks>
        /// This is same as do..loop but jumps to condition label first.
        /// It also adds aditional conditional label unlinke do..loop.
        /// Rest are same.
        /// </remarks>
        public override void EmitCode(AstWhileLoop astDoLoop)
        {
            WriteInfoCommentLine(3, " do..loop");
            Label labelBody = new Label(LLVMModule) { Name = "loopStart_" + LoopCount };
            Label labelBodyEnd = new Label(LLVMModule) { Name = "loopEnd_" + LoopCount };
            Label labelCondtion = new Label(LLVMModule) { Name = "loopCond_" + LoopCount };
            ++LoopCount;

            UnConditionalBranch ub = new UnConditionalBranch(LLVMModule)
                                       {
                                           Destination = labelCondtion.Name
                                       };
            WriteLine(2, ub.EmitCode());

            #region Body

            WriteLine(1, labelBody.EmitCode());
            astDoLoop.AstStatement.EmitCode(this);
            WriteLine(2, ub.EmitCode());

            #endregion

            #region Condition

            WriteInfoCommentLine(3, "do..loop..condition");
            WriteLine(1, labelCondtion.EmitCode());

            astDoLoop.Condition.EmitCode(this);
            ICmp toBool = ICmp.ToBool(LLVMModule, "%" + (TempCount + 1), "%" + TempCount);
            ++TempCount;
            WriteLine(2, toBool.EmitCode());

            ConditionalBranch b = new ConditionalBranch(LLVMModule)
            {
                Condition = toBool.Result,
                IfTrue = labelBody.Name,
                IfFalse = labelBodyEnd.Name
            };
            WriteLine(2, b.EmitCode());
            ++TempCount;

            #endregion

            WriteLine(1, labelBodyEnd.EmitCode());
        }
    }
}