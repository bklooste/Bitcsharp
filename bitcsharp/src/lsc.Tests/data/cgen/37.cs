class A
{
    public static void Main()
    {
        Hello();
        Hello("prabir");
    }

    public static void Hello()
    {
        __llvmsharp_system_console_write_string("Hello");
    }

    public static void Hello(string str)
    {
        __llvmsharp_system_console_write_string(str);
    }
    
    extern static private void __llvmsharp_system_console_write_string(string x);
}