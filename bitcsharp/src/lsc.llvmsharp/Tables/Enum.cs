using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        private void EmitCodeForEnum()
        {
            foreach (string i in Compiler.EnumHashtable.Keys)
                EmitCodeForEnum((AstEnum)Compiler.EnumHashtable[i]);
        }

        private void EmitCodeForEnum(AstEnum astEnum)
        {
            WriteInfoComment(" enum " + astEnum.FullQualifiedName + " { ");
//            WriteSourcePath(astEnum.Path);
            WriteLine();

            int i = 0;
            foreach (AstEnumMember item in astEnum.AstEnumMemberCollection)
            {
                LLVM.Variable v = new LLVM.Variable(LLVMModule)
                {
                    IsGlobal = true,
                    IsConstant = true,
                    LinkageType = LLVM.LinkageType.None,
                    Name = PREFIX + Mangler.Instance.MangleEnumMember(astEnum.FullQualifiedName, item.Name),
                    Value = new LLVM.I32Constant()
                    {
                        Value = i++
                    }
                };

                Writer.Write(INDENT_SPACE);
                Writer.WriteLine(v.EmitCode());

            }

            WriteInfoComment(" } ");
            WriteLine();
            WriteLine();

            /* C# input code:
                enum Color
                {
                    Red,
                    Green,
                    Blue
                }
             * 
             * LLVM Generated Code
             * 
                ; enum Color { 
                     @_LSEi32_ColorRed =  constant i32 0
                     @_LSEi32_ColorGreen =  constant i32 1
                     @_LSEi32_ColorBlue =  constant i32 2
                ; } 
             * 
             * C++ usage
             * a.h
                 extern "C" {
                    extern const int _LSEi32_ColorBlue;
                }
             * a.cpp
                #include "a.h"

                int main()
                {
                    return _LSEi32_ColorBlue;
                }
            * LLVM usage
                %tmp = load i32* @_LSEi32_ColorBlue
                ret %tmp;
             ♥1♥
             * */
        }
    }
}