using System.Diagnostics;
using System.IO;

namespace LLVMSharp.Compiler
{
    public class LLVMIntepreter
    {
        public LLVMIntepreter(string path, string args)
        {
            Process = new Process
                          {
                              StartInfo = new ProcessStartInfo(path, args)
                          };

            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.ErrorDialog = false;
            Process.StartInfo.CreateNoWindow = true;


            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardOutput = true;
        }

        public LLVMIntepreter(string args)
            : this("lli", args)
        {
        }

        public LLVMIntepreter()
            : this("lli", string.Empty)
        {
        }

        public Process Process { get; protected set; }

        public string IntepreterPath
        {
            get { return Process.StartInfo.FileName; }
        }

        public string Arguments
        {
            get { return Process.StartInfo.Arguments; }
            set { Process.StartInfo.Arguments = value; }
        }

        public int Run(StandardBinaryInputDelegate input)
        {
            string intPath = IntepreterPath;

            Process.Start();

            //Process.Start("cmd", "/c" + Process.StartInfo.FileName + Arguments + " && pause");

            BinaryWriter inputWriter = null;

            if (Process.StartInfo.RedirectStandardInput) // use streamwrite to write synchoronously
                inputWriter = new BinaryWriter(Process.StandardInput.BaseStream);

            //// start async read of output stream
            //Process.BeginOutputReadLine();

            // start async read of error stream
            Process.BeginErrorReadLine();

            if (input != null)
                input(this, inputWriter);

            //end the input stream
            inputWriter.Close();

            Process.WaitForExit();

            return Process.ExitCode;
        }
    }
}