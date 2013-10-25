using System.Collections.Generic;
using System.IO;
using LLVMSharp.Compiler;
using LLVMSharp.Compiler.Ast;
using Xunit;
using Xunit.Extensions;

namespace lsc.Tests.Compiler.Semantic.ObjectLayout
{
    public class ObjectLayoutTests
    {
        static readonly LLVMSharpCompiler _compiler;

        static readonly AstClass _classA, _classB, _classC, _classD;

        static ObjectLayoutTests()
        {
            string dir = Helpers.GetPathRelativeToExecutable(Path.Combine(Helpers.DataPath, "semantic" + Path.DirectorySeparatorChar + "objectheirarchy"));

            _compiler = new LLVMSharpCompiler(new string[] { Path.Combine(dir, "objectlayout_lookup.cs") });
            _compiler.Errors.ErrorAdded += new ErrorHandler(Helpers.ErrorListErrorAdded);

            while (_compiler.CanGoToNextStep && _compiler.CompilerPhase != CompilerPhases.GeneratingObjectHierarchyCompleted)
                _compiler.StartNextStep();

            // caching to speedup lookups
            _classA = (AstClass)_compiler.ClassHashtable["A"];
            _classB = (AstClass)_compiler.ClassHashtable["B"];
            _classC = (AstClass)_compiler.ClassHashtable["C"];
            _classD = (AstClass)_compiler.ClassHashtable["D"];
        }

        [Theory]
        [InlineData("A", 4)]
        public void ObjectLayoutCount(string className, int objectLayoutCount)
        {
            Assert.Equal(objectLayoutCount, _classA.ObjectLayout.Count);
        }

        [Theory]
        [InlineData(true, "a", false)]
        [InlineData(true, "b", false)]
        [InlineData(true, "c", false)]
        [InlineData(true, "d", false)]
        [InlineData(false, "a", true)]
        [InlineData(false, "b", true)]
        [InlineData(false, "c", true)]
        [InlineData(false, "d", true)]
        [InlineData(false, "x", false)]
        [InlineData(false, "u", true)]
        public void ObjectLayoutNameTest(bool expected, string fieldName, bool isInheritedFromBase)
        {
            Assert.Equal(expected,
                _classA.ObjectLayout.ContainsKey(Mangler.Instance.MangleField(fieldName, isInheritedFromBase)));
        }

        [Fact]
        public void ObjectLayoutChildInteritPublicFieldTest()
        {
            Assert.True(_classB.ObjectLayout.ContainsKey(Mangler.Instance.MangleField("d", true)));
        }

