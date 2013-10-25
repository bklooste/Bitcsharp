using System;
using System.IO;

namespace LLVMSharp.Compiler.CodeGenerators
{
    public abstract partial class CodeGenerator
    {
        public LLVMSharpCompiler Compiler;
        public StreamWriter Writer;
        public readonly string INDENT_SPACE = "     ";

        public string NewLine = Environment.NewLine;

        public CodeGenerator(StreamWriter writer)
        {
            Writer = writer;
        }

        public CodeGenerator(StreamWriter writer, string indentSpace)
            : this(writer)
        {
            INDENT_SPACE = indentSpace;
        }

        public abstract void EmitCode();


        //protected virtual void AstProgram_EmitCode(AstProgram astNode)
        //{
        //    WriteInfoCommentLine("Program");

        //    foreach (AstSourceFile i in astNode.SourceFiles)
        //        i.EmitCode(AstSourceFile_EmitCode);
        //}

        //protected virtual void AstSourceFile_EmitCode(AstSourceFile astSourceFile)
        //{

        //   /* foreach (AstType i in astSourceFile.AstTypeCollection)
        //        i.EmitCode();*/
        //    WriteInfoCommentLine(astSourceFile.FileName);
        //}

        /*  protected virtual void EmitCode(AstProgram astProgram)
          {
              foreach (AstSourceFile i in astProgram.SourceFiles)
                  EmitCode(i);
          }

          protected virtual void EmitCode(AstSourceFile astSourceFile)
          {
            /*  foreach (AstType i in astSourceFile.AstTypeCollection)
                  i.EmitCode();♥1♥
          }*/

        //public virtual void GenerateCode()
        //{
        //    //GenerateEntryPoint();
        //    //GenerateIntegerTable();
        //    //GenerateStringTableCodes();
        //    //GenerateEnums(Compiler.EnumHashtable);
        //    //GenerateObjectLayout();
        //    //GenerateMethodTable();
        //    //WriteLine();
        //}

        //public abstract void GenerateIntegerTable();

        //public abstract void GenerateStringTableCodes();

        //public abstract void GenerateMethodTable();

        //public abstract void GenerateEntryPoint();


        //public abstract void GenerateObjectLayout();

        //public string PREFIX = "_LS_";


        //#region Writer

        //#region Comments

        //public abstract void WriteComment(string comment);

        //public virtual void WriteCommentLine(string comment)
        //{
        //    WriteComment(comment);
        //    Write(Environment.NewLine);
        //}

        //public virtual void WriteCommentLine(int identTimes, string comment)
        //{
        //    WriteIdentSpace(identTimes);
        //    WriteCommentLine(comment);
        //}

        //public void WriteCommentLine(int identTimes, int comment)
        //{
        //    WriteCommentLine(identTimes, comment.ToString());
        //}

        //public virtual void WriteComment(int number)
        //{
        //    WriteComment(number.ToString());
        //}

        //#endregion

        //#region Debug Comments

        //[Conditional("DEBUG")]
        //public virtual void WriteDebugComment(string comment)
        //{
        //    WriteComment(comment);
        //}

        //[Conditional("COMMENTS_PATH")]
        //public virtual void WriteSourcePathLine(string path)
        //{
        //    WriteCommentLine(path);
        //}

        //[Conditional("COMMENTS_PATH")]
        //public virtual void WriteSourcePath(string path)
        //{
        //    WriteComment(path);
        //}

        //#endregion

        //#region InfoComment

        //[Conditional("INFO_COMMENT")]
        //public virtual void WriteInfo(string comment)
        //{
        //    Write(comment);
        //}

        //[Conditional("INFO_COMMENT")]
        //public virtual void WriteInfoLine(string comment)
        //{
        //    WriteLine(comment);
        //}

        //[Conditional("INFO_COMMENT")]
        //public virtual void WriteInfo(int identTimes, string str)
        //{
        //    WriteIdentSpace(identTimes);
        //    Write(str);
        //}

        //[Conditional("INFO_COMMENT")]
        //public virtual void WriteInfoLine(int identTimes, string str)
        //{
        //    Write(identTimes, str);
        //    WriteLine();
        //}

        //[Conditional("INFO_COMMENT")]
        //public virtual void WriteInfoComment(string comment)
        //{
        //    WriteComment(comment);
        //}

        //[Conditional("INFO_COMMENT")]
        //public virtual void WriteInfoCommentLine(string comment)
        //{
        //    WriteCommentLine(comment);
        //}

        //[Conditional("INFO_COMMENT")]
        //public virtual void WriteInfoComment(int identTimes, string comment)
        //{
        //    WriteIdentSpace(identTimes);
        //    WriteComment(comment);
        //}

        //[Conditional("INFO_COMMENT")]
        //public void WriteInfoCommentLine(int identTimes, int comment)
        //{
        //    WriteCommentLine(identTimes, comment.ToString());
        //}



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
