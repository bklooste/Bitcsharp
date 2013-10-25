using System;
using System.IO;
using System.Reflection;
using LLVMSharp.Compiler;
using LLVMSharp.Compiler.CodeGenerators;

namespace LLVMSharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.CancelKeyPress += (cancelkeypressSender, cancelkeypressE) =>
            {
                Console.WriteLine("Aborting...");
                Environment.Exit(1);
            };

            LLVMSharpCompiler compiler;
            try
            {
                compiler = new LLVMSharpCompiler(args);
                if (compiler.Options.IsHelp)
                    return;

                firstError = true;
                silent = compiler.Options.IsSilent;

                compiler.Errors.ErrorAdded += Errors_ErrorAdded;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(ms))
                    {
                        if (!File.Exists(compiler.WorkingDirectory + "\\lsc.llvmsharp.dll"))
                        {
                            Console.WriteLine("lsc.llvmsharp.dll not found which is required for code generator.");
                            Environment.Exit(2);
                        }
                        Assembly asm = Assembly.LoadFrom(compiler.WorkingDirectory + "\\lsc.llvmsharp.dll");
                        Type t = asm.GetType("LLVMSharp.Compiler.CodeGenerators.LLVMSharpCodeGenerator");
                        compiler.CodeGenerator = (CodeGenerator)Activator.CreateInstance(t, new object[] { writer });

                        while (compiler.CanGoToNextStep && compiler.CompilerPhase != CompilerPhases.GeneratingCodeCompleted)
                            compiler.StartNextStep();

                        if (compiler.CompilerPhase == CompilerPhases.GeneratingCodeCompleted)
                            compiler.AssembleAndLink();
                    }
                }
                if (compiler.Errors.Count > 0)
                    Environment.Exit(Environment.ExitCode != 0 ? Environment.ExitCode : 1);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Environment.Exit(Environment.ExitCode != 0 ? Environment.ExitCode : 1);
            }
        }

        private static bool firstError;
        private static bool silent;
        static void Errors_ErrorAdded(object o, ErrorArgs e)
        {
            if (firstError)
            {
                firstError = false;
                if (!silent)
                {
                    Console.Error.WriteLine("(Line,Col) ErrorMessage [FileName]");
                    Console.Error.WriteLine();
                }
            }
            if (string.IsNullOrEmpty(e.errorInfo.fileName))
                Console.Error.WriteLine(e.errorInfo.message);
            else
                Console.Error.WriteLine("({0},{1}) {2} [{3}]", e.errorInfo.line, e.errorInfo.col, e.errorInfo.message, e.errorInfo.fileName);
        }
    }
}