        [Fact]
        public void ObjectLayoutChildInheritProtectedFieldTest()
        {
            Assert.True(_classB.ObjectLayout.ContainsKey(Mangler.Instance.MangleField("c", true)));
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        public void ObjectLayoutChildInheritPrivateFieldTest(string fieldName)
        {
            Assert.False(_classB.ObjectLayout.ContainsKey(Mangler.Instance.MangleField(fieldName, true)));
            Assert.False(_classB.ObjectLayout.ContainsKey(Mangler.Instance.MangleField(fieldName, true)));
        }

        [Theory]
        [PropertyData("ObjectLayoutChildCount_Data")]
        public void ObjectLayoutChildCount(int expected, int actual)
        {
            Assert.Equal(2, _classB.ObjectLayout.Count);
        }

        public static IEnumerable<object[]> ObjectLayoutChildCount_Data
        {
            get
            {
                yield return new object[] { 2, _classB.ObjectLayout.Count };
                yield return new object[] { 4, _classC.ObjectLayout.Count };
            }
        }

        [Theory]
        [PropertyData("LookupObjectLayout_NotFound_TestData")]
        public void LookupObjectLayoutNotFoundTests(object lookedUpObject)
        {
            Assert.Null(lookedUpObject);
        }

        public static IEnumerable<object[]> LookupObjectLayout_NotFound_TestData
        {
            get
            {
                yield return new object[] { _classA.LookupObjectLayout("x") };
                yield return new object[] { _classA.LookupObjectLayout("z") };
            }
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        [InlineData("d")]
        public void LookupObjectLayout_inClassA_FoundIsInheritedFromParent(string fieldName)
        {
            Assert.NotNull(_classA.LookupObjectLayout(fieldName, false));
        }
        [Theory]
        [InlineData("c")]
        [InlineData("d")]
        public void LookupObjectLayout_inClassB_FoundIsInheritedFromParent(string fieldName)
        {
            Assert.NotNull(_classB.LookupObjectLayout(fieldName, true));
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        [InlineData("d")]
        public void LookupObjectLayoutNotFound_inClassA_IsInheritedFromParentTests(string fieldName)
        {
            Assert.Null(_classA.LookupObjectLayout(fieldName, true));
        }

        [Theory]
        [InlineData("c")]
        [InlineData("d")]
        public void LookupObjectLayoutNotFound_inClassB_IsInheritedFromParentTests(string fieldName)
        {
            Assert.Null(_classB.LookupObjectLayout(fieldName, false));
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        [InlineData("d")]
        public void LookeupObjectLayout_inClassA_FoundTests(string fieldName)
        {
            Assert.NotNull(_classA.LookupObjectLayout(fieldName));
        }

        [Theory]
        [InlineData("c")]
        [InlineData("d")]
        public void LookeupObjectLayout_inClassB_FoundTests(string fieldName)
        {
            Assert.NotNull(_classB.LookupObjectLayout(fieldName));
        }


        [Theory]
        [PropertyData("OutIsInheritedFromParent_True_Data")]
        public void OutIsInheritedFromParent_True(AstClass astClass, string fieldName)
        {
            bool tmp;
            Assert.NotNull(astClass.LookupObjectLayout(fieldName, out tmp));
            Assert.True(tmp);
        }

        public static IEnumerable<object[]> OutIsInheritedFromParent_True_Data
        {
            get
            {
                yield return new object[] { _classB, "c" };
                yield return new object[] { _classB, "d" };

                yield return new object[] { _classC, "c" };
                yield return new object[] { _classD, "d" };
            }
        }

        [Theory]
        [PropertyData("OutIsInheritedFromParent_False_Data")]
        public void OutIsInheritedFromParent_False(AstClass astClass, string fieldName)
        {
            bool tmp;
            Assert.NotNull(astClass.LookupObjectLayout(fieldName, out tmp));
            Assert.False(tmp);
        }

        public static IEnumerable<object[]> OutIsInheritedFromParent_False_Data
        {
            get
            {
                yield return new object[] { _classC, "e" };
                yield return new object[] { _classC, "f" };
            }
        }

        [Fact]
        public void SecondLevelInheritence_Count()
        {
            Assert.Equal(6, _classD.ObjectLayout.Count);
        }

        [Theory]
        [InlineData("c")]
        [InlineData("d")]
        [InlineData("e")]
        [InlineData("f")]
        public void SecondLevelInheritence_ParentField(string fieldName)
        {
            Assert.NotNull(_classD.LookupObjectLayout(fieldName));
        }

        [Theory]
        [InlineData("g")]
        [InlineData("h")]
        public void SecondLevelInheritence_OutIsThis(string fieldName)
        {
            bool tmp;
            Assert.NotNull(_classD.LookupObjectLayout(fieldName, out tmp));
            Assert.False(tmp);
        }

        [Theory]
        [InlineData("c")]
        [InlineData("d")]
        [InlineData("e")]
        [InlineData("f")]
        public void SecondLevelInheritence_OutIsBase(string fieldName)
        {
            bool tmp;
            Assert.NotNull(_classD.LookupObjectLayout(fieldName, out tmp));
            Assert.True(tmp);
        }
    }
}
