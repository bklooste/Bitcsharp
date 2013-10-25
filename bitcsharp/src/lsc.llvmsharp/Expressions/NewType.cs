using System;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstNewType astNewType)
        {
            if (Compiler.ClassHashtable.ContainsKey(astNewType.FullQualifiedType))
            {
                GenerateAstNewTypeForClass(astNewType);
            }
        }

        private void GenerateAstNewTypeForClass(AstNewType astNewType)
        {
            if (!astNewType.IsArray)
            {

                AstClass astClass = (AstClass)Compiler.ClassHashtable[astNewType.FullQualifiedType];
                string[] args = new string[astNewType.AstArgumentCollection.Count];
                int i = 0;
                foreach (AstArgument arg in astNewType.AstArgumentCollection)
                    args[i++] = arg.AssociatedType;

                string name = astNewType.Type;
                int lastDotIndex = name.LastIndexOf('.');
                if (lastDotIndex != 0)
                    name.Substring(lastDotIndex + 1, name.Length);

                string key = astClass.GetConstructorKey(name, args, false);
                if (string.IsNullOrEmpty(key))
                    key = astClass.GetConstructorKey(name, args, true);

                Call c = new Call(LLVMModule, astNewType.AstArgumentCollection.Count)
                {
                    FunctionName = "@__LS" + Mangler.Instance.MangleName(astNewType.FullQualifiedType) + key,
                    Result = "%" + TempCount,
                    ReturnType = LLVMTypeName(astNewType.FullQualifiedType) + "*"
                };
                GenerateArguments(astNewType, ref c.Arguments);

                WriteLine(2, c.EmitCode());
            }
        }

        private void GenerateArguments(AstNewType astNewType, ref string[] llvmCallArguments)
        {
            for (int i = 0; i < astNewType.AstArgumentCollection.Count; i++)
            {
                AstArgument arg = (AstArgument)astNewType.AstArgumentCollection[i];

                arg.EmitCode(this);

                llvmCallArguments[i] = GenerateArgs(arg);

                ++TempCount;
            }
        }
    }
}