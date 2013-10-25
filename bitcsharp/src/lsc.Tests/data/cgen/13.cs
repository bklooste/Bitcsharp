class A
{
    public static void Main()
    {
        int i = 5;
        int j = i++;
        __llvmsharp_system_console_write_int(j);
        __llvmsharp_system_console_write_int(i++);
        __llvmsharp_system_console_write_int(i);
    }

    extern void __llvmsharp_system_console_write_int(int x);
    extern void __llvmsharp_system_console_write_string(string x);
}