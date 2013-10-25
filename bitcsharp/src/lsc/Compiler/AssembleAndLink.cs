using System.IO;
using LLVMSharp.Compiler.Assembler;
using LLVMSharp.Compiler.Linker;
using System;

namespace LLVMSharp.Compiler
{
    public partial class LLVMSharpCompiler
    {
        public void AssembleAndLink()
        {

            if (_canGoToNextStep)
            {
                if (Options.EmitLLVMIRStdOut)
                {
                    OutputLLVMIRToStdOut(Console.Out);
                }
                string llIr = string.Empty;
                if (Options.EmitLLVMIR)
                {
                    llIr = Options.OutputFile + ".ll";
                    OutputLLVMIR(llIr);
                }
                string bc = string.Empty;
                if (Options.EmitBitCode)
                {
                    bc = Options.OutputFile + ".bc";
                    if (!OutputLLVMBitCode(llIr, bc, false))
                    {
                        _canGoToNextStep = false;
                        //return;
                    }
                }
                string bcWithCorLib = string.Empty;
                if (Options.EmitBitCodeWithCorLib)
                {
                    bcWithCorLib = Options.OutputFile + "-corlib.bc";
                    OutputLLVMBitCodeWitCorLib(llIr, bc, bcWithCorLib, false);
                }

                string nativeExe = string.Empty;
                if (Options.EmitNativeExecutable)
                {
                    nativeExe = Options.OutputFile + ".exe";
                    OutputNativeExe(llIr, bc, bcWithCorLib, nativeExe, false);
                }

                string exe = string.Empty;
                if (Options.EmitExecutable)
                {
                    exe = Options.OutputFile + "-lli.exe";
                    OutputExe(llIr, bc, bcWithCorLib, exe, false);
                }

                if (!Options.EmitLLVMIR)
                    File.Delete("~" + Options.OutputFile + ".ll");
                if (!Options.EmitBitCode)
                    File.Delete("~" + Options.OutputFile + ".bc");
                if (!Options.EmitBitCodeWithCorLib)
                    File.Delete("~" + Options.OutputFile + "-corlib.bc");
            }
        }

        private void AssertCodeGenComplete()
        {
            if (CompilerPhase != CompilerPhases.GeneratingCodeCompleted)
                throw new LLVMSharpException("LLVM IR hasn't been generated yet");
        }

        public void OutputLLVMIR(string fileName)
        {
            AssertCodeGenComplete();

            StreamWriter writer = CodeGenerator.Writer;

            writer.Flush();
            writer.BaseStream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(writer.BaseStream);

            using (StreamWriter fsWriter = new StreamWriter(fileName))
            {
                fsWriter.Write(reader.ReadToEnd());
            }
        }

        private void OutputLLVMIRToStdOut(TextWriter textWriter)
        {
            AssertCodeGenComplete();

            StreamWriter writer = CodeGenerator.Writer;

            writer.Flush();
            writer.BaseStream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(writer.BaseStream);

            textWriter.Write(reader.ReadToEnd());
        }

        public bool OutputLLVMBitCode(string llFileName, string outputBCFileName, bool deleteTmp)
        {
            if (string.IsNullOrEmpty(llFileName))
            {
                llFileName = "~" + Options.OutputFile + ".ll";
                OutputLLVMIR(llFileName);
            }

            LLVMAssembler asm = new LLVMAssembler(WorkingDirectory + "\\llvm-as", llFileName + " -f -o=" + outputBCFileName);
            asm.Process.StartInfo.RedirectStandardInput = false;
            asm.Process.StartInfo.RedirectStandardOutput = false;
            asm.Process.StartInfo.RedirectStandardError = true;
            asm.Process.ErrorDataReceived += Process_ErrorDataReceived;

            int i = asm.Assemble(null, null);

            //if (i != 0)
            //{
            //    //using (StreamReader reader = new StreamReader(outputBCFileName))
            //    //{
            //    //    ErrorInfo err = new ErrorInfo { message = reader.ReadToEnd() };
            //    //    Errors.Add(err);
            //    //}
            //    //File.Delete(outputBCFileName);
            //}

            if (deleteTmp)
                File.Delete(llFileName);

            return i == 0;
        }

