using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler
{
    public class TypeChecker
    {
        private readonly LLVMSharpCompiler _compiler;

        public TypeChecker(LLVMSharpCompiler compiler)
        {
            _compiler = compiler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subType">Full qualified type to check for</param>
        /// <param name="type">Full qualified type to check against</param>
        /// <returns></returns>
        public bool IsSubType(string subType, string type)
        {
            if (type == "System.Object")
                return true;
            if (subType == type)
                return true;

            // if the sub type is enum or struct
            // then it can only be the subtype of System.Object
            // coz they can't inherit anything else.
            if (type != null && subType != null)
            {
                if (type != "System.Object" &&
                    (_compiler.EnumHashtable.ContainsKey(subType) || _compiler.StructHashtable.ContainsKey(subType)))
                    return false;

                // todo more from class hierarchy

                if (_compiler.ClassHashtable.Contains(type))
                {
                    AstClass classTemp = (AstClass) _compiler.ClassHashtable[type];
                    if (classTemp.Parents.Contains(subType))
                    {
                        return true;
                    }
                }
            }


            return false;
        }
    }
}