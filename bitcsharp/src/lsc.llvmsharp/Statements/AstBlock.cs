using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators.LLVM;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public override void EmitCode(AstBlock astBlock)
        {
            astBlock.AstStatementCollection.EmitCode(this);

            if (astBlock != null)
            {
                foreach (AstLocalVariableDeclaration localVariableDeclaration in astBlock.AstLocalVarDeclarationCollection)
                {
                    // gc unmark root for classes
                    if (Compiler.ClassHashtable.ContainsKey(localVariableDeclaration.FullQualifiedType)
                        && localVariableDeclaration.FullQualifiedType != "System.String")
                    {
                        Load loadRoot = new Load(LLVMModule)
                                            {
                                                Type = "%struct.__llvmsharp_gc_root*",
                                                Result = "%" + TempCount,
                                                Pointer = "%lgcroot_" + localVariableDeclaration.Name
                                            };
                        WriteLine(2, loadRoot.EmitCode());

                        Call unmarkRoot = new Call(LLVMModule, 1)
                                              {
                                                  ReturnType = "void",
                                                  FunctionName = "@__llvmsharp_gc_unmarkRoot"
                                              };
                        unmarkRoot.Arguments[0] = "%struct.__llvmsharp_gc_root* %" + TempCount;

                        WriteLine(2, unmarkRoot.EmitCode());
                        ++TempCount;
                    }
                }
            }
        }
    }
}