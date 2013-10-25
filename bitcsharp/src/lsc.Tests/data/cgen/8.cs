class A
{
    public static void Main()
    {
        int i;
        i = +1;
        __llvmsharp_system_console_write_int(i);
    }

    extern void __llvmsharp_system_console_write_int(int x);
}