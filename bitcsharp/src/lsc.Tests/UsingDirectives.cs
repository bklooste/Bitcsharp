using LLVMSharp.Compiler.UsingDirectives;
using Xunit;

namespace lsc.Tests
{
    public class UsingDirectivesTest
    {
        [Fact]
        public void UsingDirectives_Test()
        {
            UsingDirective ud = new UsingDirective();

            ud.Insert("System");
            ud.Insert("System.Collection");

            string[] list = ud.Namespaces;

            Assert.Equal(2, list.Length);

            Assert.Equal("System", list[0]);
            Assert.Equal("System.Collection", list[1]);
        }

        [Fact]
        public void UsingDirectivesOpenScope_Test()
        {
            UsingDirective ud = new UsingDirective();

            ud.Insert("System");
            ud.Insert("System.Collection");

            ud.OpenScope("Test");

            string[] list = ud.Namespaces;

            Assert.Equal(3, list.Length);

            Assert.Equal("Test", list[0]);
            Assert.Equal("System", list[1]);
            Assert.Equal("System.Collection", list[2]);
        }

        [Fact]
        public void UsingDirectivesOpenScopeNested_Test()
        {
            UsingDirective ud = new UsingDirective();

            ud.Insert("System");
            ud.Insert("System.Collection");

            ud.OpenScope("Test");

            ud.OpenScope("Nested");

            string[] list = ud.Namespaces;

            Assert.Equal(4, list.Length);

            Assert.Equal("Test.Nested", list[0]);
            Assert.Equal("Test", list[1]);
            Assert.Equal("System", list[2]);
            Assert.Equal("System.Collection", list[3]);
        }

        [Fact]
        public void UsingDirectivesCloseScope_Test()
        {
            UsingDirective ud = new UsingDirective();

            ud.Insert("System");
            ud.Insert("System.Collection");

            ud.OpenScope("Test");

            ud.OpenScope("Nested");

            string[] list = ud.Namespaces;

            Assert.Equal(4, list.Length);

            Assert.Equal("Test.Nested", list[0]);
            Assert.Equal("Test", list[1]);
            Assert.Equal("System", list[2]);
            Assert.Equal("System.Collection", list[3]);

            ud.CloseScope();

            string[] list2 = ud.Namespaces;
            Assert.Equal(3, list2.Length);

            Assert.Equal("Test", list2[0]);
            Assert.Equal("System", list2[1]);
            Assert.Equal("System.Collection", list2[2]);
        }
    }
}
