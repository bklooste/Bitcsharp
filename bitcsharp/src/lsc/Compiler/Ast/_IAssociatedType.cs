using System;
using System.Collections;

namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// Incidactes that an <see cref="AstNode"/> is an expression.
    /// </summary>
    public interface IAssociatedType
    {
        // pg.127 Every value has an associated type.
        string AssociatedType { get; set; }
    }
}