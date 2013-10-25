using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstIfCondition astIfCondition)
        {
            WriteInfoCommentLine(3, "IF");
            astIfCondition.Condition.EmitCode(this);
            ICmp toBool = ICmp.ToBool(LLVMModule, "%" + (TempCount + 1), "%" + TempCount);
            WriteLine(2, toBool.EmitCode());

            ++TempCount;

            Label labelTrue = new Label(LLVMModule) { Name = "if_" + NonLoopCount };
            Label labelFalse = new Label(LLVMModule) { Name = "else_" + NonLoopCount };
            Label labelEnd = new Label(LLVMModule) { Name = "ifend_" + NonLoopCount };
            ++NonLoopCount; //need to put up coz others may use the IfTempCount

            ConditionalBranch b = new ConditionalBranch(LLVMModule)
                                      {
                                          Condition = toBool.Result,
                                          IfTrue = labelTrue.Name,
                                          IfFalse = astIfCondition.AstStatementElse != null ? labelFalse.Name : labelEnd.Name
                                      };

            WriteLine(2, b.EmitCode());
            ++TempCount;

            #region IF
            WriteLine(1, labelTrue.EmitCode());
            astIfCondition.AstStatement.EmitCode(this);
            UnConditionalBranch ub = new UnConditionalBranch(LLVMModule)
                                       {
                                           Destination = labelEnd.Name
                                       };
            WriteLine(2, ub.EmitCode());
            #endregion

            #region ELSE

            if (astIfCondition.AstStatementElse != null)
            {
                WriteLine(1, labelFalse.EmitCode());
                astIfCondition.AstStatementElse.EmitCode(this);
                WriteLine(2, ub.EmitCode());
            }

            #endregion

            #region ENDIF

            WriteLine(1, labelEnd.EmitCode());
            #endregion
        }
    }
}