using System.Collections.Generic;
using LLVMSharp.Compiler;
using Xunit;
using Xunit.Extensions;

namespace lsc.Tests.ManglerTests
{
    public class ExtractNumberTests
    {
        static readonly FakeExtractNumber M;

        static ExtractNumberTests()
        {
            M = new FakeExtractNumber();
        }

        public static IEnumerable<object[]> MangledData
        {
            get
            {
                yield return new object[] { 6, M.ExtractNumberFromParent("6Prabir.8Shrestha") };
                yield return new object[] { 17, M.ExtractNumberFromParent("17LLVMSharpCompiler10MangleName") };
            }
        }

        [Theory]
        [PropertyData("MangledData")]
        public void MangleNames(int expected, int actual)
        {
            Assert.Equal(expected, actual);
        }

        class FakeExtractNumber : Mangler
        {
            public int ExtractNumberFromParent(string str)
            {
                return base.ExtractNumber(str);
            }
        }
    }
}
