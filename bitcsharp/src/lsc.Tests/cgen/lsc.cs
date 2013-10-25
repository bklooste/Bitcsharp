using System.Diagnostics;
using System.IO;
using LLVMSharp.Compiler;

namespace lsc.Tests.cgen
{
    public class LscCommandLine
    {
        public Process Process { get; protected set; }

        public LscCommandLine(string path, string args)
        {
            Process = new Process()
            {
                StartInfo = new ProcessStartInfo(path, args)
            };

            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.ErrorDialog = false;
            Process.StartInfo.CreateNoWindow = true;
        }

        public LscCommandLine(string args)
            : this(@"lsc", args)
        {

        }

        public LscCommandLine()
            : this("lsc", string.Empty)
        {

        }

        public string LinkerPath { get { return Process.StartInfo.FileName; } }

        public string Arguments
        {
            get { return Process.StartInfo.Arguments; }
            set { Process.StartInfo.Arguments = value; }
        }

        public int Run(StandardOutputDelegate output)
        {
            Process.Start();

            StreamReader outputReader = null;

            if (Process.StartInfo.RedirectStandardOutput)
                outputReader = new StreamReader(Process.StandardOutput.BaseStream);

            if (Process.StartInfo.RedirectStandardError)
                Process.BeginErrorReadLine();

            if (output != null)
                output(this, outputReader);

            Process.WaitForExit();

            return Process.ExitCode;
        }

    }
}

