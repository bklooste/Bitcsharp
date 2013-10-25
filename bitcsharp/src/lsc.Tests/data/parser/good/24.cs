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

            }

            public class helloworld
            {
                helloworld() { }
                public helloworld(string a) { }
                private helloworld(int a, int b,TestStruct x) { }
                protected helloworld(int a, ref int b, TestStruct x) { }

                helloworld() : this() { }
                helloworld() : base() { }
            }
        };
    }
}
