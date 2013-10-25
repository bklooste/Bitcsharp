using System.IO;

namespace LLVMSharp.Compiler.Assembler
{
    /// <summary>
    /// Mapper to llvm-as - LLVM assembler
    /// </summary>
    /// <remarks>
    ///     http://llvm.org/cmds/llvm-as.html
    /// NAME:
    ///     llvm-as - LLVM assembler
    /// SYNOPSIS:
    ///     llvm-as [options] [filename]
    /// DESCRIPTION:
    ///     llvm-as is the LLVM assembler. It reads a file containing human-readable LLVM assembly language, translates it to LLVM bitcode, and writes the result into a file or to standard output.
    ///     If filename is omitted or is -, then llvm-as reads its input from standard input.
    ///     If an output file is not specified with the -o option, then llvm-as sends its output to a file or standard output by following these rules:
    ///     • If the input is standard input, then the output is standard output. 
    ///     • If the input is a file that ends with .ll, then the output file is of the same name, except that the suffix is changed to .bc. 
    ///     • If the input is a file that does not end with the .ll suffix, then the output file has the same name as the input file, except that the .bc suffix is appended. 
    /// OPTIONS:
    ///     -f
    ///         Enable binary output on terminals. Normally, llvm-as will refuse to write raw bitcode output if the output stream is a terminal. With this option, llvm-as will write raw bitcode regardless of the output device. 
    ///     --help
    ///         Print a summary of command line options. 
    ///     -o filename
    ///         Specify the output file name. If filename is -, then llvm-as sends its output to standard output. 
    /// EXIT STATUS:
    ///     If llvm-as succeeds, it will exit with 0. Otherwise, if an error occurs, it will exit with a non-zero value
    /// </remarks>
    public class LLVMAssembler : AssemblerBase
    {
        private readonly string _llvmAssemblerPath;

        public LLVMAssembler()
            : this("llvm-as.exe", "")
        {
        }

        public LLVMAssembler(string args)
            : this("llvm-as.exe", args)
        {
        }

        public LLVMAssembler(string path, string args)
            : base(path, args)
        {
            _llvmAssemblerPath = path;

            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.ErrorDialog = false;
            Process.StartInfo.CreateNoWindow = true;

            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardOutput = true;
        }

        public override string AssemblerPath
        {
            get { return _llvmAssemblerPath; }
        }

        public override int Assemble(StandardInputDelegate input, StandardBinaryOutputDelegate output)
        {
            Process.Start();

            StreamWriter inputWriter = null;
            BinaryReader outputReader = null;

            if (Process.StartInfo.RedirectStandardInput) // use streamwrite to write synchoronously
                inputWriter = Process.StandardInput;

            if (Process.StartInfo.RedirectStandardOutput)
                outputReader = new BinaryReader(Process.StandardOutput.BaseStream);

            //// start async read of output stream
            //Process.BeginOutputReadLine();

            // start async read of error stream
            Process.BeginErrorReadLine();

            if (input != null)
            {
                input(this, inputWriter);

                // end the input stream
                inputWriter.Close();
            }

            if (output != null)
                output(this, outputReader);

            Process.WaitForExit();

            return Process.ExitCode;
        }
    }
}