
namespace System
{
    public class Console
    {
        // start of native wrappers

        private static extern void __llvmsharp_system_console_writeline();
        public static void WriteLine()
        {
            __llvmsharp_system_console_writeline();
        }

        private extern static void __llvmsharp_system_console_write_int(int value);
        public static void Write(int value)
        {
            __llvmsharp_system_console_write_int(value);
        }

        private extern static void __llvmsharp_system_console_write_string(string value);
        public static void Write(string value)
        {
            __llvmsharp_system_console_write_string(value);
        }

        private extern static void __llvmsharp_system_console_write_single(float value);
        public static void Write(float value)
        {
            __llvmsharp_system_console_write_single(value);
        }

        private extern static string __llvmsharp_system_console_readline();
        public static string ReadLine()
        {
            return __llvmsharp_system_console_readline();
        }

        // end of native wrappers


        public static void WriteLine(int value)
        {
            Write(value);
            WriteLine();
        }

        public static void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }

        public static void WriteLine(float value)
        {
            Write(value);
            WriteLine();
        }

    }
}
