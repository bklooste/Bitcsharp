using System.Collections.Generic;
using LLVMSharp.Compiler;
using Xunit;
using Xunit.Extensions;

namespace lsc.Tests.ManglerTests
{
    public class DemanglingTests
    {
        static readonly Mangler M;

        static DemanglingTests()
        {
            M = new Mangler();
        }

        public static IEnumerable<object[]> MangledData
        {
            get
            {
                yield return new object[] { "Prabir.Shrestha", M.DemangleName("6Prabir8Shrestha") };
                yield return new object[] { "LLVMSharpCompiler.MangleName", M.DemangleName("17LLVMSharpCompiler10MangleName") };
            }
        }

        [Theory]
        [PropertyData("MangledData")]
        public void DemangleNames(string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

    }
}
