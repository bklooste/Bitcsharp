
namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// Incidactes that an <see cref="AstNode"/> is a Class or Struct Member.
    /// </summary>
    public interface IObjectMember
    {
        AstMemberModifierCollection AstMemberModifierCollection { get; set; }
    }
}