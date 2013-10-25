
class A
{
    public static void Main()
    {

        float a = 1.3; float b = 2.4; float c = 7.9; float d = 1.3;
        bool test = false;

        if (a < b)        
            __llvmsharp_system_console_write_string("true");        
        if (a <= b)
            __llvmsharp_system_console_write_string("true");        
        if (c > b)        
            __llvmsharp_system_console_write_string("true");        
        if (c >= b)        
            __llvmsharp_system_console_write_string("true");        
        if (a != c)
            __llvmsharp_system_console_write_string("true");
        if (a == d)
            __llvmsharp_system_console_write_string("true");

    }
    extern void __llvmsharp_system_console_write_string(string s);
}