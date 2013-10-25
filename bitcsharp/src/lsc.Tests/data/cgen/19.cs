class A
{
    public static void Main()
    {
        if ("hello" == "world")
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        else
        {
            __llvmsharp_system_console_write_string("right");
        }
        if ("hello" == "hellx")
        {
            __llvmsharp_system_console_write_string("wrong");
        }
        else
        {
            __llvmsharp_system_console_write_string("right");
        }
        if ("hello" == "hello")
        {
            __llvmsharp_system_console_write_string("right");
        }
        else
        {
            __llvmsharp_system_console_write_string("wrong");
        }
    }

    extern void __llvmsharp_system_console_write_string(string x);
}