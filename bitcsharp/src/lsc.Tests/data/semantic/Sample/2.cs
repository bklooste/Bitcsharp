namespace LLVMSharp
{
    class A
    {
        void private_method()
        {
            bool a = true ;

            if (a) { }  //if condition type is System.Boolean
            if (1) { }  //if condition type is System.Int32 
        }
        protected void protected_method()
        {
        }

        public void public_method()
        {
            return;
        }

        int private_int_method()
        {
            return 1;
        }

        void method_caller()
        {
            private_method();   //private method is accessable within the class itself
            protected_method(); //protected method is accessable within the class itself
            public_method();    //public method is accessable within the class itself
            
            //method return type
            int a = private_int_method();         //method reutrn System.Int32
            string str = private_int_method();    //method reutrn System.Int32
        }
    }

    class B:A
    {
        void method_caller()
        {
            A a = new A();

            a.private_method();     //private method is not accessable
            a.protected_method();   //protected method is not accessable
            a.public_method();      //public method is called with the correct parameter(s)
            a.public_method("");    //public method is called with the incorrect parameter(s)

            B b = new B();
            b.private_method();    //private method is not accessable from child class
            b.protected_method();  //protected method is accessable from child class
        }

    }
  
}
