//using EnglishAlphabet;
namespace EnglishAlphabet
{
    class A
    {
        int integer1;
        //A(int integer1)
        //{this.integer1=integer1;}
        public A()
        { }
        public A(int x)
        {
            integer1 = x;
        }
        A function()
        {
            A obj= null;
            return obj;
        }
        public void function1(int a, char b, float c)
        {
            B obj2 = new B();
            //return obj2;
        }

        A function2(A x, B y)//testing built in types
        {
            return x;
        }

    }
    class B
    {
 
    }
}

class C
{
    C function3(char x, A y)
    {
        C something = new C();
        return something;
    }
}

