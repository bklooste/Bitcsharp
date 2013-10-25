using System;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstVariableReference astVariableReference)
        {
            if (astVariableReference.AssociatedType == "System.Int32")
                EmitCodeForInt32(astVariableReference);
            else if (astVariableReference.AssociatedType == "System.String")
                EmitCodeForString(astVariableReference);
            else if (astVariableReference.AssociatedType == "System.Boolean")
                EmitCodeForBool(astVariableReference);
            else if (astVariableReference.AssociatedType == "System.Single")
                EmitCodeForFloat(astVariableReference);
            else
            {
                if (Compiler.ClassHashtable.ContainsKey(astVariableReference.AssociatedType))
                    EmitCodeForClass(astVariableReference);
            }

        }

        private void EmitCodeForClass(AstVariableReference astVariableReference)
        {
            if (astVariableReference.MemberRefCollection.Count == 0)
            {
                Load l = new Load(LLVMModule)
                             {
                                 Result = "%" + TempCount,
                                 Type = LLVMTypeNamePtr(astVariableReference.AssociatedType),
                                 Pointer = "%l_" + astVariableReference.VariableName
                             };
                WriteLine(2, l.EmitCode());
            }
            else
            {
                LoadAdr(astVariableReference.MemberRefCollection);
            }
        }

        private void EmitCodeForFloat(AstVariableReference astVariableReference)
        {
            if (astVariableReference.MemberRefCollection.Count == 0)
            {
                Load l = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "float",
                    Pointer = "%l_" + astVariableReference.VariableName
                };
                WriteLine(2, l.EmitCode());
            }
            else
            {
                LoadAdr(astVariableReference.MemberRefCollection);
            }
        }

        private void EmitCodeForString(AstVariableReference astVariableReference)
        {
            if (astVariableReference.MemberRefCollection.Count == 0)
            {
                Load l = new Load(LLVMModule)
                           {
                               Result = "%" + TempCount,
                               Type = LLVMTypeName("System.String") + "*",
                               Pointer = "%l_" + astVariableReference.VariableName
                           };
                WriteLine(2, l.EmitCode());
            }
            else
            {
                LoadAdr(astVariableReference.MemberRefCollection);
            }
        }

        private void EmitCodeForInt32(AstVariableReference astVariableReference)
        {
            if (astVariableReference.MemberRefCollection.Count == 0)
            {
                Load l = new Load(LLVMModule)
                           {
                               Result = "%" + TempCount,
                               Type = "i32",
                               Pointer = "%l_" + astVariableReference.VariableName
                           };
                WriteLine(2, l.EmitCode());
            }
            else
            {
                LoadAdr(astVariableReference.MemberRefCollection);
            }
        }

        private void EmitCodeForBool(AstVariableReference astVariableReference)
        {
            if (astVariableReference.MemberRefCollection.Count == 0)
            {
                Load l = new Load(LLVMModule)
                {
                    Result = "%" + TempCount,
                    Type = "i8",
                    Pointer = "%l_" + astVariableReference.VariableName
                };
                WriteLine(2, l.EmitCode());
            }
            else
            {
                LoadAdr(astVariableReference.MemberRefCollection);
            }
        }
    }
}