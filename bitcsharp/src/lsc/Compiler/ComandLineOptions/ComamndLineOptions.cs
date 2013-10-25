using Plossum.CommandLine;

namespace LLVMSharp.Compiler
{
    [CommandLineManager(ApplicationName = "LLVM# Compiler", Copyright = "Copyright (c) Prabir, Morris, Vicky", Version = "0.1")]
    [CommandLineOptionGroup("target", Name = "Target")]
    [CommandLineOptionGroup("output", Name = "OutputFormat")]
    public class CmdOption
    {
        [CommandLineOption(Name = "help", Description = "Displays this help text", Aliases = "h")]
        public bool Help = false;

        [CommandLineOption(Name = "silent", Description = "Don't display compiler info", Aliases = "s")]
        public bool Silent = false;

        [CommandLineOption(Name = "target", Description = "Target File: Either exe,winexe or library", Aliases = "t",
            DefaultAssignmentValue = Target.Exe, RequireExplicitAssignment = true)]
        public Target Target { get; set; }

        [CommandLineOption(Name = "out", Description = "Specify the output FileName", Aliases = "o")]
        public string OutputFile { get; set; }

        [CommandLineOption(Name = "output-bc", Description = "Emit LLVM Bit Code", GroupId = "output")]
        public bool EmitBitCode;

        [CommandLineOption(Name = "output-ll", Description = "Emit LLVM IR", GroupId = "output")]
        public bool EmitLLVMIR;

        [CommandLineOption(Name = "output-ll-out", Description = "Emit LLVM IR to standard output", GroupId = "output")]
        public bool EmitLLVMIRStdOut;

        [CommandLineOption(Name = "output-o", Description = "Emit native object", GroupId = "output")]
        public bool EmitNativeObject;

        [CommandLineOption(Name = "output-s", Description = "Emit native assembly", GroupId = "output")]
        public bool EmitNativeAssembly;

        [CommandLineOption(Name = "output-bc-corlib", Description = "Emit LLVM Bit Code with CorLib", GroupId = "output")]
        public bool EmitBitCodeWithCorLib;

        [CommandLineOption(Name = "output-ll-corlib", Description = "Emit LLVM IR with CorLib", GroupId = "output")]
        public bool EmitLLVMIRWithCorLib;

        [CommandLineOption(Name = "output-o-corlib", Description = "Emit native object with CorLib", GroupId = "output")]
        public bool EmitNativeObjectWithCorLib;

        [CommandLineOption(Name = "output-s-corlib", Description = "Emit native assembly with CorLib", GroupId = "output")]
        public bool EmitNativeAssemblyWithCorLib;

        [CommandLineOption(Name = "output-bin", Description = "Emit shell script", GroupId = "output")]
        public bool EmitExecutable;

        [CommandLineOption(Name = "output-bin-native", Description = "Emit native binary", GroupId = "output")]
        public bool EmitNativeExecutable;
    }
}
