using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstMethodCall astMethodCall)
        {
            AstClass cMethodOwner = null;
            AstStruct sMethodOwner = null;
            AstEnum eMethodOwner = null;

            // at first assume that the method owner is the current class it self
            cMethodOwner = CurrentAstClass;
            sMethodOwner = CurrentAstStruct;

            int thisAddr = 0;
            string fqt = string.Empty;


            foreach (object memRef in astMethodCall.MemberRefCollection)
            {
                if (memRef is AstMethod)
                {
                    GenerateMethodCallMemberRef(astMethodCall, cMethodOwner, sMethodOwner, eMethodOwner, (AstMethod)memRef, thisAddr, fqt);
                }
                else if (memRef is AstClass)
                {
                    sMethodOwner = null;
                    eMethodOwner = null;

                    cMethodOwner = (AstClass)memRef;
                }
                else if (memRef is AstStruct)
                {
                    cMethodOwner = null;
                    eMethodOwner = null;

                    sMethodOwner = (AstStruct)memRef;
                }
                else if (memRef is AstLocalVariableDeclaration)
                {
                    fqt = LoadAddrForMethods((AstLocalVariableDeclaration)memRef);
                    thisAddr = TempCount++;

                    sMethodOwner = null;
                    eMethodOwner = null;
                    cMethodOwner = null;

                    if (Compiler.ClassHashtable.ContainsKey(fqt))
                        cMethodOwner = (AstClass)Compiler.ClassHashtable[fqt];
                    else if (Compiler.StructHashtable.ContainsKey(fqt))
                        sMethodOwner = (AstStruct)Compiler.StructHashtable[fqt];
                }
                else if (memRef is AstField)
                {
                    sMethodOwner = null;
                    eMethodOwner = null;
                    cMethodOwner = null;

                    fqt = ((AstField)memRef).FullQualifiedType;
                    if (Compiler.ClassHashtable.ContainsKey(fqt))
                        cMethodOwner = (AstClass)Compiler.ClassHashtable[fqt];
                    else if (Compiler.StructHashtable.ContainsKey(fqt))
                        sMethodOwner = (AstStruct)Compiler.StructHashtable[fqt];

                    LoadAdr(astMethodCall.MemberRefCollection);

                    thisAddr = TempCount;
                }
            }
        }

        private void LoadAddrForMethods(AstField astField, int TempCount)
        {

        }

        private string LoadAddrForMethods(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            Load load = new Load(LLVMModule)
                            {
                                Type = LLVMTypeNamePtr(astLocalVariableDeclaration.FullQualifiedType),
                                Pointer = "%l_" + astLocalVariableDeclaration.Name,
                                Result = "%" + TempCount
                            };
            WriteLine(2, load.EmitCode());
            return astLocalVariableDeclaration.FullQualifiedType;
        }

        private void GenerateMethodCallMemberRef(AstMethodCall astMethodCall, AstClass cMethodOwner, AstStruct sMethodOwner, AstEnum eMethodOwner, AstMethod astMethod, int thisAddr, string fqtAddr)
        {
            if (astMethod.AstMemberModifierCollection.IsExtern)
                GenerateExternMethodCall(astMethodCall, cMethodOwner, sMethodOwner, eMethodOwner, astMethod);
            else if (astMethod.AstMemberModifierCollection.IsStatic)
                GenerateStaticMethodCall(astMethodCall, cMethodOwner, sMethodOwner, eMethodOwner, astMethod);
            else // non static
                GenerateNonStaticMethodCall(astMethodCall, cMethodOwner, sMethodOwner, eMethodOwner, astMethod, thisAddr, fqtAddr);
        }

        private void GenerateNonStaticMethodCall(AstMethodCall astMethodCall, AstClass cMethodOwner, AstStruct sMethodOwner, AstEnum eMethodOwner, AstMethod astMethod, int thisAddr, string fqtAddr)
        {
            Call call = new Call(LLVMModule, astMethodCall.ArgumentCollection.Count + 1) // +1 required for this pointer
                            {
                                ReturnType = LLVMTypeName(astMethod.FullQReturnType),
                                FunctionName = "@" + PREFIX
                            };

            call.FunctionName = GetMethodKey(cMethodOwner, sMethodOwner, eMethodOwner, astMethod);

            call.Arguments[0] = LLVMTypeNamePtr(fqtAddr) + " %" + thisAddr;

            GenerateArguments(astMethodCall, ref call.Arguments, 1);

            WriteLine(2, call.EmitCode());
        }

        private void GenerateStaticMethodCall(AstMethodCall astMethodCall, AstClass cMethodOwner, AstStruct sMethodOwner, AstEnum eMethodOwner, AstMethod astMethod)
        {
            Call call = new Call(LLVMModule, astMethodCall.ArgumentCollection.Count)
                            {
                                ReturnType = LLVMTypeNamePtr(astMethod.FullQReturnType),
                                FunctionName = "@" + PREFIX,
                            };

            call.FunctionName = GetMethodKey(cMethodOwner, sMethodOwner, eMethodOwner, astMethod);

            GenerateArguments(astMethodCall, ref call.Arguments, 0);

            if (astMethod.ReturnType != "System.Void")
                call.Result = "%" + TempCount;

            WriteLine(2, call.EmitCode());
        }

        private string GetMethodKey(AstClass cMethodOwner, AstStruct sMethodOwner, AstEnum eMethodOwner, AstMethod astMethod)
        {
            string r = "@" + PREFIX;

            if (cMethodOwner != null)
            {
                r += Mangler.Instance.MangleName(cMethodOwner.FullQualifiedName) +
                    cMethodOwner.GetMethodKey(astMethod);
            }
            else if (sMethodOwner != null)
            {
                r += Mangler.Instance.MangleName(sMethodOwner.FullQualifiedName) +
                    sMethodOwner.GetMethodKey(astMethod);
            }

            return r;
        }

        private void GenerateExternMethodCall(AstMethodCall astMethodCall, AstClass cMethodOwner, AstStruct sMethodOwner, AstEnum eMethodOwner, AstMethod astMethod)
        {
            Call call = new Call(LLVMModule, astMethodCall.ArgumentCollection.Count)
                            {
                                ReturnType = LLVMTypeNamePtr(astMethodCall.AssociatedType),
                                FunctionName = "@" + astMethod.Name
                            };

            GenerateArguments(astMethodCall, ref call.Arguments, 0);

            if (astMethod.ReturnType != "System.Void")
                call.Result = "%" + TempCount;

            WriteLine(2, call.EmitCode());
        }

        #region Load Arguments

        private void GenerateArguments(AstMethodCall astMethodCall, ref string[] llvmCallArguments)
        {
            GenerateArguments(astMethodCall, ref llvmCallArguments, 0);
        }

        private void GenerateArguments(AstMethodCall astMethodCall, ref string[] llvmCallArguments, int start)
        {
            for (int i = start, j = 0; j < astMethodCall.ArgumentCollection.Count; ++i, ++j)
            {
                AstArgument arg = (AstArgument)astMethodCall.ArgumentCollection[j];

                arg.EmitCode(this);

                llvmCallArguments[i] = GenerateArgs(arg);

                ++TempCount;
            }
        }

        #endregion


        #region Old

        private void GenerateMethodCallMemberRefs(AstMethodCall astMethodCall)
        {
            AstMethod method =
                (AstMethod)astMethodCall.MemberRefCollection[astMethodCall.MemberRefCollection.Count - 1];

            int argsCount = astMethodCall.ArgumentCollection.Count;
            if (!method.AstMemberModifierCollection.IsStatic)
                ++argsCount;

            Call call = new Call(LLVMModule, argsCount)
                            {
                                ReturnType = LLVMTypeName(astMethodCall.AssociatedType),
                            };
            if (method.AstMemberModifierCollection.IsExtern)
                call.FunctionName = "@" + method.Name;
            else
            {
                call.FunctionName = "@" + PREFIX + Mangler.Instance.MangleName(CurrentAstType.FullQualifiedName);

                if (CurrentAstClass != null)
                    call.FunctionName += CurrentAstClass.GetMethodKey(method);
                else if (CurrentAstStruct != null)
                    call.FunctionName += CurrentAstStruct.GetMethodKey(method);

                if (astMethodCall.MemberRefCollection[0] is AstLocalVariableDeclaration)
                {
                    call.FunctionName = "@" + PREFIX +
                                        Mangler.Instance.MangleName(
                                            (astMethodCall.MemberRefCollection[0] as AstLocalVariableDeclaration).FullQualifiedType);
                }
                else if (astMethodCall.MemberRefCollection[0] is AstClass)
                {
                    call.FunctionName = "@" + PREFIX +
                                        Mangler.Instance.MangleName(
                                            (astMethodCall.MemberRefCollection[0] as AstClass).FullQualifiedName);
                }

            }

            LoadAddrForMethods(astMethodCall.MemberRefCollection);

            GenerateMethodCallMemberRef(astMethodCall, ref call.Arguments);

            WriteLine(2, call.EmitCode());
        }

        private void GenerateMethodCallMemberRef(AstMethodCall astMethodCall, ref string[] llvmCallArguments)
        {
            AstMethod method =
                (AstMethod)astMethodCall.MemberRefCollection[astMethodCall.MemberRefCollection.Count - 1];

            int thisPtrTempCount = TempCount++;

            if (!method.AstMemberModifierCollection.IsStatic)
            {
                llvmCallArguments[0] = LLVMTypeName(MethodCallThisType) + "* %" + thisPtrTempCount;
                GenerateArguments(astMethodCall, ref llvmCallArguments, 1);
            }
        }

        public string MethodCallThisType;
        private void LoadAddrForMethods(MemberRefCollection memberRefCollection)
        {
            object firstMember = memberRefCollection[0];
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
                MethodCallThisType = lvar.FullQualifiedType;
            }
            else
            {
                // todo other load vars: fields and accessors
            }
        }

        private void GenerateMetodCall1MemberRef(AstMethodCall astMethodCall)
        {
            AstMethod method = (AstMethod)astMethodCall.MemberRefCollection[0];

            Call call = new Call(LLVMModule, astMethodCall.ArgumentCollection.Count)
                                 {
                                     ReturnType = LLVMTypeName(astMethodCall.AssociatedType)
                                 };

            if (method.AstMemberModifierCollection.IsExtern)
                call.FunctionName = "@" + method.Name;
            else
                call.FunctionName = "@" + PREFIX + Mangler.Instance.MangleName(CurrentAstType.FullQualifiedName)
                                    + CurrentAstClass.GetMethodKey(method);

            GenerateArguments(astMethodCall, ref call.Arguments);

            WriteLine(2, call.EmitCode());
        }


        private string GenerateArgs(AstArgument arg)
        {
            if (arg.AssociatedType == "System.Int32")
                return GenerateInt32Args(arg);
            if (arg.AssociatedType == "System.String")
                return GenerateStringArgs(arg);
            if (arg.AssociatedType == "System.Single")
                return GenerateFloatArgs(arg);
            else
            {
                //todo generate args for class, enum, struct,bool
                return string.Empty;
            }
        }

        private string GenerateFloatArgs(AstArgument arg)
        {
            return "float %" + TempCount;
        }

        private string GenerateStringArgs(AstArgument arg)
        {
            return LLVMTypeName("System.String") + "* %" + TempCount;
        }

        private string GenerateInt32Args(AstArgument arg)
        {
            return "i32 %" + TempCount;
        }

        #endregion

    }
}