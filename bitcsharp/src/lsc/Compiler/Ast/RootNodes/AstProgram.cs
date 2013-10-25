
using System;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// The root AST node for the source code(s).
    /// </summary>
    public class AstProgram : BaseMinimumAstNode, IEntryPoint, IRootWalker
    {
        public AstProgram()
        {
            SourceFiles = new AstSourceFileCollection();
        }

        /// <summary>
        /// Stores list of Files used in AstProgram.
        /// </summary>
        /// <remarks>
        /// You have to store it in even if it is not a file stream,
        /// it will be handled by <see cref="AstSourceFile"/> node which
        /// will assign the file as null.
        /// </remarks>
        public AstSourceFileCollection SourceFiles;

        public void CheckEntryPoint(LLVMSharpCompiler compiler)
        {
            foreach (AstSourceFile file in SourceFiles)
                file.CheckEntryPoint(compiler);
        }

        public void Walk(Walker walker)
        {
            walker.Walk();
        }
    }
}
