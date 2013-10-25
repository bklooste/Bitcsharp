using System.IO;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class Module
    {
        public StreamWriter Writer;

        public int TempCount;

        public Module(StreamWriter writer)
        {
            Writer = writer;
        }

        public VariableCollection VariableCollection = new VariableCollection();
    }
}
