namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        public string StringTablePrefix = "@.__ls_str";

        private void EmitCodeForStringTable()
        {
            if (Compiler.StringTable.Keys.Count > 0)
            {
                WriteInfoCommentLine(" String Table");
                WriteInfo("");

                foreach (string item in Compiler.StringTable.Keys)
                {
                    int length;
                    string tmp = LLVMString(item, out length);


                    Write(1, StringTablePrefix);
                    Write((Compiler.StringTable[item]).ToString());
                    Write(" = internal constant [");
                    Write(length);
                    Write(" x i8] c\"");
                    Write(tmp);
                    //Write(item);
                    //Write("\\00\"");
                    WriteLine("\"");
                }

                WriteLine();
            }
        }

        private string LLVMString(string str)
        {
            return str.Replace("\\\"", "\\22") + "\\00";
        }

        private string LLVMString(string str, out int length)
        {
            length = LLVMStringLength(str);
            return LLVMString(str);
        }

        private int LLVMStringLength(string str)
        {
            return str.Replace("\\\"", "1").Length + 1;
        }
    }
}