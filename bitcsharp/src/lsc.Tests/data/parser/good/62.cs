using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lsc.Tests.data.parser.good
{
    class MainClass
    {
        void function1()
        { }
        void function2(string filename, int a, double b)
        { }
        public static void Main()
        {
            string x; int z; double n;
            MainClass c = new MainClass();
            c.function1();
            hello();
            c.function2("something.txt",10,2.5);
            c.function2(x,z,n);
        }
    }   
}
