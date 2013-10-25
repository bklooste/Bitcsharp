using System.Collections;
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        private void EmitCodeForObjectLayout(AstClass astClass)
        {
            if (astClass.FullQualifiedName == "System.String")
                return;

            WriteInfoComment(" class ");
            WriteInfo(astClass.FullQualifiedName);
            if (astClass.AstParentClass != null)
            {
                WriteInfo(" : ");
                WriteInfo(astClass.AstParentClass.FullQualifiedName);
            }
            WriteInfo(" { ");
            //                WriteSourcePath(astClass.Path);
            WriteLine();

            #region Actual ObjectLayout codegen

            EmitCodeForActualObjectLayout(astClass);

            #endregion

            WriteInfoCommentLine(" }");
            WriteLine();
        }

        private void EmitCodeForActualObjectLayout(AstClass astClass)
        {
            /*
             * todo check for struct and class pointers
             */
            Write(1, LLVMClassName(astClass));
            WriteLine(" = type { ");

            int i = 0;
            if(astClass.Name=="Node")
            {
                
            }

            if (astClass.AstParentClass != null)
            {
                Write(2, LLVMClassName(astClass.AstParentClass.FullQualifiedName));
                Write("*");
                if (astClass.AstFieldCollection.Count > 0)
                    Write(",");
                i++;
                WriteInfoComment(3, "parent class");
                WriteLine();
            }

            ArrayList nonClassFields = new ArrayList();
            foreach (AstField astField in astClass.AstFieldCollection)
            {
                if (astField.FullQualifiedType != "System.String" && Compiler.ClassHashtable.ContainsKey(astField.FullQualifiedType))
                {
                    Write(2, LLVMTypeName(astField.FullQualifiedType));
                    Write("*");
                    if (i < astClass.AstFieldCollection.Count)
                        Write(",");
                    astField.Index = i++;
                    WriteInfoComment(3, astField.Name);
                    WriteInfo(" [" + astField.Index + "]");
                    WriteLine();
                }
                else
                    nonClassFields.Add(astField);
            }
            astClass.PtrCount = i;

            foreach (AstField astField in nonClassFields)
            {
                Write(2, LLVMTypeName(astField.FullQualifiedType));
                if (astField.FullQualifiedType == "System.String")
                    Write("*");
                if (i < astClass.AstFieldCollection.Count)
                    Write(",");
                astField.Index = i++;
                WriteInfoComment(3, astField.Name);
                WriteInfo(" [" + astField.Index + "]");
                WriteLine();
            }

            WriteLine(1, "}");
        }
    }
}