
namespace System
{
    public class Environment
    {
        private static extern void __llvmsharp_system_environment_exit(int exitCode);
        public static void Exit(int exitCode)
        {
            __llvmsharp_system_environment_exit(exitCode);
        }
    }
}
