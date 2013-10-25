
namespace LLVMSharp.Compiler.CocoR
{
    public interface IParser
    {
        /// <summary>
        /// Last Recognized Token
        /// </summary>
        IToken Token { get; }
        IToken LookAhead { get; }
        IScanner Scanner { get; }
    }
}
