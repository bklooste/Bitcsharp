using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            WriteIndentSpace(3);
            WriteInfoCommentLine(string.Format("{0} {1}", astLocalVariableDeclaration.FullQualifiedType,
                                               astLocalVariableDeclaration.Name));

            switch (astLocalVariableDeclaration.FullQualifiedType)
            {
                case "System.Int32":
                    GenerateLocalVarDeclForInt32(astLocalVariableDeclaration);
                    break;
                case "System.String":
                    GenerateLocalVarDeclForString(astLocalVariableDeclaration);
                    break;
                case "System.Boolean":
                    GenerateLocalVarDeclForBool(astLocalVariableDeclaration);
                    break;
                case "System.Single":
                    GenerateLocalVarDeclForFloat(astLocalVariableDeclaration);
                    break;
                default:
                    if (Compiler.ClassHashtable.ContainsKey(astLocalVariableDeclaration.FullQualifiedType))
                        GenerateLocalVarDeclForClass(astLocalVariableDeclaration);
                    else if (Compiler.StructHashtable.ContainsKey(astLocalVariableDeclaration.FullQualifiedType))
                        GenerateLocalVarDeclForStruct(astLocalVariableDeclaration);
                    else if (Compiler.EnumHashtable.ContainsKey(astLocalVariableDeclaration.FullQualifiedType))
                        GenerateLocalVarDeclForEnum(astLocalVariableDeclaration);
                    break;

            }
        }

        private void GenerateLocalVarDeclForEnum(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            // throw new System.NotImplementedException();
        }

        private void GenerateLocalVarDeclForStruct(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            Alloca a = new Alloca(LLVMModule)
                           {
                               Type = LLVMTypeName(astLocalVariableDeclaration.FullQualifiedType) + "*",
                               Result = "%l_" + astLocalVariableDeclaration.Name
                           };
            WriteLine(2, a.EmitCode());

            if (astLocalVariableDeclaration.Initialization == null)
            {
                Store sNull = Store.StoreNull(LLVMModule, a.Type, a.Result);
                WriteLine(2, sNull.EmitCode());
            }
            else
            {
                astLocalVariableDeclaration.Initialization.EmitCode(this);

                Store s = new Store(LLVMModule)
                              {
                                  Type = a.Type,
                                  Value = "%" + TempCount,
                                  Pointer = a.Result
                              };
                ++TempCount;

                WriteLine(2, s.EmitCode());

            }
        }

        private void GenerateLocalVarDeclForClass(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {

            Alloca a = new Alloca(LLVMModule)
                         {
                             Type = LLVMTypeName(astLocalVariableDeclaration.FullQualifiedType) + "*",
                             Result = "%l_" + astLocalVariableDeclaration.Name
                         };
            WriteLine(2, a.EmitCode());

            #region Prepare GC root

            Alloca root = new Alloca(LLVMModule)
                              {
                                  Type = "%struct.__llvmsharp_gc_root*",
                                  Result = "%lgcroot_" + astLocalVariableDeclaration.Name
                              };
            WriteLine(2, root.EmitCode());

            Store storeNullToRoot = Store.StoreNull(LLVMModule, root.Type, root.Result);
            WriteLine(2, storeNullToRoot.EmitCode());

            #endregion


            if (astLocalVariableDeclaration.Initialization == null)
            {
                Store sNull = Store.StoreNull(LLVMModule, a.Type, a.Result);
                WriteLine(2, sNull.EmitCode());
            }
            else
            {
                astLocalVariableDeclaration.Initialization.EmitCode(this);

                Store s = new Store(LLVMModule)
                            {
                                Type = a.Type,
                                Value = "%" + TempCount,
                                Pointer = a.Result
                            };
                ++TempCount;

                WriteLine(2, s.EmitCode());

                BitCast bcLvar = new BitCast(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Value = a.Result,
                    Type1 = a.Type + "*",
                    Type2 = "i8*"
                };
                WriteLine(2, bcLvar.EmitCode());

                Call markRoot = new Call(LLVMModule, 1)
                                  {
                                      FunctionName = "@__llvmsharp_gc_markRootAndReturn",
                                      ReturnType = "%struct.__llvmsharp_gc_root*",
                                      Result = "%" + (TempCount + 1)
                                  };
                markRoot.Arguments[0] = "i8* " + bcLvar.Result;
                WriteLine(2, markRoot.EmitCode());
                ++TempCount;

                Store storeRoot = new Store(LLVMModule)
                                   {
                                       Type = markRoot.ReturnType,
                                       Value = markRoot.Result,
                                       Pointer = root.Result
                                   };
                WriteLine(2,storeRoot.EmitCode());
                ++TempCount;
            }
        }

        private void GenerateLocalVarDeclForBool(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            if (!astLocalVariableDeclaration.IsConstant)
            {
                if (!astLocalVariableDeclaration.IsArray)
                {
                    Alloca a = new Alloca(LLVMModule)
                                 {
                                     Result = "%l_" + astLocalVariableDeclaration.Name,
                                     Type = "i8"
                                 };

                    WriteLine(2, a.EmitCode());

                    if (astLocalVariableDeclaration.Initialization != null)
                    {
                        astLocalVariableDeclaration.Initialization.EmitCode(this);

                        Store s = new Store(LLVMModule)
                        {
                            Type = "i8",
                            Value = "%" + TempCount,
                            Pointer = a.Result
                        };

                        if (astLocalVariableDeclaration.AssociatedType == "System.Boolean")
                            s.Type = "i8";
                        WriteLine(2, s.EmitCode());

                        ++TempCount;
                    }
                }
            }
        }

        private void GenerateLocalVarDeclForString(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            if (!astLocalVariableDeclaration.IsConstant)
            {
                Alloca strHdr = new Alloca(LLVMModule)
                                  {
                                      Result = "%l_" + astLocalVariableDeclaration.Name,
                                      Type = LLVMTypeName(astLocalVariableDeclaration.FullQualifiedType) + "*"
                                  };
                WriteLine(2, strHdr.EmitCode());

                Store storeNull = new Store(LLVMModule)
                {
                    Type = strHdr.Type,
                    Value = "null",
                    Pointer = strHdr.Result
                };

                if (astLocalVariableDeclaration.Initialization == null)
                    WriteLine(2, storeNull.EmitCode());
                else if (astLocalVariableDeclaration.Initialization is AstNull)
                    WriteLine(2, storeNull.EmitCode());
                else
                {
                    astLocalVariableDeclaration.Initialization.EmitCode(this);

                    Store store = new Store(LLVMModule)
                    {
                        Type = strHdr.Type,
                        Value = "%" + TempCount,
                        Pointer = strHdr.Result
                    };

                    WriteLine(2, store.EmitCode());
                    ++TempCount;
                }
            }
        }

        private void GenerateLocalVarDeclForFloat(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            if (!astLocalVariableDeclaration.IsConstant)
            {
                if (!astLocalVariableDeclaration.IsArray)
                {
                    Alloca a = new Alloca(LLVMModule)
                                {
                                    Result = "%l_" + astLocalVariableDeclaration.Name,
                                    Type = "float"
                                };

                    WriteLine(2, a.EmitCode());

                    if (astLocalVariableDeclaration.Initialization != null)
                    {
                        astLocalVariableDeclaration.Initialization.EmitCode(this);

                        Store s = new Store(LLVMModule)
                                    {
                                        Type = "i8",
                                        Value = "%" + TempCount,
                                        Pointer = a.Result
                                    };

                        if (astLocalVariableDeclaration.AssociatedType == "System.Single")
                            s.Type = "float";
                        WriteLine(2, s.EmitCode());

                        ++TempCount;
                    }
                }
            }
        }
        private void GenerateLocalVarDeclForInt32(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            if (!astLocalVariableDeclaration.IsConstant)
            {
                if (!astLocalVariableDeclaration.IsArray)
                {
                    Alloca a = new Alloca(LLVMModule)
                                {
                                    Result = "%l_" + astLocalVariableDeclaration.Name,
                                    Type = "i32"
                                };

                    WriteLine(2, a.EmitCode());

                    if (astLocalVariableDeclaration.Initialization != null)
                    {
                        astLocalVariableDeclaration.Initialization.EmitCode(this);

                        Store s = new Store(LLVMModule)
                                    {
                                        Type = "i8",
                                        Value = "%" + TempCount,
                                        Pointer = a.Result
                                    };

                        if (astLocalVariableDeclaration.AssociatedType == "System.Int32")
                            s.Type = "i32";
                        WriteLine(2, s.EmitCode());

                        ++TempCount;
                    }
                }
            }
        }
    }
}