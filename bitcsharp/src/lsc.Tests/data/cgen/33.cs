
class A
{
    public int i, j;

    public static void Main()
    {
        A tmp = new A();
        tmp.j = 5;
        tmp.i = 10;
        int x;
        x = tmp.i;

        __llvmsharp_system_console_write_int(x);
        __llvmsharp_system_console_write_int(tmp.j);
    }

    extern void __llvmsharp_system_console_write_int(int x);
}