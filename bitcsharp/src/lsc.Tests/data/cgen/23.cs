class A
{
    public static void Main()
    {
        if (1 < 2)
        {
            __llvmsharp_system_console_write_string("right");
        }
        else
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        if (2 < 1)
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        else
        {
            __llvmsharp_system_console_write_string("right");
        }
        if (2 < 2)
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        else
        {
            __llvmsharp_system_console_write_string("right");
        }
    }

    extern void __llvmsharp_system_console_write_string(string x);
}