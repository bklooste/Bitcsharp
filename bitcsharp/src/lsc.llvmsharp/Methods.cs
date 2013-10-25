using System.Collections;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public Hashtable ExternMethods = new Hashtable();
        public string FunctionReturnType;

        public AstStruct CurrentAstStruct;
        public AstClass CurrentAstClass;
        public AstMethod CurrentAstMethod;
        public AstConstructor CurrentAstConstructor;

        public AstType CurrentAstType
        {
            get { return (AstType)CurrentAstClass ?? CurrentAstStruct; }
            set
            {
                if (value is AstStruct)
                {
                    CurrentAstStruct = (AstStruct)value;
                    CurrentAstClass = null;
                }
                else if (value is AstClass)
                {
                    CurrentAstClass = (AstClass)value;
                    CurrentAstStruct = null;
                }
            }
        }

        private void EmitCodeForMethods()
        {
            EmitCodeMethodsForStructs();
            EmitCodeMethodsForClasses();
        }

        private void EmitCodeMethodsForClasses()
        {
            foreach (string i in Compiler.ClassHashtable.Keys)
                EmitCodeMethodsForClasses((AstClass)Compiler.ClassHashtable[i]);
        }

        private void EmitCodeMethodsForStructs()
        {
            foreach (string i in Compiler.StructHashtable.Keys)
                EmitCodeMethodsForStruct((AstStruct)Compiler.StructHashtable[i]);
        }

        private void EmitCodeMethodsForStruct(AstStruct astStruct)
        {
            CurrentAstType = astStruct;
            foreach (string i in astStruct.methodTable.Keys)
            {
                object x = astStruct.methodTable[i];
                if (x is AstMethod)
                    EmitCodeForMethods(astStruct, (AstMethod)astStruct.methodTable[i], i);
            }
        }

        private void EmitCodeMethodsForClasses(AstClass astClass)
        {
            CurrentAstType = astClass;
            foreach (string i in astClass.methodTable.Keys)
            {
                object x = astClass.methodTable[i];
                if (x is AstMethod)
                    EmitCodeForMethods(astClass, (AstMethod)x, i);
                else if (x is AstConstructor)
                    EmitCodeForConstructors(astClass, (AstConstructor)x, i);
            }
        }

        private void EmitCodeForConstructors(AstType astType, AstConstructor astConstructor, string ctorKey)
        {
            CurrentAstConstructor = astConstructor;

            TempCount = 0;
            LoopCount = 0;
            FunctionReturnType = astConstructor.FullQName;

            bool isReturnTypeClass = Compiler.ClassHashtable.ContainsKey(FunctionReturnType);

            WriteInfoComment(" [ctor] ");
            WriteInfo(astType.FullQualifiedName);
            WriteInfo(" ");
            WriteInfo(ctorKey);
            WriteLine();

            Write(1, "define linkonce ");
            Write(LLVMTypeName(FunctionReturnType));
            if (isReturnTypeClass)
                Write("*");
            Write(" @__LS");
            Write(Mangler.Instance.MangleName(astType.FullQualifiedName));
            Write(ctorKey);
            Write("(");

            int i = 0;
            //if (!astConstructor.AstMemberModifierCollection.IsStatic)
            //{
            //    Write(LLVMTypeName(astType.FullQualifiedName));
            //    Write(" %this");
            //    ++i;
            //}

            for (; i < astConstructor.Parameters.Count; i++)
            {
                if (i != 0)
                    Write(", ");
                AstParameter param = (AstParameter)astConstructor.Parameters[i];
                Write(LLVMTypeName(param.FullQualifiedType));
                if (isReturnTypeClass)
                    Write("*");
                Write(" %p");
                Write(i.ToString());
            }

            Write(") nounwind ");

            Write("{");
            WriteLine();

            #region Block codegen
            #region Entry Block
            Label entryLabel = new Label(LLVMModule) { Name = "entry" };
            WriteLine(1, entryLabel.EmitCode());

            if (FunctionReturnType != "System.Void") // then allocate return value
                EmitCodeForReturnVariable(FunctionReturnType, isReturnTypeClass);
            #endregion

            #region pre-ctor
            if (Compiler.ClassHashtable.ContainsKey(FunctionReturnType))
                GeneratePreCtorForClasses(astConstructor);
            #endregion

            if (astConstructor.AstBlock != null)
                astConstructor.AstBlock.EmitCode(this);

            #region Return Block

            Label returnLabel = new Label(LLVMModule) { Name = "return" };
            UnConditionalBranch ub = new UnConditionalBranch(LLVMModule) { Destination = returnLabel.Name };
            WriteLine(2, ub.EmitCode());
            WriteLine(1, returnLabel.EmitCode());

            if (FunctionReturnType == "System.Void")
                WriteLine(2, new LLVM.ReturnVoid(LLVMModule).EmitCode());
            else
                EmitCodeForReturnLabelBody(FunctionReturnType, isReturnTypeClass);
            #endregion

            #endregion

            WriteLine(1, "}");
            WriteLine();

            CurrentAstConstructor = null;
        }

        private void GeneratePreCtorForClasses(AstConstructor astConstructor)
        {
            AstClass astClass = (AstClass)Compiler.ClassHashtable[astConstructor.FullQName];

            int parentClassTempCount = 0;
            if (astClass.AstParentClass != null)
            {
                GenerateCtorCall(astClass, astConstructor);
                parentClassTempCount = TempCount++;

                // Temporarily add to gc root and remove it after main gc_alloc for the current class
                Alloca allocaParentData = new Alloca(LLVMModule)
                                            {
                                                Result = "%" + TempCount,
                                                Type = LLVMTypeNamePtr(astClass.AstParentClass.FullQualifiedName)
                                            };
                WriteLine(2, allocaParentData.EmitCode());

                Store storeParentData = new Store(LLVMModule)
                            {
                                Type = allocaParentData.Type,
                                Value = "%" + parentClassTempCount,
                                Pointer = allocaParentData.Result
                            };
                WriteLine(2, storeParentData.EmitCode());
                ++TempCount;

                Alloca aparentRoot = new Alloca(LLVMModule)
                             {
                                 Result = "%" + TempCount,
                                 Type = "%struct.__llvmsharp_gc_root*"
                             };
                WriteLine(2, aparentRoot.EmitCode());
                ++TempCount;

                BitCast bcParent = new BitCast(LLVMModule)
                                       {
                                           Type1 = allocaParentData.Type + "*",
                                           Type2 = "i8*",
                                           Result = "%" + TempCount,
                                           Value = allocaParentData.Result
                                       };

                WriteLine(2, bcParent.EmitCode());

                Call markRoot = new Call(LLVMModule, 1)
                {
                    FunctionName = "@__llvmsharp_gc_markRootAndReturn",
                    ReturnType = "%struct.__llvmsharp_gc_root*",
                    Result = "%gcroot_p"
                };

                markRoot.Arguments[0] = "i8* " + bcParent.Result;
                ++TempCount;
                WriteLine(2, markRoot.EmitCode());
            }


            Call c = new Call(LLVMModule, 2)
            {
                FunctionName = "@__llvmsharp_gc_alloc",
                Result = "%" + TempCount,
                ReturnType = "i8*"
            };

            c.Arguments[0] = "i32 " + astClass.SizeOfObj;
            c.Arguments[1] = "i32 " + astClass.PtrCount;

            WriteLine(2, c.EmitCode());
            ++TempCount;

            if (astClass.AstParentClass != null)
            {
                Call unmarkRoot = new Call(LLVMModule, 1)
                {
                    ReturnType = "void",
                    FunctionName = "@__llvmsharp_gc_unmarkRoot"
                };
                unmarkRoot.Arguments[0] = "%struct.__llvmsharp_gc_root* %gcroot_p";

                WriteLine(2, unmarkRoot.EmitCode());
            }

            BitCast bc = new BitCast(LLVMModule)
                           {
                               Result = "%" + TempCount,
                               Type1 = "i8*",
                               Value = c.Result,
                               Type2 = LLVMTypeName(astConstructor.FullQName) + "*",
                           };
            WriteLine(2, bc.EmitCode());
            Store s = new Store(LLVMModule)
                        {
                            Type = bc.Type2,
                            Value = bc.Result,
                            Pointer = "%retval"
                        };
            WriteLine(2, s.EmitCode());
            ++TempCount;

            GenerateCtorInit(astClass, astConstructor, parentClassTempCount);
        }

        private void GenerateCtorCall(AstClass astClass, AstConstructor astConstructor)
        {
            if (astConstructor.AstConstructorCall == null) // call default ctor of parent
            {
                Call parentCtor = new Call(LLVMModule, 0)
                                      {
                                          FunctionName =
                                              "@" + PREFIX + Mangler.Instance.MangleName(astClass.AstParentClass.FullQualifiedName) + "_ct_" +
                                              Mangler.Instance.MangleName(((AstClass)Compiler.ClassHashtable[astClass.AstParentClass.FullQualifiedName]).Name),
                                          Result = "%" + TempCount,
                                          ReturnType = LLVMTypeNamePtr(astClass.AstParentClass.FullQualifiedName)
                                      };

                WriteLine(2, parentCtor.EmitCode());
            }
            else
            {
                astConstructor.AstConstructorCall.EmitCode(this);
            }
        }

        private void GenerateCtorInit(AstClass astClass, AstConstructor astConstructor, int parentClassTempCount)
        {
            Load l = new Load(LLVMModule)
                       {
                           Type = LLVMTypeName(astClass.FullQualifiedName) + "*",
                           Result = "%" + TempCount,
                           Pointer = "%retval"
                       };
            WriteLine(2, l.EmitCode());

            if (astClass.AstParentClass != null && astClass.FullQualifiedName != "System.String")
            {
                GetElementPtr gepParent = new GetElementPtr(LLVMModule)
                                            {
                                                PointerType = LLVMTypeName(astClass.FullQualifiedName),
                                                Indices = new ArrayList(2)
                                                              {
                                                                  new Index{ Idx = "0",Type = "i32"},
                                                                  new Index{Idx = "0",Type = "i32"}
                                                              },
                                                Result = "%" + (TempCount + 1),
                                                PointerValue = l.Result
                                            };
                WriteLine(2, gepParent.EmitCode());

                Store storeParent = new Store(LLVMModule)
                             {
                                 Value = "%" + parentClassTempCount,
                                 Pointer = gepParent.Result,
                                 Type = LLVMTypeName(astClass.AstParentClass.FullQualifiedName) + "*"
                             };
                Write(2, storeParent.EmitCode());
                WriteInfoCommentLine(2, "store parent");
                ++TempCount;
            }

            GetElementPtr ptr = new GetElementPtr(LLVMModule)
                                    {
                                        PointerType = LLVMTypeName(astClass.FullQualifiedName),
                                        PointerValue = l.Result
                                    };
            Store s = new Store(LLVMModule);

            foreach (AstField astField in astClass.ObjectLayout.Values)
            {
                ptr.Result = "%" + (TempCount + 1);
                s.Pointer = ptr.Result;
                //WriteCommentLine(astField.Name + astField.Index);
                ptr.Indices = new ArrayList(2)
                                          {
                                              new Index {Idx = "0", Type = "i32"},
                                              new Index {Idx = astField.Index.ToString(), Type = "i32"}
                                          };
                Write(2, ptr.EmitCode());
                WriteInfoCommentLine(2, astField.Name);
                switch (astField.FullQualifiedType)
                {
                    case "System.Int32":
                        s.Value = "0";
                        s.Type = "i32";

                        WriteLine(2, s.EmitCode());
                        ++TempCount;
                        break;
                    case "System.Boolean":
                        s.Value = "0";
                        s.Type = "i8";

                        WriteLine(2, s.EmitCode());
                        ++TempCount;
                        break;
                    case "System.Single":
                        s.Value = "0.000000e+000";
                        s.Type = "float";

                        WriteLine(2, s.EmitCode());
                        ++TempCount;
                        break;
                    default:
                        if (Compiler.ClassHashtable.ContainsKey(astField.FullQualifiedType))
                        {
                            s.Value = "null";
                            s.Type = LLVMTypeName(astField.FullQualifiedType) + "*";
                            WriteLine(2, s.EmitCode());
                            ++TempCount;
                        }
                        else if (Compiler.StructHashtable.ContainsKey(astField.FullQualifiedType))
                        {
                            //todo: struct astField ctor init
                        }
                        //todo: enum astField ctor init
                        break;
                }
            }
            ++TempCount;
        }

        private void EmitCodeForMethods(AstType astType, AstMethod astMethod, string methodKey)
        {
            CurrentAstMethod = astMethod;
            if (astMethod.AstMemberModifierCollection.IsExtern)
            {
                if (!ExternMethods.ContainsKey(astMethod.Name))
                {
                    ExternMethods.Add(astMethod.Name, astMethod);
                }
            }
            else
            {
                TempCount = 0; //seems like if there is an entry label insturctions should start from 0 else 1.
                LoopCount = 0;
                FunctionReturnType = astMethod.FullQReturnType;

                bool isReturnTypeClass = Compiler.ClassHashtable.ContainsKey(FunctionReturnType);

                WriteInfoComment(" [method] ");
                WriteInfo(astType.FullQualifiedName);
                WriteInfo(".");
                WriteInfo(astMethod.Name);
                WriteInfo(" ");
                WriteInfo(methodKey);
                WriteLine();

                Write(1, "define linkonce ");
                Write(LLVMTypeName(astMethod.FullQReturnType));
                if (isReturnTypeClass)
                    Write("*");
                Write(" @__LS");
                Write(Mangler.Instance.MangleName(astType.FullQualifiedName));
                Write(methodKey);
                Write("(");

                int i = 0;
                if (!astMethod.AstMemberModifierCollection.IsStatic)
                {
                    Write(LLVMTypeName(astType.FullQualifiedName));
                    Write("* %this");
                    ++i;
                }

                for (int j = 0; j < astMethod.Parameters.Count; ++i, ++j)
                {
                    if (i != 0)
                        Write(", ");
                    AstParameter param = (AstParameter)astMethod.Parameters[j];
                    Write(LLVMTypeName(param.FullQualifiedType));
                    if (Compiler.ClassHashtable.ContainsKey(param.FullQualifiedType))
                        Write("*");
                    Write(" %p_");
                    Write(param.Name);
                }

                Write(") nounwind ");

                Write("{");
                WriteLine();

                #region Block codegen
                Label entryLabel = new Label(LLVMModule) { Name = "entry" };
                WriteLine(1, entryLabel.EmitCode());

                if (astMethod.FullQReturnType != "System.Void") // then allocate return value
                    EmitCodeForReturnVariable(FunctionReturnType, isReturnTypeClass);

                PrepareMethodParams(astMethod);

                WriteInfoCommentLine(3, "method init finsihed");

                astMethod.AstBlock.EmitCode(this);


                #region Return Block

                Label returnLabel = new Label(LLVMModule) { Name = "return" };
                UnConditionalBranch ub = new UnConditionalBranch(LLVMModule) { Destination = returnLabel.Name };
                WriteLine(2, ub.EmitCode());

                WriteLine(1, returnLabel.EmitCode());

                if (astMethod.FullQReturnType == "System.Void")
                    WriteLine(2, new LLVM.ReturnVoid(LLVMModule).EmitCode());
                else
                    EmitCodeForReturnLabelBody(FunctionReturnType, isReturnTypeClass);
                #endregion

                #endregion

                WriteLine(1, "}");
                WriteLine();
            }

            FunctionReturnType = "";
            CurrentAstMethod = null;
        }

        private void PrepareMethodParams(AstMethod astMethod)
        {
            int i = 0;
            if (!astMethod.AstMemberModifierCollection.IsStatic)
            {
                Alloca allocaThis = new Alloca(LLVMModule)
                                      {
                                          Result = "%this_addr",
                                          Type = LLVMTypeName(CurrentAstType.FullQualifiedName)
                                      };
                if (CurrentAstClass != null)
                    allocaThis.Type += "*";
                WriteLine(2, allocaThis.EmitCode());

                Store storeThis = new Store(LLVMModule)
                                    {
                                        Type = allocaThis.Type,
                                        Value = "%this",
                                        Pointer = allocaThis.Result
                                    };
                WriteLine(2, storeThis.EmitCode());
                ++i;
            }


            for (int j = 0; j < astMethod.Parameters.Count; ++i, ++j)
            {
                AstParameter param = (AstParameter)astMethod.Parameters[j];

                //if (Compiler.ClassHashtable.ContainsKey(param.FullQualifiedType))
                //    continue;

                Alloca allocaParam = new Alloca(LLVMModule)
                                         {
                                             Result = "%l_" + param.Name,
                                             Type = LLVMTypeNamePtr(param.FullQualifiedType)
                                         };
                WriteLine(2, allocaParam.EmitCode());
                Store storeParam = new Store(LLVMModule)
                                     {
                                         Type = allocaParam.Type,
                                         Value = "%p_" + param.Name,
                                         Pointer = allocaParam.Result
                                     };
                WriteLine(2, storeParam.EmitCode());
                //                Write(LLVMTypeName(param.FullQualifiedType));
                //                if (CurrentAstClass != null)
                //                    Write("*");
                //                Write(" %p_");
                //                Write(param.Name);
            }
        }

        private void EmitCodeForReturnLabelBody(string fqt, bool isReturnTypeClass)
        {
            Load load = new Load(LLVMModule)
                          {
                              Result = "%retval1",
                              Pointer = "%retval",
                              Type = LLVMTypeName(fqt)
                          };

            Return r = new Return(LLVMModule)
                         {
                             Value = "%retval1",
                             Type = LLVMTypeName(fqt)
                         };

            if (isReturnTypeClass)
            {
                load.Type += "*";
                r.Type += "*";
            }

            WriteLine(2, load.EmitCode());
            WriteLine(2, r.EmitCode());
        }

        private void EmitCodeForReturnVariable(string fqt, bool isReturnTypeClass)
        {
            Alloca retval = new Alloca(LLVMModule)
                              {
                                  Result = "%retval"
                              };
            if (fqt == "System.Int32")
                retval.Type = "i32";
            else if (fqt == "System.Boolean")
                retval.Type = "i8";
            else
            {
                retval.Type = LLVMTypeName(fqt);
                if (isReturnTypeClass)
                    retval.Type += "*";
            }

            WriteLine(2, retval.EmitCode());

        }

        private void EmitCodeForExternMethods()
        {
            WriteInfoCommentLine(" Extern Methods");
            foreach (AstMethod externMethod in ExternMethods.Values)
                EmitCodeForExternMethods(externMethod);
        }

        private void EmitCodeForExternMethods(AstMethod astMethod)
        {

            Write(1, "declare ");

            Write(LLVMTypeName(astMethod.FullQReturnType));

            bool isReturnTypeClass = Compiler.ClassHashtable.ContainsKey(astMethod.FullQReturnType);

            if (isReturnTypeClass)
                Write("*");

            Write(" @");
            Write(astMethod.Name);

            Write("(");

            GenerateMethodTableParametersForExtern(astMethod);

            WriteLine(")");
        }

        private void GenerateMethodTableParametersForExtern(AstMethod astMethod)
        {
            for (int i = 0; i < astMethod.Parameters.Count; i++)
            {
                if (i != 0)
                    Write(", ");
                string fqt = ((AstParameter)astMethod.Parameters[i]).FullQualifiedType;
                Write(LLVMTypeName(fqt));
                if (Compiler.ClassHashtable.ContainsKey(fqt))
                    Write("*");
            }
        }

        private void EmitLLVMMethods()
        {
            WriteLine(1, "declare void @llvm.memcpy.i32(i8*, i8*, i32, i32) nounwind");
            WriteLine(1, "declare %struct.__llvmsharp_stringHeader* @__llvmsharp_System_String_ctor_charPtr(%struct.__llvmsharp_stringHeader* %value) nounwind");
            WriteLine(1, "declare signext i8 @__llvmsharp_String_compare(%struct.__llvmsharp_stringHeader* %strHdr1, %struct.__llvmsharp_stringHeader* %strHdr2) nounwind");
            WriteLine(1, "declare void @__llvmsharp_String_freeIfRequired(%struct.__llvmsharp_stringHeader* %strHdr) nounwind");
            WriteLine(1, "declare %struct.__llvmsharp_stringHeader* @__llvmsharp_System_String_concat(%struct.__llvmsharp_stringHeader* %strHdr1, %struct.__llvmsharp_stringHeader* %strHdr2) nounwind ");
            WriteLine(1, "declare i8* @__llvmsharp_gc_alloc(i32 %size, i32 %ptrCount) nounwind");
            WriteLine(1, "declare void @__llvmsharp_gc_markRoot(i8* %ptrRoot) nounwind");
            WriteLine(1, "declare %struct.__llvmsharp_gc_root* @__llvmsharp_gc_markRootAndReturn(i8* %ptrRoot) nounwind");
            WriteLine(1, "declare void @__llvmsharp_gc_unmarkRoot(%struct.__llvmsharp_gc_root* %gcRoot) nounwind ");

        }
    }
}