namespace LLVMSharp
{
    class A
    {
        public A(){}

        public A(int i) { }
        public A(string str) { }
    }

    class Tester
    {
        A obja = new A();


        void method()
        {
            A a1 = new A(1);           //check object A's constructor with System.Int32
            A a2 = new A("string");    //check object A's constructor with System.String

            A a3 = new A(1, 2);       //check object A's constructor with (System.Int32, System.Int32) 

        }
    }


}