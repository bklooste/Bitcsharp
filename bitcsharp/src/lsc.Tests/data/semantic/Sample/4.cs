namespace LLVMSharp
{
    class A
    {
        int a_private;
        public int a_public;

        void private_method() { }
        public void public_method() { }
    }


    class B
    {
        public static void static_method() { }
    }


    class Tester
    {
        A a = new A();
        B b = new B();

        void method()
        {
            a.a_private = 1;       //private memeber field is not accessable
            a.a_public = 1;        //public memeber field is accessable

            a.private_method();    //private member method is not accessable
            a.public_method();     //public memeber method is accessable

            b.static_method();           //static method must be access using type-name
            LLVMSharp.B.static_method(); //static method must be access using type-name
            B.static_method();           //static method must be access using type-name
        }
    }
}