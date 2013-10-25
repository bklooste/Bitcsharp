namespace LLVMSharp.Compiler.CodeGenerators
{
    public partial class LLVMSharpCodeGenerator
    {
        private void EmitCodeForIntegerTable()
        {
            /*
             * todo
             * int c=1+1 doesnt come in integer table
             * but int c=1 comes up in int table
             */
            if (Compiler.IntegerTable.Keys.Count > 0)
            {
                WriteInfoCommentLine(" Integer Table");
                WriteInfo("");

                int i = 0;
                foreach (int item in Compiler.IntegerTable.Keys)
                {
                    Write(1, "@.__ls_int");
                    Write((i++).ToString());
                    Write(" = internal constant i32 ");
                    WriteLine(item.ToString());
                }

                WriteLine();
            }
        }
    }
}