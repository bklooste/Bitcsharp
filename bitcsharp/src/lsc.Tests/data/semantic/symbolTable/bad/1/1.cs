namespace Data.bad
{
    public struct S
    {
        public S(string a)
        {

        }

        public void Method()
        {

        }

        public int MethodCaller()
        {
            return 1;
        }
    }

    public class A
    {
        public int a;

        public A()
        {
        }

        A(string str)
        {
        }

        public static implicit operator A(C c)
        {
            A ret = new A();
            return ret;
        }

        public static B operator +(A first, C second)
        {
//            return 1;

            B ret = new B();
            return ret;
        }

        public void MemberMethod()
        {

        }        
    }

    public class B:A
    {
       
    }

    public struct C 
    {
        public static implicit operator A(C c)
        {
            A ret = new A();
            return ret;

        }

        //public static A operator +(A first, C second)
        //{
        //    A ret = new A();
        //    return ret;
        //}

        public static C operator ++(C oper)
        {
            C ret = new C();
            return ret;
        }
    }
    

    enum Color
    {
        red,
        green
    }

    /// <summary>
    /// Type Checking Tester
    /// </summary>
    public class Hello
    {
        A obja;
        B objb;
        C objc;

        public B subtype()
        {
            A a = new A();

            return a;
        }

        Hello(int a)
        {
            //Either Object A or Object C 
            //Has to have a method that accept Obejct A and C       
            objb = obja + objc;       //No Error : A + C
            a = obja + obja;
            a = objc + obja + objc; //Error : A + C
           


            //Object C has to Convert it to Object A
            //It can either done by 
            //C having a method that covert C -> A 
            //Or 
            //A having a method that convert C -> A
            obja = objc;
            objb = objc;

            A lobja;

            //Member Method
            obja.MemberMethod(); //No Error
            lobja.MemberMethod(); //No Error            
            lobja.MemberMethod1(); //Error

            const int ci = 1;

            ci = 1;

            objb = obja;                                // Error   : Sub Type
            obja = objb;                                // No Error: Sub Type
            
            //Struct
            S s = new S(1);

            for (; ; )
            {
                if (true)
                {
                    break;
                }
            }

            continue;


            //Member Reference
            a = obja.a;     //No Error 
            a = objaa.a;    //Error
            a = obja.bb.a;  //Error

            //Array
            A[] arr1, arr2 ;

            a = a + arr1;

            arr1 = arr2;

            //Either Object A or Object C 
            //Has to have a method that accept Obejct A and C            
            a = obja + obja;
            a = objc + obja + objc; //Error : A + C
            objb = obja + objc;       //No Error : A + C

            
            //Unary
            obja = objc++;
            objb = objc++;
            objc = (objc++) + (objc++);


            obja = new C();

            bool boo;
            string str = "";
            obja = new A(boo);   //Error: New Object
            obja = new A(a);     //Error: New Object
            objb = new A(str);   //No Error: New Object

            obja = new A();      //No Error: New Object
            obja = new A("str"); //No Error: New Object
            obja = new A(1);     //Error: New Object

            a = function_call(); //Error : Method Call
            a = Method(1);       //Error : Method Call
            a = Method(1,2);     //No Error : Method Call

           
            str = Method(1, 2);     //Error : Method Call

            a = null;               //No Error : null
            z = Color.green;
            
            a = 1 + null;           //Error    : null

            string b;

            if (1)                                      // Error : Condition Type
            {
                while (1)                               // Error : Condition Type
                { 
                }
                for (int i = 0; 1 ; i++)                // Error : Condition Type
                {

                }
                do
                {
                } while (1);                            // Error : Condition Type
            }

            bool flag;
            while (flag)                                // No Error : Condition Type
            {
            }
            if (flag)                                   // No Error : Condition Type
            {
            }
            do
            {
            } while (flag);                             // No Error : Condition Type


            a = 1 + "b";            // Error : Addition & Assignment
            a = 1 - "a";            // Error : Substraction & Assginment
            a = 1 + 8 - 4 + "aa" ;  // Error : Addition & Subsctractiong & Assignment

            a = 1 = 2 = "a";        // Error : Nested Assignment

            a = a + b;              // Error : Variable Type Different

        }

        //int Method(int a, int b){
            
        //    int a; // Error : Declare Loval Variable twice

        //    return "a"; //Error : Return Type
        //}


    }
}