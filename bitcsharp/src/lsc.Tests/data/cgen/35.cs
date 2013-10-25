
class A
{
    public string str;
    public string str2;
    public int i;

    public static void Main()
    {
        A tmp = new A();
        tmp.str = "str";
        tmp.str2 = "str2 ";
        string x = "hello";
        __llvmsharp_system_console_write_string(tmp.str);
        __llvmsharp_system_console_write_string(x);
        __llvmsharp_system_console_write_string(tmp.str2);
        tmp.i = 10;
        __llvmsharp_system_console_write_int(tmp.i); ;

    }

    extern void __llvmsharp_system_console_write_int(int x);
    extern void __llvmsharp_system_console_write_single(float x);
    extern void __llvmsharp_system_console_write_string(string x);
}