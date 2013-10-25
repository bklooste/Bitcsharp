using System;
using System.Collections;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {

        public override void EmitCode(AstSimpleAssignmentExpression astSimpleAssignmentExpression)
        {
            // todo astSimpleAssignmentExpression

            astSimpleAssignmentExpression.RightOperand.EmitCode(this);
            int rightOpResult = TempCount++;

            switch (astSimpleAssignmentExpression.LeftOperand.AssociatedType)
            {
                case "System.Int32":
                    GenerateLeftOperandForSimpleAssignmentExpressionInt32(astSimpleAssignmentExpression, rightOpResult);
                    break;
                case "System.Boolean":
                    GenerateLeftOperandForSimpleAssignmentExpressionBoolean(astSimpleAssignmentExpression, rightOpResult);
                    break;
                case "System.Single":
                    GenerateLeftOperandForSimpleAssignmentExpressionFloat(astSimpleAssignmentExpression, rightOpResult);
                    break;
                case "System.String":
                    GenerateLeftOperandForSimpleAssignmentExpressionString(astSimpleAssignmentExpression, rightOpResult);
                    break;
                default:
                    GenerateLeftOperandForSimpleassignmentExpressionUnknown(astSimpleAssignmentExpression, rightOpResult);
                    break;
            }

        }

        private void GenerateLeftOperandForSimpleassignmentExpressionUnknown(AstSimpleAssignmentExpression astSimpleAssignmentExpression, int rightOpResult)
        {
            AstVariableReference v = (AstVariableReference)astSimpleAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Load loadRoot = new Load(LLVMModule)
                {
                    Type = "%struct.__llvmsharp_gc_root*",
                    Result = "%" + TempCount,
                    Pointer = "%lgcroot_" + v.VariableName
                };
                WriteLine(2, loadRoot.EmitCode());

                Call unmarkRoot = new Call(LLVMModule, 1)
                {
                    ReturnType = "void",
                    FunctionName = "@__llvmsharp_gc_unmarkRoot"
                };
                unmarkRoot.Arguments[0] = "%struct.__llvmsharp_gc_root* %" + TempCount;

                WriteLine(2, unmarkRoot.EmitCode());

                Store storeVal = new Store(LLVMModule)
                                   {
                                       Type = LLVMTypeNamePtr(v.AssociatedType),
                                       Value = "%" + rightOpResult,
                                       Pointer = "%l_" + v.VariableName
                                   };
                WriteLine(2, storeVal.EmitCode());
                ++TempCount;

                BitCast bcLvar = new BitCast(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Value = storeVal.Pointer,
                    Type1 = storeVal.Type + "*",
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
                    Pointer = "%lgcroot_" + v.VariableName
                };
                WriteLine(2, storeRoot.EmitCode());
            }
            else
            {
                LoadAdr(v.MemberRefCollection);

                Store storeVal = new Store(LLVMModule)
                {
                    Type = LLVMTypeNamePtr(v.AssociatedType),
                    Value = "%" + rightOpResult,
                    Pointer = "%" + (TempCount - 1)
                };
                WriteLine(2, storeVal.EmitCode());
            }
        }

        private void GenerateLeftOperandForSimpleAssignmentExpressionString(AstSimpleAssignmentExpression astSimpleAssignmentExpression, int rightOpResult)
        {
            AstVariableReference v = (AstVariableReference)astSimpleAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Load loadOldHdr = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "%struct.__llvmsharp_stringHeader*",
                    Pointer = "%l_" + v.VariableName
                };
                WriteLine(2, loadOldHdr.EmitCode());

                Call freeIfRequired = new Call(LLVMModule, 1)
                {
                    FunctionName = "@__llvmsharp_String_freeIfRequired",
                    ReturnType = "void",
                };
                freeIfRequired.Arguments[0] = "%struct.__llvmsharp_stringHeader* " + loadOldHdr.Result;
                WriteLine(2, freeIfRequired.EmitCode());

                if (astSimpleAssignmentExpression.RightOperand is AstNull)
                {
                    Store storeNull = new Store(LLVMModule)
                    {
                        Type = LLVMTypeName("System.String") + "*",
                        Value = "null",
                        Pointer = "%l_" + v.VariableName
                    };
                    WriteLine(2, storeNull.EmitCode());
                }
                else
                {
                    Store s = new Store(LLVMModule)
                                  {
                                      Value = "%" + rightOpResult,
                                      Pointer = "%l_" + v.VariableName,
                                      Type = LLVMTypeName("System.String") + "*"
                                  };
                    WriteLine(2, s.EmitCode());
                }
            }
            else // if memberref
            {
                LoadAdr(v.MemberRefCollection);

                Call freeIfRequired = new Call(LLVMModule, 1)
                {
                    FunctionName = "@__llvmsharp_String_freeIfRequired",
                    ReturnType = "void",
                };
                freeIfRequired.Arguments[0] = "%struct.__llvmsharp_stringHeader* %" + TempCount;
                WriteLine(2, freeIfRequired.EmitCode());

                if (astSimpleAssignmentExpression.RightOperand is AstNull)
                {
                    Store storeNull = new Store(LLVMModule)
                    {
                        Type = LLVMTypeName("System.String") + "*",
                        Value = "null",
                        Pointer = "%" + (TempCount - 1)
                    };
                    WriteLine(2, storeNull.EmitCode());
                }
                else
                {
                    Store s = new Store(LLVMModule)
                                  {
                                      Value = "%" + rightOpResult,
                                      Pointer = "%" + (TempCount - 1),
                                      Type = LLVMTypeName("System.String") + "*"
                                  };
                    WriteLine(2, s.EmitCode());
                }

            }
        }

        private void GenerateLeftOperandForSimpleAssignmentExpressionFloat(AstSimpleAssignmentExpression astSimpleAssignmentExpression, int rightOpResult)
        {

            AstVariableReference v = (AstVariableReference)astSimpleAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                              {
                                  Value = "%" + rightOpResult,
                                  Pointer = "%l_" + v.VariableName
                              };

                if (astSimpleAssignmentExpression.LeftOperand.AssociatedType == "System.Single")
                    s.Type = "float";

                WriteLine(2, s.EmitCode());

                Load l = new Load(LLVMModule)
                             {
                                 Result = "%" + (TempCount),
                                 Type = "float",
                                 //since we already know this is going to be integer
                                 Pointer = "%l_" + v.VariableName
                             };

                WriteLine(2, l.EmitCode());
            }
            if (v.MemberRefCollection.Count > 0)
            {
                LoadAdr(v.MemberRefCollection);

                Alloca a = new Alloca(LLVMModule)
                               {
                                   Type = "float*",
                                   Result = "%" + (TempCount + 1)
                               };
                WriteLine(2, a.EmitCode());

                Store s = new Store(LLVMModule)
                              {
                                  Value = "%" + (TempCount - 1),
                                  Pointer = "%" + (TempCount + 1),
                                  Type = "float*"
                              };
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load loadPtr = new Load(LLVMModule)
                                   {
                                       Result = "%" + (TempCount + 1),
                                       Type = "float*",
                                       Pointer = a.Result
                                   };
                WriteLine(2, loadPtr.EmitCode());
                ++TempCount;

                Store storeActual = new Store(LLVMModule)
                                        {
                                            Value = "%" + rightOpResult,
                                            Pointer = loadPtr.Result,
                                            Type = "float"
                                        };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForSimpleAssignmentExpressionBoolean(AstSimpleAssignmentExpression astSimpleAssignmentExpression, int rightOpResult)
        {
            AstVariableReference v = (AstVariableReference)astSimpleAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                {
                    Value = "%" + rightOpResult,
                    Pointer = "%l_" + v.VariableName
                };

                if (astSimpleAssignmentExpression.LeftOperand.AssociatedType == "System.Boolean")
                    s.Type = "i8";

                WriteLine(2, s.EmitCode());

                Load l = new Load(LLVMModule)
                {
                    Result = "%" + (TempCount),
                    Type = "i8",
                    //since we already know this is going to be integer
                    Pointer = "%l_" + v.VariableName
                };

                WriteLine(2, l.EmitCode());
            }
            if (v.MemberRefCollection.Count > 0)
            {
                LoadAdr(v.MemberRefCollection);

                Alloca a = new Alloca(LLVMModule)
                {
                    Type = "i8*",
                    Result = "%" + (TempCount + 1)
                };
                WriteLine(2, a.EmitCode());

                Store s = new Store(LLVMModule)
                {
                    Value = "%" + (TempCount - 1),
                    Pointer = "%" + (TempCount + 1),
                    Type = "i8*"
                };
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load loadPtr = new Load(LLVMModule)
                {
                    Result = "%" + (TempCount + 1),
                    Type = "i8*",
                    Pointer = a.Result
                };
                WriteLine(2, loadPtr.EmitCode());
                ++TempCount;

                Store storeActual = new Store(LLVMModule)
                {
                    Value = "%" + rightOpResult,
                    Pointer = loadPtr.Result,
                    Type = "i8"
                };
                WriteLine(2, storeActual.EmitCode());

            }
        }

        private void GenerateLeftOperandForSimpleAssignmentExpressionInt32(AstSimpleAssignmentExpression astSimpleAssignmentExpression, int rightOpResult)
        {
            AstVariableReference v = (AstVariableReference)astSimpleAssignmentExpression.LeftOperand;

            if (v.MemberRefCollection.Count == 0)
            {
                Store s = new Store(LLVMModule)
                              {
                                  Value = "%" + rightOpResult,
                                  Pointer = "%l_" + v.VariableName
                              };

                if (astSimpleAssignmentExpression.LeftOperand.AssociatedType == "System.Int32")
                    s.Type = "i32";

                WriteLine(2, s.EmitCode());

                Load l = new Load(LLVMModule)
                             {
                                 Result = "%" + (TempCount),
                                 Type = "i32",
                                 //since we already know this is going to be integer
                                 Pointer = "%l_" + v.VariableName
                             };

                WriteLine(2, l.EmitCode());
            }
            if (v.MemberRefCollection.Count > 0)
            {
                LoadAdr(v.MemberRefCollection);

                //	store i32 0, i32* %1, align 4

                Alloca a = new Alloca(LLVMModule)
                             {
                                 Type = "i32*",
                                 Result = "%" + (TempCount + 1)
                             };
                WriteLine(2, a.EmitCode());

                Store s = new Store(LLVMModule)
                              {
                                  Value = "%" + (TempCount - 1),
                                  Pointer = "%" + (TempCount + 1),
                                  Type = "i32*"
                              };
                WriteLine(2, s.EmitCode());
                ++TempCount;

                Load loadPtr = new Load(LLVMModule)
                                 {
                                     Result = "%" + (TempCount + 1),
                                     Type = "i32*",
                                     Pointer = a.Result
                                 };
                WriteLine(2, loadPtr.EmitCode());
                ++TempCount;

                Store storeActual = new Store(LLVMModule)
                              {
                                  Value = "%" + rightOpResult,
                                  Pointer = loadPtr.Result,
                                  Type = "i32"
                              };
                WriteLine(2, storeActual.EmitCode());



                // todo load
                //Load l = new Load(LLVMModule)
                //{
                //    Result = "%" + (TempCount),
                //    Type = "i32",
                //    //since we already know this is going to be integer
                //    Pointer = "%l_" + v.VariableName
                //};
            }

        }

        private void LoadAdr(MemberRefCollection memberRefCollection)
        {
            object firstMember = memberRefCollection[0]; string TempType = "";
            string type = "";
            if (firstMember is AstLocalVariableDeclaration)
            {
                AstLocalVariableDeclaration lvar = (AstLocalVariableDeclaration)firstMember;
                type = lvar.FullQualifiedType;
                Load loadLVar = new Load(LLVMModule)
                                    {
                                        Type = LLVMTypeName(lvar.FullQualifiedType),
                                        Pointer = "%l_" + lvar.Name,
                                        Result = "%" + TempCount
                                    };
                if (Compiler.ClassHashtable.ContainsKey(type))
                    loadLVar.Type += "*";
                WriteLine(2, loadLVar.EmitCode());
            }
            else if (firstMember is AstField)
            {
                AstField fvar = (AstField)firstMember;
                type = fvar.FullQualifiedType;

                string currentAstType = LLVMTypeNamePtr(CurrentAstType.FullQualifiedName);
                Load loadThisPtr = new Load(LLVMModule)
                                       {
                                           Type = currentAstType,
                                           Result = "%" + TempCount,
                                           Pointer = "%this_addr"
                                       };
                WriteLine(2, loadThisPtr.EmitCode());

                GetElementPtr gep = new GetElementPtr(LLVMModule)
                {
                    PointerType = LLVMTypeName(CurrentAstType.FullQualifiedName),
                    PointerValue = "%" + TempCount,
                    Indices = new ArrayList(2),
                    Result = "%" + (TempCount + 1)
                };
                gep.Indices.Add(new Index { Type = "i32", Idx = "0" });
                gep.Indices.Add(new Index { Type = "i32", Idx = fvar.Index.ToString() });
                TempType = fvar.FullQualifiedType;
                WriteLine(2, gep.EmitCode());
                ++TempCount;

                type = fvar.FullQualifiedType;
                Load loadLVar = new Load(LLVMModule)
                {
                    Type = LLVMTypeName(fvar.FullQualifiedType),
                    Pointer = gep.Result,
                    Result = "%" + (TempCount + 1)
                };
                if (Compiler.ClassHashtable.ContainsKey(type))
                    loadLVar.Type += "*";
                WriteLine(2, loadLVar.EmitCode());
                ++TempCount;
            }
            else
            {
                // todo other load vars: fields and accessors
            }
            for (int i = 1; i < memberRefCollection.Count; i++)
            {
                bool load = false;
                object memRef = memberRefCollection[i];
                if (memRef is AstField)
                {
                    AstField cMemRef = (AstField)memRef;
                    GetElementPtr gep = new GetElementPtr(LLVMModule)
                                          {
                                              PointerType = LLVMTypeName(type),
                                              PointerValue = "%" + TempCount,
                                              Indices = new ArrayList(2),
                                              Result = "%" + (TempCount + 1)
                                          };
                    gep.Indices.Add(new Index { Type = "i32", Idx = "0" });
                    gep.Indices.Add(new Index { Type = "i32", Idx = cMemRef.Index.ToString() });
                    TempType = cMemRef.FullQualifiedType;
                    WriteLine(2, gep.EmitCode());
                    ++TempCount;
                    load = true;
                }
                else
                {
                    // todo: accessors
                    load = false;
                }

                if (load)
                {
                    Load l = new Load(LLVMModule)
                                 {
                                     Type = LLVMTypeName(TempType),
                                     Pointer = "%" + TempCount,
                                     Result = "%" + (TempCount + 1)
                                 };
                    if (Compiler.ClassHashtable.ContainsKey(TempType))
                        l.Type += "*";
                    WriteLine(2, l.EmitCode());
                    ++TempCount;
                }
            }
        }
    }
}