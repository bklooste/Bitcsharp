using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using LLVMSharp.Compiler;

namespace lsc.Tests
{
    public partial class Helpers
    {
        public static string GetPathRelativeToExecutable(string fileName)
        {
            string executable = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(executable), fileName));
        }

        static string _dataPath;
        public static string DataPath
        {
            get { return _dataPath ?? (_dataPath = ConfigurationManager.AppSettings["dataPath"]); }
        }

        static string _llvmPath;
        public static string LLVMPath
        {
            get { return _llvmPath ?? (_llvmPath = ConfigurationManager.AppSettings["llvmPath"]); }
        }

        static string _llvmAssemblerPath;
        public static string LLVMAssemblerPath
        {
            get
            {
                if (string.IsNullOrEmpty(_llvmAssemblerPath))
                    _llvmAssemblerPath = Path.Combine(LLVMPath, "llvm-as.exe");
                return _llvmAssemblerPath;
            }
        }

        private static string _lscPath;
        public static string LSCPath
        {
            get
            {
                if (string.IsNullOrEmpty(_lscPath))
                    _lscPath = Path.Combine(ConfigurationManager.AppSettings["lscPath"], "lsc.exe");
                return _lscPath;
            }
        }


        public static int ParseFileForParseError(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Parser p = new Parser(new Scanner(fs));
                p.errors.errorList.ErrorAdded += ErrorListErrorAdded;
                p.Parse();

                return p.errors.count;
            }
        }

        public static int ObjectHierarchyErrorCount(string[] path)
        {
            LLVMSharpCompiler compiler = new LLVMSharpCompiler(path);
            compiler.Errors.ErrorAdded += ErrorListErrorAdded;

            while (compiler.CanGoToNextStep && compiler.CompilerPhase != CompilerPhases.GeneratingObjectHierarchyCompleted)
                compiler.StartNextStep();

            return compiler.Errors.Count;
        }


        public static void ErrorListErrorAdded(object o, ErrorArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("File: ");
            sb.Append(e.errorInfo.fileName);
            sb.AppendFormat("l({0}) c({1}) ", e.errorInfo.line, e.errorInfo.col);
            sb.Append(e.errorInfo.message);
            Console.WriteLine(sb.ToString());
        }
    }
}
