class A
{
    public static void Main()
    {
        __llvmsharp_system_console_write_string("print this");
        return;
        __llvmsharp_system_console_write_string("dont print this");
    }

    extern void __llvmsharp_system_console_write_string(string x);
}