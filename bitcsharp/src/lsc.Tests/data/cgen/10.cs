class A
{
    public static void Main()
    {
        bool i = !true;
        if (i)
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        else
        {
            __llvmsharp_system_console_write_string("right");
        }
        __llvmsharp_system_console_write_string(" ");
        if (!i)
        {
            __llvmsharp_system_console_write_string("right");
        }
        else
        {
            __llvmsharp_system_console_write_string("wrong");
        }
    }

    extern void __llvmsharp_system_console_write_int(int x);
    extern void __llvmsharp_system_console_write_string(string x);
}