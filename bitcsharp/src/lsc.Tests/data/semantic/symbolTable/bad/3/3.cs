
namespace nsp
{
    class A
    {
        void method(int a, string b)
        {
            if (1) // Type error, if conditions has to be boolean
            {  
            }

            //Type Errors
            while(1){}
            for (int i; 1; i++) { }
            do { } while (1);

            int i;
            i = "string";

            string str;
            str = 1;
            str = i;
            i = str;
        }
        void methodcaller()
        {
            string str; 
            int  i;
            //Type mismatched method call
            method("a", 1);
            method(str, i);
            method(str, str);
            method(1, 2);
        }
    }
}
