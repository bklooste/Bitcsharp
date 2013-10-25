class A
{
    public static void Main()
    {
        bool a;
        a = (true != false);
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