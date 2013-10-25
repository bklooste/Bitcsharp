class A
{
    public static void Main()
    {
        bool i = false;
        bool i2 = true;
        int j = 0;

        if (i)
        {
            j = 1;
            __llvmsharp_system_console_write_int(j);
            if (i2)
            {
                j = 3;
                __llvmsharp_system_console_write_int(j);
            }
            else
            {
                j = 4;
                __llvmsharp_system_console_write_int(j);
            }
            __llvmsharp_system_console_write_int(j);
        }
        else
        {
            j = 5;
            __llvmsharp_system_console_write_int(j);
            if (i2)
            {
                j = 6;
                __llvmsharp_system_console_write_int(j);
            }
            else
            {
                j = 7;
                __llvmsharp_system_console_write_int(j);
            }
            __llvmsharp_system_console_write_int(j);
        }
        __llvmsharp_system_console_write_int(j);

        i2 = false;
        i = true;

        if (i)
        {
            j = 1;
            __llvmsharp_system_console_write_int(j);
            if (i2)
            {
                j = 3;
                __llvmsharp_system_console_write_int(j);
            }
            else
            {
                j = 4;
                __llvmsharp_system_console_write_int(j);
            }
            __llvmsharp_system_console_write_int(j);
        }
        else
        {
            j = 5;
            __llvmsharp_system_console_write_int(j);
            if (i2)
            {
                j = 6;
                __llvmsharp_system_console_write_int(j);
            }
            else
            {
                j = 7;
                __llvmsharp_system_console_write_int(j);
            }
            __llvmsharp_system_console_write_int(j);
        }
        __llvmsharp_system_console_write_int(j);
    }

    extern void __llvmsharp_system_console_write_int(int x);
    extern void __llvmsharp_system_console_write_string(int str);
}