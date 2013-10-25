using System;

class A : B
{
    public static void Main()
    {
        A tmpA = new A();
        tmpA.Hello();
        tmpA.World();
    }

    public void Hello()
    {
        Console.Write("Hello ");
    }
}

class B
{
    public void World()
    {
        Console.Write("World");
    }
}