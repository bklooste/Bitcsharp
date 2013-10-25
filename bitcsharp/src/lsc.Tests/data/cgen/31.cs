
class A
{
    public static void Main()
    {
        float a = 1.3; float b = 2.4; float c = 7.9; float d = 1.3;

        a += 2.5;
        __llvmsharp_system_console_write_single(a);
        a += b;
        __llvmsharp_system_console_write_single(a);
        c -= 1.0;
        __llvmsharp_system_console_write_single(c);
        c -= d;
        __llvmsharp_system_console_write_single(c);
        d *= b;
        __llvmsharp_system_console_write_single(d);
        d *= 2.0;
        __llvmsharp_system_console_write_single(d);
        b /= 0.2;
        __llvmsharp_system_console_write_single(b);
        b /= a;
        __llvmsharp_system_console_write_single(b);
    }
    extern void __llvmsharp_system_console_write_single(float f);
}