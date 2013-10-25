using System.Diagnostics;

namespace LLVMSharp.Compiler.Linker
{
    public abstract class LinkerBase
    {
        public Process Process { get; protected set; }

        public LinkerBase(string path, string args)
        {
            Process = new Process()
            {
                StartInfo = new ProcessStartInfo(path, args)
            };
        }

        public LinkerBase(string args)
            : this("llvm-link", args)
        {

        }

        public LinkerBase()
            : this("llvm-link", string.Empty)
        {

        }

        public string LinkerPath { get { return Process.StartInfo.FileName; } }

        public string Arguments
        {
            get { return Process.StartInfo.Arguments; }
            set { Process.StartInfo.Arguments = value; }
        }

        public abstract int Link(StandardBinaryOutputDelegate output);
    }
}
