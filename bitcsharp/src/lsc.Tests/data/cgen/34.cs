
class A
{
    public int i;

    public static void Main()
    {
        A tmp = new A();
        tmp.i = 1;
        __llvmsharp_system_console_write_int(tmp.i++);
        __llvmsharp_system_console_write_int(tmp.i);
    }

    extern void __llvmsharp_system_console_write_int(int x);
}