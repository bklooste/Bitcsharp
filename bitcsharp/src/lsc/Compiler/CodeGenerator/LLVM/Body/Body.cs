/*
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.LLVM
{
    public partial class LLVMCodeGenerator : CodeGenerator
    {
        private void GenerateBody(AstType astType, AstMethod astMethod, string methodKey)
        {
            GerateBlock(astMethod.AstBlock);
        }

        private void GerateBlock(AstBlock astBlock)
        {
            if (astBlock != null)
            {
                foreach (AstStatement item in astBlock.AstStatementCollection)
                {
                    GenerateAstStatement(item);
                }
            }
        }

        private void GenerateAstStatement(AstStatement item)
        {
            if (item is AstLocalVariableDeclaration)
                GenerateAstLocalVariableDeclaration((AstLocalVariableDeclaration)item);
        }

        private void GenerateAstLocalVariableDeclaration(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            if (astLocalVariableDeclaration.FullQualifiedType == "System.Int32")
            {
                GenerateAstLocalVariableDeclarationForInt32(astLocalVariableDeclaration);
            }
        }

        private void GenerateAstLocalVariableDeclarationForInt32(AstLocalVariableDeclaration astLocalVariableDeclaration)
        {
            if (!astLocalVariableDeclaration.IsConstant)
            {
                if (!astLocalVariableDeclaration.IsArray)
                {
                    Alloca a = new Alloca(this)
                                   {
                                       Result = "%_" + astLocalVariableDeclaration.Name,
                                       Type = "i32"
                                   };
                    WriteLine(2, a.GenerateCode());

                    if (astLocalVariableDeclaration.Initialization != null)
                    {
                        GenerateExpression(astLocalVariableDeclaration.Initialization);

                        Load l = new Load(this)
                                     {
                                         Result = "%" + (TempCount + 1),
                                         Type = "i32",
                                         Pointer = "%" + TempCount
                                     };

                        WriteLine(2, l.GenerateCode());

                        ++TempCount;

                        Store s = new Store(this)
                                      {
                                          Type = "i32",
                                          Value = "%" + TempCount,
                                          Pointer = "%_" + astLocalVariableDeclaration.Name
                                      };
                        WriteLine(2, s.GenerateCode());

                        ++TempCount;
                    }
                }
            }
        }

    }
}
*/
