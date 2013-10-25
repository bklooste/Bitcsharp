using System;

class A
{
    public A()
    {
        Console.Write("a ctor");
    }

    public static void Main()
    {
        A tmp = new A();
    }
}