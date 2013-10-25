class B
{
    public static void Hello()
    {
        __llvmsharp_system_console_write_string("Hello");
    }

    extern static protected void __llvmsharp_system_console_write_string(string x);
}

class A : B
{
    public static void Main()
    {
        Hello();
        World();
    }

    public static void World()
    {
        __llvmsharp_system_console_write_string("world");
    }
}