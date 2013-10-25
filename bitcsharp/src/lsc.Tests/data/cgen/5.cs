class A
{
    public static void Main()
    {
        bool i = false;

        int j = 0;

        if (i)
        {
            j = 1;
        }
        else
        {
            j = 2;
        }
        __llvmsharp_system_console_write_int(j);

        i = true;
        if (i)
        {
            j = 3;
        }
        else
        {
            j = 4;
        }
        __llvmsharp_system_console_write_int(j);
    }

    extern void __llvmsharp_system_console_write_int(int x);
}