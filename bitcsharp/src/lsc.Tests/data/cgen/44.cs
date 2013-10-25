using System;

namespace Data.Good
{
    class A
    {
        public int i, j;

        public static void Main()
        {
            A tmp = new A();
            tmp.Hello();
            tmp.World();
        }

        public void Hello()
        {
            i = 3;
            Console.Write(i);
        }

        public void World()
        {
            A tmp = new A();
            tmp.j = 5;
            tmp.i = 10;
            int x;
            x = tmp.i;

            Console.Write(x);
            Console.Write(tmp.j);
        }
    }
}