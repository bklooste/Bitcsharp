using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lsc.Tests.data.parser.good
{
    class Program
    { 
    }
    class MainClass
    {
        
        public static void Main()
        {
            int x = 5;
            Program a = new Program();
            MainClass b = new MainClass();
            b=(Program)x;
        }
    }   
}
