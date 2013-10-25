namespace hello
{
    namespace world
    {

        public sealed class hi
        {
            public enum TestEnum
            {
                x,
                y
            }

            public struct TestStruct
            {
                public TestStruct method1(string a) { return new TestStruct();  }
                private void method1(int a, int b, TestStruct x) { }
                protected TestEnum method3(int a, ref int b, TestStruct x) { return TestEnum.x; }
                public extern string method4();
                public string Hello() { return "Hello World!"; }
                public int Hello2() { return 1; }
                public bool Hello3(int a,int b) { return true; }
            }

            public class helloworld
            {
                public TestStruct method1(string a) { return new TestStruct(); }
                private void method1(int a, int b, TestStruct x) { }
                protected TestEnum method3(int a, ref int b, TestStruct x) { return TestEnum.x; }
                public extern string method4();
                public string Hello() { return "Hello World!"; }
                public int Hello2() { return 1; }
                public bool Hello3(int a, int b) { return true; }
            }
        };
    }
}
