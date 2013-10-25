using System.Collections.Generic;
using LLVMSharp.Compiler;
using Xunit;
using Xunit.Extensions;

namespace lsc.Tests.ManglerTests
{
    public class ManglingTests
    {
        static readonly Mangler M;

        static ManglingTests()
        {
            M = new Mangler();
        }

        public static IEnumerable<object[]> EmptyAndNullString
        {
            get
            {
                yield return new object[] { string.Empty, M.MangleName("") };
                yield return new object[] { string.Empty, M.MangleName(string.Empty) };
                yield return new object[] { "", M.MangleName("") };
                yield return new object[] { "", M.MangleName(string.Empty) };
            }
        }

        public static IEnumerable<object[]> MangledNameWithoutDots
        {
            get
            {
                yield return new object[] { "5Hello", M.MangleName("Hello") };
            }
        }

        public static IEnumerable<object[]> MangledNameWithDots
        {
            get
            {
                yield return new object[] { "5Hello5World", M.MangleName("Hello.World") };
                yield return new object[] { "9LLVMSharp8Compiler4LLVM", M.MangleName("LLVMSharp.Compiler.LLVM") };
            }
        }

        [Theory]
        [PropertyData("EmptyAndNullString")]
        [PropertyData("MangledNameWithoutDots")]
        [PropertyData("MangledNameWithDots")]
        public void MangleNames(string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

        //[Test]
        //public void MangleNamespace_EnumWithNS()
        //{
        //    Assert.AreEqual(_m.PREFIX + "Ei32_6System12ConsoleColor3Red",
        //        _m.GenerateOutputNameForEnumMember("System.ConsoleColor", "Red"));
        //}
        //[Test]
        //public void MangleEnumWithoutNS()
        //{
        //    Assert.AreEqual("Ei32_5Color3Red", _m.MangleEnumMember("", "Color", "Red", ""));
        //    //Assert.AreEqual("", _m.MangleMethod("System", "Object",
        //    //    new LLVMSharp.Compiler.Ast.AstMethod("a.cs", 0, 0) { Name = "sd" }));
        //    //Assert.AreEqual(_m.PREFIX + "Ei32_3Red",
        //    //    _m.GenerateOutputNameForEnumMember(string.Empty, "Red"));
        //    //Assert.AreEqual(_m.PREFIX + "Ei32_3Red",
        //    //    _m.GenerateOutputNameForEnumMember(null, "Red"));
        //}
    }
}
