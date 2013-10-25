using System;
using System.IO;
using Xunit;
using Xunit.Extensions;

namespace lsc.Tests
{
    public class ParserTests
    {
        private readonly string _path;
        private readonly string _badPath;
        private readonly string _goodPath;
        private readonly FakeConsole _fakeConsole;

        public ParserTests()
        {
            _path = Helpers.GetPathRelativeToExecutable(Path.Combine(Helpers.DataPath, "parser"));
            _badPath = Path.Combine(_path, "bad" + Path.DirectorySeparatorChar);
            _goodPath = Path.Combine(_path, "good" + Path.DirectorySeparatorChar);
            _fakeConsole = new FakeConsole();
        }

        [Theory]
        [CSVData(@"data\parser\ParseErrorsCountTestBad.csv")]
        public void BadParserErrorCountTest(string path, string description)
        {
            Console.SetOut(_fakeConsole);
            string file = _badPath + path;
            Assert.NotEqual(0, Helpers.ParseFileForParseError(file));
        }

        [Theory]
        [CSVData(@"data\parser\ParseErrorsCountTestGood.csv")]
        public void GoodParserErrorCountTest(string path, string description)
        {
            Console.SetOut(_fakeConsole);
            string file = _goodPath + path;
            Assert.Equal(0, Helpers.ParseFileForParseError(file));
        }

        public class FakeConsole : TextWriter
        {

            public override System.Text.Encoding Encoding
            {
                get { return System.Text.Encoding.ASCII; }
            }
        }
    }
}
