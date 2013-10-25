using System.IO;
using LLVMSharp.Compiler;
using Xunit;
using Xunit.Extensions;

namespace lsc.Tests.semantic
{
    public class SymbolTableTests
    {
        private readonly string _path;
        private readonly string _badPath;
        private string _goodPath;

        public SymbolTableTests()
        {
            _path = Helpers.GetPathRelativeToExecutable(Path.Combine(Helpers.DataPath, "semantic" + Path.DirectorySeparatorChar + "symbolTable"));
            _badPath = Path.Combine(_path, "bad" + Path.DirectorySeparatorChar);
            _goodPath = Path.Combine(_path, "good" + Path.DirectorySeparatorChar);
        }

        private static LLVMSharpCompiler CompileTillSymbolTable(string[] path)
        {
            LLVMSharpCompiler compiler = new LLVMSharpCompiler(path);
            compiler.Errors.ErrorAdded += Helpers.ErrorListErrorAdded;

            while (compiler.CanGoToNextStep && compiler.CompilerPhase != CompilerPhases.TypesCheckedCompleted)
                compiler.StartNextStep();

            return compiler;
        }

        [Theory]
        [CSVData("data\\semantic\\symbolTable\\SymbolTableErrorsCountTestBad.csv")]
        public void BadErrorCountTest(string path, string description)
        {
            LLVMSharpCompiler compiler = CompileTillSymbolTable(Directory.GetFiles(_badPath + path));
            Assert.InRange(compiler.Errors.Count, 1, int.MaxValue);
        }
    }
}
