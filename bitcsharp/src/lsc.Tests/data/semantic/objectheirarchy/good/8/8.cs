namespace World 
{
    class A
    {
        public string text1;
        private int num1;
        public string name;
        int x;
        public int Num
        {
            get;
            set;
        }
        public A(int x, string a, A axes)
        { }
        void function1()
        {
        }
        public extern void ExternFunction(string s);
        
        //int function1(int x)
        //{
        //    return x;
        //}

        public static int operator +(A first, B second)
        {
            A a; B b;

            //a + a;
            //int x; 
            //X something= new X();
            //return new B();
            return 1;
        }

        public static B operator +(A first, A second)
        {
            A a; B b;

            //a + a;
            //int x; 
            //X something= new X();
            return new B();
        }

        
        void function1(int x, A a)
        {
            x = 5;
            B b= new B();
            b = a + a;
            //x = a + b;
        }
        //public static implicit operator A (B b)
        //{
        //    return new A();
        //}
        public extern void FF();
        
       
    }
    struct S
    {
        public extern void FF();
        
        public void Something()
        {}
        static virtual void ANother_F()
        {}
    }
    class B
    {
        private string text1;
        private int num1;
        public string name;

        public static A operator +(A first, B second)
        {
            A a; B b; C c;
            //b = c;
            //b = a + a;
            int x;
            //X something = new X();
            return null;
        }
        public object Soemthing()
        {
            
            return null;
        }

        //public static A operator <(A first, B second)
        //{ return new A(); }
         virtual void function1()
        {
            //A a= new A(); B b= new B();
            //int i = a + b;
            //b.Num.CompareTo(1);
        }
        //void function1(int x)
        //{
        //    x = 5;
        //}
        int function1(int x)
        {
            return x;
        }
    }
    class C
    {
        public static implicit operator B(C c)
        {
            return new B();
        }
    }

}

