using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lsc.Tests.data.parser.good
{
    class MainClass
    {
        public static void Main()
        {
            string a = base[x + 2];
            /*Unary Tests*/
            int x = 5; int y = 2;
            x = +y;
            x = -13;
            y = -x;
            y = +9;
            y=++x;
            x=--y;
            y++;
            ++y;
            bool test; bool test2=true;
            test = !test2;
            test = !false;
            
        }
    }   
}
