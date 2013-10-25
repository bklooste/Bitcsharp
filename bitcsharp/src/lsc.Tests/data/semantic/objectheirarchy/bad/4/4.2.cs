//using EnglishAlphabet;
namespace EnglishAlphabet
{
    class A
    {
        private int integer1;
        private string text1;

        public A(int x)
        {
            integer1 = x;
        }
        A function()
        {
            A obj= null;
            return obj;
        }
        public int MyInt
        {
            get
            {
                return integer1;
            }
            set
            {
                integer1 = value;
            }
        }
        public string MyInt
        {
            get
            {
                return text1;
            }
            set
            {
                text1 = value;
            }
        }

    }
    class B
    {
 
    }
}

class C
{
    C function3(char x, EnglishAlphabet.A y)
    {
        C something = new C();
        return something;
    }
}

