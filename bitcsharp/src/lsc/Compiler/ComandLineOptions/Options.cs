using System;
using System.Collections;
using System.IO;
using System.Reflection;
using Plossum.CommandLine;

namespace LLVMSharp.Compiler
{
    public class Options
    {
        public Target Target;

        private ArrayList _coreLibFiles;
        private string[] _sourceFiles;

        public Options()
        {
            Target = Target.Exe;
            LinkCoreLib = true;
        }

        public Options(string[] args)
        {
            CmdOption options = new CmdOption();
            CommandLineParser parser = new CommandLineParser(options);
            parser.Parse(args, false);
            parser.UsageInfo.IndentWidth = 7;

            if (!options.Silent || options.Help || args.Length == 0)
            {
                Console.Write(parser.UsageInfo.GetHeaderAsString(78));
                Console.WriteLine("http://projects.prabir.me/compiler");
                Console.WriteLine();
            }

            if (options.Help || args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("\tlsc [Options] [OutputFormat] files");
                Console.WriteLine();
                Console.WriteLine(parser.UsageInfo.GetOptionsAsString(78));
                Environment.Exit(1);
            }
            else if (parser.HasErrors)
            {
                Console.WriteLine(parser.UsageInfo.GetErrorsAsString(78));
                Console.WriteLine("For more info try: ");
                Console.WriteLine("\tlsc -help");

                Console.WriteLine();
                Environment.ExitCode = 1;
                Environment.Exit(1);
            }

            Target = options.Target;
            IsSilent = options.Silent;
            IsHelp = options.Help;
            LinkCoreLib = true;
            SourceFiles = parser.RemainingArguments.ToArray();
            OutputFile = options.OutputFile ?? "a";
            EmitBitCode = options.EmitBitCode;
            EmitLLVMIR = options.EmitLLVMIR;
            EmitNativeObject = options.EmitNativeObject;
            EmitNativeAssembly = options.EmitNativeAssembly;
            EmitBitCodeWithCorLib = options.EmitBitCodeWithCorLib;
            EmitLLVMIRWithCorLib = options.EmitLLVMIRWithCorLib;
            EmitNativeObjectWithCorLib = options.EmitNativeObjectWithCorLib;
            EmitNativeAssemblyWithCorLib = options.EmitNativeAssemblyWithCorLib;
            EmitNativeExecutable = options.EmitNativeExecutable;
            EmitExecutable = options.EmitExecutable;
            EmitLLVMIRStdOut = options.EmitLLVMIRStdOut;

#if CMD_DEBUG
            Console.WriteLine("Outputfile: {0}", OutputFile ?? "[will be auto generated]");
            Console.WriteLine("Target: {0}", Target);
            Console.WriteLine("Emit LLVM Bit Code: {0}", EmitBitCode);
            Console.WriteLine("Emit LLVM IR: {0}", EmitLLVMIR);
            Console.WriteLine("Emit Native Object: {0}", EmitNativeObject);
            Console.WriteLine("Emit Native Assembly: {0}", EmitNativeAssembly);
            Console.WriteLine("Emit LLVM Bit Code with CorLib: {0}", EmitBitCodeWithCorLib);
            Console.WriteLine("Emit LLVM IR with CorLib: {0}", EmitLLVMIRWithCorLib);
            Console.WriteLine("Emit Native Object with CorLib: {0}", EmitNativeObjectWithCorLib);
            Console.WriteLine("Emit Native Assembly with CorLib: {0}", EmitNativeAssemblyWithCorLib);
            Console.WriteLine("SourceFiles: [corlib files included]");
            string corlibDir =
                Path.Combine(
                    Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "corlib");
            foreach (string i in SourceFiles)
            {
                if (!i.StartsWith(corlibDir))
                    Console.WriteLine("\t" + i);
            }
#endif
        }

        public bool EmitBitCode;
        public bool EmitLLVMIR;
        public bool EmitLLVMIRStdOut;
        public bool EmitNativeObject;
        public bool EmitNativeAssembly;
        public bool EmitBitCodeWithCorLib;
        public bool EmitLLVMIRWithCorLib;
        public bool EmitNativeObjectWithCorLib;
        public bool EmitNativeAssemblyWithCorLib;

        public bool EmitNativeExecutable;
        public bool EmitExecutable;

        public bool IsSilent;
        public bool IsHelp;

        public string OutputFile { get; private set; }

        public string[] SourceFiles
        {
            get { return _sourceFiles; }
            set
            {
                _sourceFiles = null;

                if (LinkCoreLib)
                {
                    _coreLibFiles = (SourceFiles == null) ? new ArrayList() : new ArrayList(SourceFiles);

                    //if (!Directory.Exists("corlib"))
                    string corlibDir =
                        Path.Combine(
                            Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "corlib");
                    if (!Directory.Exists(corlibDir))
                        throw new ApplicationException("corlib not found at " + corlibDir);

                    AddCoreLibFiles(corlibDir);
                }

                int vsize = 0;
                if (value != null) vsize = value.Length;
                int size = _coreLibFiles.Count + vsize;

                _sourceFiles = new string[size];

                int i, j;
                for (i = 0; i < _coreLibFiles.Count; i++)
                    _sourceFiles[i] = _coreLibFiles[i].ToString();

                for (j = 0; j < vsize; j++, i++)
                    _sourceFiles[i] = value[j];
            }
        }

        public bool LinkCoreLib { get; set; }

        private void AddCoreLibFiles(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
                _coreLibFiles.Add(file);

            string[] directories = Directory.GetDirectories(dir);
            foreach (string directory in directories)
                AddCoreLibFiles(directory);
        }
    }
}