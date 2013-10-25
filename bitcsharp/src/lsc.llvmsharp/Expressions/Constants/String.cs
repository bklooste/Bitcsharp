using System.Collections;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstStringConstant astStringConstant)
        {
            int length;
            string str = LLVMString(astStringConstant.ConstantValue, out length);

            string llvmStrVarName = StringTablePrefix + Compiler.StringTable[astStringConstant.ConstantValue];

            Alloca strHeader = new Alloca(LLVMModule)
                                 {
                                     Result = "%" + TempCount,
                                     Type = LLVMTypeName("System.String") + "*",
                                 };
            WriteLine(2, strHeader.EmitCode());
            ++TempCount;

            var a = new Alloca(LLVMModule)
                        {
                            Result = "%" + TempCount,
                            Type = "[" + length + " x i8]"
                        };
            WriteLine(2, a.EmitCode());

            ++TempCount;

            var bc = new BitCast(LLVMModule)
                         {
                             Result = "%" + TempCount,
                             Type1 = "[" + length + " x i8]*",
                             Value = "%" + (TempCount - 1),
                             Type2 = "i8*"
                         };

            WriteLine(2, bc.EmitCode());

            var c = new Call(LLVMModule, 4)
                        {
                            ReturnType = "void",
                            FunctionName = "@llvm.memcpy.i32"
                        };
            c.Arguments[0] = "i8* %" + TempCount;

            var idx = new ArrayList(2)
                          {
                              new Index {Type = "i32", Idx = "0"},
                              new Index {Type = "i32", Idx = "0"}
                          };

            var gep1 = new GetElementPtr(LLVMModule)
                           {
                               PointerType = "[" + length + " x i8]",
                               PointerValue = llvmStrVarName,
                               Indices = idx
                           };

            c.Arguments[1] = "i8* " + gep1.EmitCode();
            c.Arguments[2] = "i32 " + length;
            c.Arguments[3] = "i32 1";

            WriteLine(2, c.EmitCode());

            var gep2 = new GetElementPtr(LLVMModule)
                           {
                               Result = "%" + (TempCount + 1),
                               PointerType = gep1.PointerType,
                               PointerValue = "%" + (TempCount - 1),
                               Indices = idx
                           };

            WriteLine(2, gep2.EmitCode());

            ++TempCount;

            Call callManagedString = new Call(LLVMModule, 1)
                                         {
                                             ReturnType = LLVMTypeName(astStringConstant.AssociatedType) + "*",
                                             FunctionName = "@__llvmsharp_System_String_ctor_charPtr",
                                             Result = "%" + (TempCount + 1)
                                         };
            callManagedString.Arguments[0] = "i8* %" + TempCount;

            WriteLine(2, callManagedString.EmitCode());

            ++TempCount;

            Store store = new Store(LLVMModule)
            {
                Type = strHeader.Type,
                Value = "%" + TempCount,
                Pointer = strHeader.Result
            };

            WriteLine(2, store.EmitCode());

            ++TempCount;

            Load l = new Load(LLVMModule)
                       {
                           Result = "%" + TempCount,
                           Type = callManagedString.ReturnType,
                           Pointer = strHeader.Result
                       };
            //
            WriteLine(2, l.EmitCode());

            //++TempCount;
        }
    }
}