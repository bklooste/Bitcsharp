using System.Diagnostics;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public abstract partial class CodeGenerator
    {
        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfo(string str)
        {
            Write(str);
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfo(int no)
        {
            Write(no);
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfoLine(string str)
        {
            WriteLine(str);
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfoLine(int no)
        {
            WriteLine(no);
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfo(int indentTimes, string str)
        {
            WriteIndentSpace(indentTimes);
            Write(str);
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfoLine(int indentTimes, string str)
        {
            Write(indentTimes, str);
            WriteLine();
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfoComment(string comment)
        {
            WriteComment(comment);
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfoCommentLine(string comment)
        {
            WriteCommentLine(comment);
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfoComment(int indentTimes, string comment)
        {
            WriteIndentSpace(indentTimes);
            WriteComment(comment);
        }

        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public virtual void WriteInfoCommentLine(int indentTimes, string comment)
        {
            WriteIndentSpace(indentTimes);
            WriteCommentLine(comment);
        }


        [DebuggerStepThrough]
        [Conditional("INFO_COMMENT")]
        public void WriteInfoCommentLine(int indentTimes, int no)
        {
            WriteIndentSpace(indentTimes);
            WriteCommentLine(no);
        }

        //#endregion

        //public virtual void Write(string str)
        //{
        //    Writer.Write(str);
        //}

        //public virtual void WriteLine(string str)
        //{
        //    Writer.WriteLine(str);
        //}

        //public virtual void WriteLine()
        //{
        //    Writer.WriteLine();
        //}

        //public virtual void Write(int identTimes, string str)
        //{
        //    WriteIdentSpace(identTimes);
        //    Write(str);
        //}

        //public virtual void WriteLine(int identTimes, string str)
        //{
        //    Write(identTimes, str);
        //    WriteLine();
        //}

        //public virtual void WriteIdentSpace()
        //{
        //    Write(INDENT_SPACE);
        //}

        //public virtual void WriteIdentSpace(int times)
        //{
        //    for (int i = 0; i < times; i++)
        //        WriteIdentSpace();
        //}

        //#endregion

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
