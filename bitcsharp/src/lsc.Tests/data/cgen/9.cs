class A
{
    public static void Main()
    {
        int i = 3;
        i = i + -i;
        __llvmsharp_system_console_write_int(i);
        i = 2;
        __llvmsharp_system_console_write_int(-i);
        i = -i;
        __llvmsharp_system_console_write_int(i);
    }

    extern void __llvmsharp_system_console_write_int(int x);
}