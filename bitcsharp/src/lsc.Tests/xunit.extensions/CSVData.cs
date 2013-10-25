using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Extensions;

namespace lsc.Tests
{
    public class CSVDataAttribute : DataAttribute
    {
        readonly string _fileName;
        readonly bool _hasHeaders;

        public CSVDataAttribute(string fileName, bool hasHeaders)
        {
            _hasHeaders = hasHeaders;

            _fileName = Helpers.GetPathRelativeToExecutable(fileName);
        }

        public CSVDataAttribute(string fileName)
            : this(fileName, true)
        {

        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            FileInfo[] badFiles = Helpers.ReadCsvAsFileInfos(_fileName, _hasHeaders);

            return badFiles.Select(item => new object[] { item.Path, item.Description });
        }
    }
}
