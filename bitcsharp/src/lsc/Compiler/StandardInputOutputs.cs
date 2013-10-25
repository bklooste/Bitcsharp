using System.IO;

namespace LLVMSharp.Compiler
{
    public delegate void StandardInputDelegate(object sender, StreamWriter inputWriter);

    public delegate void StandardBinaryInputDelegate(object sender, BinaryWriter inputWriter);

    public delegate void StandardBinaryOutputDelegate(object sender, BinaryReader outputReder);

    public delegate void StandardOutputDelegate(object sender, StreamReader outputReder);
}