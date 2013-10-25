using System.IO;

namespace LLVMSharp.Compiler.Linker
{
    public class LLVMLinker : LinkerBase
    {
        public LLVMLinker(string path, string args)
            : base(path, args)
        {

            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.ErrorDialog = false;
            Process.StartInfo.CreateNoWindow = true;

            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardOutput = true;
        }

        public LLVMLinker(string args)
            : this("llvm-link", args)
        {
        }

        public LLVMLinker()
            : this("llvm-link", string.Empty)
        {
        }

        public override int Link(StandardBinaryOutputDelegate output)
        {
            Process.Start();

            BinaryReader outputReader = null;

            if (InputFiles == null || InputFiles.Length == 0)
                throw new LLVMSharpException("No files to link.");

            if (Process.StartInfo.RedirectStandardOutput)
                outputReader = new BinaryReader(Process.StandardOutput.BaseStream);

            Process.BeginErrorReadLine();

            if (output != null)
                output(this, outputReader);

            Process.WaitForExit();

            return Process.ExitCode;
        }

        public string[] InputFiles
        {
            get
            {
                return Process.StartInfo.Arguments.Split(' ');
            }
        }
    }
}
