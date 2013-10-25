class A
{
    public static void Main()
    {
        bool a = true || false;
        if (true || false)
        {
            __llvmsharp_system_console_write_string("right");
        }
        else
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        if (false || true)
        {
            __llvmsharp_system_console_write_string("right");
        }
        else
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        if (false || false)
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        else
        {
            __llvmsharp_system_console_write_string("right");
        }
    }

    extern void __llvmsharp_system_console_write_int(int x);
    extern void __llvmsharp_system_console_write_string(string x);
}