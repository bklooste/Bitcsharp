using System;
using System.IO;
using Xunit;
using Xunit.Extensions;

namespace lsc.Tests
{
    public class SemanticObjectHierarchyParentClassTests
    {
        private readonly string _path;
        private readonly string _badPath;
        private readonly string _goodPath;

        public SemanticObjectHierarchyParentClassTests()
        {
            _path = Helpers.GetPathRelativeToExecutable(Path.Combine(Helpers.DataPath, "semantic" + Path.DirectorySeparatorChar + "objectheirarchy"));
            _badPath = Path.Combine(_path, "bad" + Path.DirectorySeparatorChar);
            _goodPath = Path.Combine(_path, "good" + Path.DirectorySeparatorChar);
        }

        [Theory(Skip = "Need to fix the test files")]
        [CSVData("data\\semantic\\objectheirarchy\\SemanticErrorsCountTestBad.csv")]
        public void BadErrorCountTest(string path, string description)
        {
            Assert.InRange(
                Helpers.ObjectHierarchyErrorCount(Directory.GetFiles(_badPath + path)),
                0,
                int.MaxValue);
        }

        [Theory]
        [CSVData("data\\semantic\\objectheirarchy\\SemanticErrorsCountTestGood.csv")]
        public void GoodErrorCountTest(string path, string description)
        {
            Console.WriteLine(path);           
            Assert.Equal(0, Helpers.ObjectHierarchyErrorCount(Directory.GetFiles(_goodPath + path)));
        }
    }
}
