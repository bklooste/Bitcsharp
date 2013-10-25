using System;
using System.Collections;
using System.IO;
using System.Text;
using LLVMSharp.Compiler.CocoR;

namespace LLVMSharp.Compiler.Ast
{
    /// <summary>
    /// The node containing the source file information.
    /// </summary>
    public class AstSourceFile : BaseMinimumAstNode, IEntryPoint
    {
        /// <summary>
        /// FileName of the source file.
        /// </summary>
        /// <remarks>
        /// FileName is null if the Scanner stream is not a file stream.
        /// </remarks>
        public string FileName;

        public AstUsingDeclarativeCollection AstUsingDeclarativeCollection;
        public AstNamespaceBlockCollection AstNamespaceBlockCollection;
        public AstTypeCollection AstTypeCollection;

        public AstSourceFile(IParser parser)
            : base(parser)
        {
            FileName = string.IsNullOrEmpty(parser.Scanner.FileName) ? null :
                Path.GetFullPath(parser.Scanner.FileName); // get the full path so its easier to work with later on incase we need the file.
            AstUsingDeclarativeCollection = new AstUsingDeclarativeCollection();
            AstNamespaceBlockCollection = new AstNamespaceBlockCollection();
            AstTypeCollection = new AstTypeCollection();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstSourceFile--{0}{0}");
            if (string.IsNullOrEmpty(FileName))
                sb.Append("FileName: [No FileName - using streams]{0}");
            else
                sb.Append("FileName: {1}{0}");

            return string.Format(sb.ToString(), Environment.NewLine, FileName);
        }

        #region Helper Methods
        // that may be usefull

        /// <summary>
        /// Returns the file name and extension of the specified path string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> consisting of the characters after the last directory character
        /// in path. If the last character of path is a directory or volume separator
        /// character, this method returns <see cref="System.String.Empty"/>. If path is null, this
        /// method returns null.
        /// </returns>
        public string GetFileName()
        {
            return Path.GetFileName(FileName);
        }

        /// <summary>
        /// Returns the file name of the specified path string without the extension.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> containing the string returned by System.IO.Path.GetFileName(<see cref="System.String"/>),
        /// minus the last period (.) and all characters following it.
        /// </returns>
        public string GetFileNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(FileName);
        }

        /// <summary>
        /// Returns the directory information for the specified path string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> containing directory information for path, or null if path
        /// denotes a root directory, is the empty string (""), or is null. Returns <see cref="System.String.Empty"/>
        /// if path does not contain directory information.
        /// </returns>
        public string GetDirectoryName()
        {
            return Path.GetDirectoryName(FileName);
        }

        #endregion


        public void CheckEntryPoint(LLVMSharpCompiler compiler)
        {
            AstNamespaceBlockCollection.CheckEntryPoint(compiler);
            AstTypeCollection.CheckEntryPoint(compiler);
        }
    }

    public class AstSourceFileCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstSourceFile))
                throw new ArgumentException("You can add type of only AstSourceFile.");
            return base.Add(value);
        }

        public int Add(AstSourceFile value)
        {
            return base.Add(value);
        }
    }

}
