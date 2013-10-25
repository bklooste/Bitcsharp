class A
{
    public static void Main()
    {
        if (true)
        {
            __llvmsharp_system_console_write_int(1);
        }
        else
        {
            __llvmsharp_system_console_write_int(0);
        }

        if (false)
        {
            __llvmsharp_system_console_write_int(0);
        }
        else
        {
            __llvmsharp_system_console_write_int(1);
        }
    }

    extern void __llvmsharp_system_console_write_int(int x);
}