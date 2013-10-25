using System.Diagnostics;
using System.IO;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator : CodeGenerator
    {
        public readonly string PREFIX = "__LS";
        public LLVM.Module LLVMModule;


        public LLVMSharpCodeGenerator(StreamWriter writer)
            : base(writer)
        {
            LLVMModule = new LLVM.Module(writer);
            ++LLVMModule.TempCount;
        }

        protected int TempCount
        {
            get { return LLVMModule.TempCount; }
            set { LLVMModule.TempCount = value; }
        }

        protected int LoopCount;
        protected int NonLoopCount;

        [DebuggerStepThrough]
        public override void WriteComment(string comment)
        {
            Write(";");
            Write(comment);
        }
    }
}