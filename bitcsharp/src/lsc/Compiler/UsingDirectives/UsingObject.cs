using System.Collections;

namespace LLVMSharp.Compiler.UsingDirectives
{
    /// <summary>
    /// Structure of each symbol of UsingObject
    /// </summary>
    public class UsingObject
    {
        public UsingObject()
        {
            UsingDirectives = new ArrayList();
        }

        /// <summary>
        /// List of Using Directives
        /// </summary>
        public ArrayList UsingDirectives;

        /// <summary>
        /// Next Using Object
        /// </summary>
        public UsingObject Next;
        
        /// <summary>
        /// Name of namespace
        /// </summary>
        public string Name;
    }
}
