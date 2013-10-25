class A
{
    public static void Main()
    {
        bool a;
        a = (1 != 2);
        if (a)
        {
            __llvmsharp_system_console_write_string("right");
        }
        else
        {
            __llvmsharp_system_console_write_string("wrong");
        }

    }

    extern void __llvmsharp_system_console_write_string(string x);
}