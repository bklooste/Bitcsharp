
using System.Text;
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        protected string LLVMClassName(string fullClassName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("%struct.");
            sb.Append(PREFIX + "C_");
            sb.Append(Mangler.Instance.MangleName(fullClassName));

            return sb.ToString();
        }

        protected string LLVMClassName(AstClass astClass)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("%struct.");
            sb.Append(PREFIX + "C_");
            sb.Append(Mangler.Instance.MangleName(astClass.FullQualifiedName));

            return sb.ToString();
        }

        protected string LLVMTypeName(string type)
        {
            switch (type)
            {
                case "System.Int32":
                    return "i32";
                case "System.Single":
                    return "float";
                case "System.Void":
                    return "void";
                case "System.Boolean":
                    return "i8";
                case "System.String":
                    return "%struct.__llvmsharp_stringHeader";
                default:
                    if (Compiler.ClassHashtable.ContainsKey(type))
                        return LLVMClassName(type);
                    return string.Empty;
            }
        }

        protected string LLVMTypeNamePtr(string type)
        {
            string r = LLVMTypeName(type);
            if (Compiler.ClassHashtable.ContainsKey(type))
                r += "*";
            return r;
        }
    }
}