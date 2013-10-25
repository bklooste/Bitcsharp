/*using LLVMSharp.Compiler.Ast;
using System.Collections;

namespace LLVMSharp.Compiler.LLVM
{
    public partial class LLVMCodeGenerator : CodeGenerator
    {
       /* public Hashtable ExternMethods = new Hashtable();

        public override void GenerateMethodTable()
        {
            GenerateMethodTableForClasses();
        }

        protected virtual void GenerateMethodTableForClasses()
        {
            foreach (string item in Compiler.ClassHashtable.Keys)
                GenerateMethodTableForClasses((AstClass)Compiler.ClassHashtable[item]);
        }

        protected virtual void GenerateMethodTableForClasses(AstClass astClass)
        {
            foreach (string item in astClass.methodTable.Keys)
            {
                GenerateMethodTable((AstType)astClass, astClass.methodTable[item], item);
            }
        }

        public void GenerateMethodTable(AstType astType, object p, string methodKey)
        {
            if (p is AstMethod)
            {
                GenerateMethodTable(astType, (AstMethod)p, methodKey);
                WriteLine();
                WriteLine();
            }
        }

        public void GenerateMethodTable(AstType astType, AstMethod astMethod, string methodKey)
        {
            WriteInfoComment(" [method] ");
            WriteInfo(astType.FullQualifiedName);
            WriteInfo(".");
            WriteInfo(astMethod.Name);
            WriteInfo(" ");
            WriteInfo(methodKey);
            WriteLine();

            if (astMethod.AstMemberModifierCollection.IsExtern)
            {
                if (!ExternMethods.ContainsKey(astMethod.Name))
                {
                    ExternMethods.Add(astMethod.Name, astMethod);
                    GenerateMethodTableForExtern(astType, astMethod, methodKey);
                }
            }
            else
            {
                Write(1, "define linkonce ");
                Write(LLVMTypeName(astMethod.FullQReturnType));
                Write(" @__LS");
                Write(Mangler.Instance.MangleName(astType.FullQualifiedName));
                Write("_");
                Write(methodKey);
                Write("(");

                Write(LLVMTypeName(astType.FullQualifiedName));
                Write(" %this");

                for (int i = 0; i < astMethod.Parameters.Count; i++)
                {
                    Write(", ");
                    AstParameter param = (AstParameter)astMethod.Parameters[i];
                    Write(LLVMTypeName(param.FullQualifiedType));
                    if (Compiler.ClassHashtable.ContainsKey(param.FullQualifiedType))
                        Write("*");
                    Write(" %p");
                    Write(i.ToString());
                }

                Write(")");

                Write("{");
                WriteLine();

                GenerateBody(astType, astMethod, methodKey);

                Write(1, "}");


            }
        }

        private void GenerateMethodTableForExtern(AstType astType, AstMethod astMethod, string methodKey)
        {
            Write("define ");

            Write(LLVMTypeName(astMethod.FullQReturnType));

            Write(" ");
            Write(astMethod.Name);

            Write("(");

            GenerateMethodTableParametersForExtern(astType, astMethod, methodKey);

            Write(");");
        }

        private void GenerateMethodTableParametersForExtern(AstType astType, AstMethod astMethod, string methodKey)
        {
            for (int i = 0; i < astMethod.Parameters.Count; i++)
            {
                if (i != 0)
                    Write(", ");
                Write(LLVMTypeName(((AstParameter)astMethod.Parameters[i]).FullQualifiedType));
            }
        }♥1♥
    }
}*/