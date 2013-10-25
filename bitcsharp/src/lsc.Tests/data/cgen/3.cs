// integer local var assingment

class B
{
    public static void Main()
    {
        int i;

        i = 1;
        __llvmsharp_system_console_write_int(i);

        i = 2 * 3;
        __llvmsharp_system_console_write_int(i);
    }

    extern void __llvmsharp_system_console_write_int(int x);
    extern void __llvmsharp_system_console_write_string(string s);
}