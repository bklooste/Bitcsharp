
namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class Helpers
    {
        public static string ToString(LinkageType lt)
        {
            switch (lt)
            {
                case LinkageType.None:
                    return string.Empty;
                case LinkageType.Private:
                    return "private";
                case LinkageType.Linker_private:
                    return "linker_private";
                case LinkageType.Internal:
                    return "internal";
                case LinkageType.Available_externally:
                    return "available_externally";
                case LinkageType.LinkOnce:
                    return "linkonce";
                case LinkageType.Common:
                    return "common";
                case LinkageType.Weak:
                    return "weak";
                case LinkageType.Appending:
                    return "appending";
                case LinkageType.Extern_Weak:
                    return "extern_weak";
                case LinkageType.Linkonce_Odr:
                    return "linkonce_odr";
                case LinkageType.Weak_Odr:
                    return "weak_odr";
                case LinkageType.Externally_Visible:
                    return "externally_visible";
                case LinkageType.Dllimport:
                    return "dllimport";
                case LinkageType.Dllexport:
                    return "dllexport";
                default:
                    return "internal";
            }
        }
    }
}
