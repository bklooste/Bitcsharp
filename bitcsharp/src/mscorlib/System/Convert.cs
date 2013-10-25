
namespace System
{
    public class Convert
    {
        // start of native wrappers

        private extern static int __llvmsharp_system_convert_string_to_int32(string value);
        public static int ToInt32(string value)
        {
            return __llvmsharp_system_convert_string_to_int32(value);
        }

        private extern static float __llvmsharp_system_convert_string_to_single(string value);
        public static float ToSingle(string value)
        {
            return __llvmsharp_system_convert_string_to_single(value);
        }

        private static extern string __llvmsharp_system_convert_int_to_string(int value);
        public static string ToString(int value)
        {
            return __llvmsharp_system_convert_int_to_string(value);
        }

        private static extern string __llvmsharp_system_convert_single_to_string(float value);
        public static string ToString(float value)
        {
            return __llvmsharp_system_convert_single_to_string(value);
        }

        // end of native wrappers
    }
}
