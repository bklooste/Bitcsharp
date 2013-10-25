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
                public void method1(string a) { }
                private void method1(int a, int b, TestStruct x) { }
                protected void method3(int a, ref int b, TestStruct x) { }
                public extern void method4() { }
            }

            public class helloworld
            {
                helloworld() { }
                public void method1(string a) { }
                private void method1(int a, int b, TestStruct x) { }
                protected void method3(int a, ref int b, TestStruct x) { }
                public extern void method4() { }
                helloworld() : this() { }
                helloworld() : base() { }
            }
        };
    }
}