        public bool OutputLLVMBitCodeWitCorLib(string llIr, string bc, string outputBcWithCorLib, bool deleteTemp)
        {
            if (string.IsNullOrEmpty(bc))
            {
                bc = "~" + Options.OutputFile + ".bc";
                if (!OutputLLVMBitCode(llIr, "~" + Options.OutputFile + ".bc", deleteTemp))
                {
                    return false;
                }
            }

            LLVMLinker linker = new LLVMLinker(WorkingDirectory + "\\llvm-link", bc + " " + WorkingDirectory + "\\llvmsrtl.bc -f -o=" + outputBcWithCorLib);
            linker.Process.StartInfo.RedirectStandardInput = false;
            linker.Process.StartInfo.RedirectStandardOutput = false;
            linker.Process.StartInfo.RedirectStandardError = true;
            linker.Process.ErrorDataReceived += Process_ErrorDataReceived;

            int i = linker.Link(null);

            if (i != 0)
            {
                if (File.Exists(outputBcWithCorLib))
                    File.Delete(outputBcWithCorLib);
            }

            if (deleteTemp)
                File.Delete(bc);

            return i == 0;
        }

        public void OutputNativeExe(string llIr, string bc, string bcWithCorLib, string nativeExe, bool deleteTemp)
        {

            if (string.IsNullOrEmpty(bc))
            {
                bc = "~" + Options.OutputFile + ".bc";
                OutputLLVMBitCode(llIr, "~" + Options.OutputFile + ".bc", deleteTemp);
                bcWithCorLib = bc;
            }

            LLVMLd ld = new LLVMLd(WorkingDirectory + "\\llvm-ld.exe", bcWithCorLib + " " + WorkingDirectory + "\\llvmsrtl.bc -native -o=" + nativeExe);

            if (!File.Exists(ld.LinkerPath))
            {
                Console.Error.WriteLine("llvm-ld doesn't exist");
            }
            //LLVMLd ld = new LLVMLd(WorkingDirectory + "\\llvm-ld", bcWithCorLib + " " + WorkingDirectory + "\\llvmsrtl.bc -native -o=" + nativeExe);
            ld.Process.StartInfo.RedirectStandardInput = false;
            ld.Process.StartInfo.RedirectStandardOutput = false;
            ld.Process.StartInfo.RedirectStandardError = true;
            ld.Process.ErrorDataReceived += Process_ErrorDataReceived;

            int i = ld.Link(null);

            if (i != 0)
            {
                if (File.Exists(nativeExe))
                    File.Delete(nativeExe);
            }

            if (deleteTemp)
                File.Delete(bcWithCorLib);
            File.Delete(nativeExe + ".bc");
        }

        public void OutputExe(string llIr, string bc, string bcWithCorLib, string nativeExe, bool deleteTemp)
        {


            if (string.IsNullOrEmpty(bc))
            {
                bc = "~" + Options.OutputFile + ".bc";
                OutputLLVMBitCode(llIr, "~" + Options.OutputFile + ".bc", deleteTemp);
                bcWithCorLib = bc;
            }

            LLVMLd ld = new LLVMLd(bcWithCorLib + " " + WorkingDirectory + "\\llvmsrtl.bc -o=" + nativeExe);
            ld.Process.StartInfo.RedirectStandardInput = false;
            ld.Process.StartInfo.RedirectStandardOutput = false;
            ld.Process.StartInfo.RedirectStandardError = true;
            ld.Process.ErrorDataReceived += Process_ErrorDataReceived;

            int i = ld.Link(null);

            if (i != 0)
            {
                if (File.Exists(nativeExe))
                    File.Delete(nativeExe);
            }

            if (deleteTemp)
                File.Delete(bcWithCorLib);
        }

        void Process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data))
                return;

            ErrorInfo err = new ErrorInfo { message = e.Data };
            Errors.Add(err);
        }
    }
}
