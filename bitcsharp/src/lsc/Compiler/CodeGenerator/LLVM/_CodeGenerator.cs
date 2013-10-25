/*
using System.IO;

namespace LLVMSharp.Compiler.LLVM
{
    public partial class LLVMCodeGenerator : CodeGenerator
    {
        public LLVM.Module LLVMModule = null;

        private int TempCount = 0;

        //private int TempCount
        //{
        //    get { return LLVMModule.TempCount; }
        //    set { LLVMModule.TempCount = value; }
        //}

        public LLVMCodeGenerator(StreamWriter writer)
            : base(writer)
        {
            LLVMModule = new Module(writer);
           /* WriteCommentLine(" output-format = 'llvm-2.5'");
            WriteLine();♥1♥
        }

        /*    #region Comments

            public override void WriteComment(string comment)
            {
                Writer.Write(";");
                Writer.Write(comment);
            }
            #endregion♥1♥

    }
}
*/
