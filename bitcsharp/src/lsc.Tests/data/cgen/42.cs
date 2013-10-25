using System;

class A
{
    public void Hello(string msg, int times)
    {
        int i = 0;
        while (i < times)
        {
            Console.Write("Hello ");
            Console.Write(msg);
            i += 1;
        }
    }

    public static void World(string msg, int times)
    {
        A tmp = new A();
        tmp.Hello(msg, times);
    }

    public static void Main()
    {
        World("Prabir", 2);
    }
}
