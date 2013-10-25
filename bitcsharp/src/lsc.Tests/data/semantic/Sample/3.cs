namespace LLVMSharp
{
    class A
    {
        public static implicit operator A(B b)
        {
            A ret = new A();
            return ret;
        }

        //public static B operator +(A first, B second)
        //{
        //    B ret = new B();
        //    return ret;
        //}
    }

    class B
    {
        //public static implicit operator A(B b)
        //{
        //    A ret = new A();
        //    return ret;
        //}

        public static B operator +(A first, B second)
        {
            B ret = new B();
            return ret;
        }

        public static B operator ++(B oper)
        {
            B ret = new B();
            return ret;
        }
    }

    class Tester
    {
        A obja = new A();
        B objb = new B();

        void method()
        {
            obja = objb;   //type convert from B -> A
                           //either class A or B has to have type convert method
                           //cannot have it in both

            objb = obja;   // type convert from A -> B is not implemented

            obja = obja + objb;   // '+' binary operator overloading method with Object A and Object B 
                                  // either class A or B has to be have the method
                                  // cannot have it in both
            int a = obja + objb;  // '+' binary operator overloading method with Object A and Object B 
            obja = objb + obja;   // '+' binary operator overloading method with Object B and Object A 
            objb++;               // '++' unary operator overloading method in object B

        }
    }
}