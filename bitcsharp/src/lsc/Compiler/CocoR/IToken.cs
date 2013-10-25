
namespace LLVMSharp.Compiler.CocoR
{
    public interface IToken
    {
        int LineNumber { get; }
        int ColumnNumber { get; }
    }
}
