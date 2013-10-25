using System;

namespace Data.Good
{
    class A
    {
        public int i;

        public static void Main()
        {
            A tmp = new A();
            tmp = new A();
            tmp.i = 1;
            Console.Write(tmp.i);
            tmp.i = 2;
            Console.Write(tmp.i);
            tmp = new A();
            Console.Write(tmp.i);
        }

    }
}