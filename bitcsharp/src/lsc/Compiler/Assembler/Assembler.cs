using System.Diagnostics;

namespace LLVMSharp.Compiler.Assembler
{
    public abstract class AssemblerBase
    {
        private readonly string _assemblerPath;

        public AssemblerBase()
            : this("llvm-as", "")
        {
        }

        public AssemblerBase(string args)
            : this("llvm-as", args)
        {
        }

        public AssemblerBase(string path, string args)
        {
            _assemblerPath = path;

            Process = new Process()
                          {
                              StartInfo = new ProcessStartInfo(path)
                          };

            Process.StartInfo.Arguments = args;
        }

        public Process Process { get; protected set; }

        public string Arguments
        {
            get { return Process.StartInfo.Arguments; }
            set { Process.StartInfo.Arguments = value; }
        }

        public abstract string AssemblerPath { get; }

        public abstract int Assemble(StandardInputDelegate input, StandardBinaryOutputDelegate output);
    }
}