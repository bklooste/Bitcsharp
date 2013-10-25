using Morris2;
using System;

namespace Morris2
{
    public class S2
    {
        static public void s2()
        {
        }
        public int[] MyProperty { get; set; }
    }

    public class S3
    {
    }
}

namespace Morris.m
{
    public struct S
    {
        public int s;
        public static A a;

        public  void Sfunc() { }

    }

    public class A
    {
        public static int statica;

        int priavte_a;

        A obja;

        public virtual void a()
        {
            obja.priavte_a = 1;
        }
        public static void Afunc() { }
    }

    public class B : A
    {
        public int i;

        public override void a()
        {
        }
        public static void a2()
        {
        }
    }



    public class Hello : B
    {
        A obja;
        B objb;
        

        public B subtype()
        {
            Console.Write("");
            
            if(true){}
            else if(false){
            }
            else if (false)
            {
            }

            A.statica = 1;

            string str = "Test";

            statica = 1;
            
            S.Sfunc();
 
            this.h();
            base.a();

            obja = S.a;                       

            Morris.m.B.a2();
            Morris2.S2.s2();

            S2 s2;
            S3 s3;

            S2.s2();

            obja = s2;

            objb = new B();
            objb.a();

            B localb;
            S lstruct;

            lstruct.a.a();
            lstruct.s = 1;
            localb.a();

            objb.i = 1;

            A a = objb;
            a.a();

            Morris.m.B.a();

            this.objb = new B();

            h();

            return objb;
        }

        public static void h()
        {

        }
    }
}
