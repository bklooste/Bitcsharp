using System;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstForLoop astForLoop)
        {

            ////todo : forloop cgen
            //Label labelBody = new Label(LLVMModule) { Name = "for_" + LoopCount };
            //Label labelCond = new Label(LLVMModule) { Name = "forcond_" + LoopCount };
            //Label labelEnd = new Label(LLVMModule) { Name = "forend_" + LoopCount };
            //++LoopCount;

            //if (astForLoop.Initializers != null)
            //{
            //    foreach (AstStatement statement in astForLoop.Initializers)
            //    {
            //        statement.EmitCode(this);
            //    }
            //    ++TempCount;
            //}

            //UnConditionalBranch ub = new UnConditionalBranch(LLVMModule) { Destination = labelCond.Name };
            //WriteLine(2, ub.EmitCode());

            //WriteLine(1, labelBody.EmitCode());

            //astForLoop.Body.EmitCode(this);
            //--TempCount;

            //foreach (IAstExpression incrementExpression in astForLoop.IncrementExpressions)
            //    incrementExpression.EmitCode(this);


            //#region condtion

            //WriteLine(2, ub.EmitCode());
            //WriteLine(1, labelCond.EmitCode());

            //astForLoop.Condition.EmitCode(this);

            //ICmp toBool = ICmp.ToBool(LLVMModule, "%" + (TempCount + 1), "%" + TempCount);
            //WriteLine(2, toBool.EmitCode());
            //ConditionalBranch cb = new ConditionalBranch(LLVMModule)
            //                         {
            //                             Condition = toBool.Result,
            //                             IfTrue = labelBody.Name,
            //                             IfFalse = labelEnd.Name
            //                         };
            //WriteLine(2, cb.EmitCode());

            //#endregion

            //#region End
            //WriteLine(1, labelEnd.EmitCode());
            //#endregion
        }
    }
};