using System;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstDoLoop astDoLoop)
        {
            WriteInfoCommentLine(3, " do..loop");
            Label labelBody = new Label(LLVMModule) { Name = "loopStart_" + LoopCount };
            Label labelBodyEnd = new Label(LLVMModule) { Name = "loopEnd_" + LoopCount };
            ++LoopCount;

            UnConditionalBranch ub = new UnConditionalBranch(LLVMModule)
                                       {
                                           Destination = labelBody.Name
                                       };
            WriteLine(2, ub.EmitCode());

            #region Body

            WriteLine(1, labelBody.EmitCode());
            astDoLoop.AstStatement.EmitCode(this);
            WriteInfoCommentLine(3, "do..loop..condition");
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