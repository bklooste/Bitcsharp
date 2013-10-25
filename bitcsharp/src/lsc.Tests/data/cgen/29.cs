class A
{
    public static void Main()
    {
        int i = 5;
        i--;
        __llvmsharp_system_console_write_int(i);
        __llvmsharp_system_console_write_int(i--);
        __llvmsharp_system_console_write_int(i);
    }

    extern void __llvmsharp_system_console_write_int(int x);
}