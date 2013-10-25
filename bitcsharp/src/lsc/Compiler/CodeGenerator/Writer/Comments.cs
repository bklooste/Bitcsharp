using System.Diagnostics;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public abstract partial class CodeGenerator
    {
        [DebuggerStepThrough]
        public abstract void WriteComment(string comment);

        [DebuggerStepThrough]
        public virtual void WriteCommentLine(string comment)
        {
            WriteComment(comment);
            Write(NewLine);
        }

        [DebuggerStepThrough]
        public virtual void WriteComment(int number)
        {
            WriteComment(number.ToString());
        }

        [DebuggerStepThrough]
        public virtual void WriteCommentLine(int number)
        {
            WriteCommentLine(number.ToString());
        }
        ////public virtual string MangaleNamespace(string ns)
        ////{
        ////    if (string.IsNullOrEmpty(ns))
        ////        return string.Empty;

        ////    string[] s = ns.Split('.');
        ////    StringBuilder sb = new StringBuilder();
        ////    foreach (string item in s)
        ////    {
        ////        sb.Append(item.Length);
        ////        sb.Append(item);
        ////    }
        ////    return sb.ToString();
        ////}

        ////public virtual string GenerateOutputNameForEnumMember(string ns, string memberName)
        ////{
        ////    StringBuilder sb = new StringBuilder();

        ////    sb.Append(PREFIX);
        ////    sb.Append("Ei32_");
        ////    sb.Append(Mangler.Instance.MangleName(ns));
        ////    sb.Append(memberName.Length);
        ////    sb.Append(memberName);

        ////    return sb.ToString();
        ////}

        ////#region Root Nodes

        ////public virtual void GenerateSourceFileCodes(AstSourceFile astSourceFile)
        ////{
        ////    foreach (AstType item in astSourceFile.AstTypeCollection)
        ////    {
        ////        GenerateAstTypeCodes(item);
        ////    }

        ////    foreach (AstNamespaceBlock item in astSourceFile.AstNamespaceBlockCollection)
        ////    {
        ////        GenerateAstNameSpaceBlockCodes(item);
        ////    }
        ////}

        ////private void GenerateAstNameSpaceBlockCodes(AstNamespaceBlock astNamespaceBlock)
        ////{
        ////    foreach (AstType item in astNamespaceBlock.AstTypeCollection)
        ////    {
        ////        GenerateAstTypeCodes(item);
        ////    }
        ////    foreach (AstNamespaceBlock item in astNamespaceBlock.AstNamespaceBlockCollection)
        ////    {
        ////        GenerateAstNameSpaceBlockCodes(item);
        ////    }
        ////}

        ////public virtual void GenerateAstTypeCodes(AstType astType)
        ////{
        ////    if (astType is AstClass)
        ////    {
        ////        Writer.WriteLine();
        ////        GenerateAstClassCodes((AstClass)astType);
        ////    }
        ////    else if (astType is AstEnum)
        ////    {
        ////        Writer.WriteLine();
        ////        GenerateAstEnumCodes((AstEnum)astType);
        ////    }
        ////    else if (astType is AstStruct)
        ////    {
        ////        Writer.WriteLine();
        ////        GenerateAstStructCodes((AstStruct)astType);
        ////    }
        ////}

        ////public abstract void GenerateAstStructCodes(AstStruct astStruct);
        ////public abstract void GenerateAstClassCodes(AstClass astClass);
        ////public abstract void GenerateAstEnumCodes(AstEnum astEnum);

        ////#endregion

        //public abstract void GenerateEnums(Hashtable enumHashTable);
    }
}
