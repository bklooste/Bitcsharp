
namespace LLVMSharp.Compiler.Semantic
{
    /// <summary>
    /// This symbolobject class represent the structure of each symbol 
    /// in the particular scope and it also represent the scope itself
    /// </summary>
    public class SymbolObject
    {
        public string Name;  //Name of the object in the scope
        public string Type;  //Type of the object in the scope OR Scope Type
        public bool isArray = false; //Is Array or not
        public bool isConst = false; //Is Constant or not
        public SymbolObject Next;  //Next object in the scope OR Next scope
        public SymbolObject Locals; //Local Varible
    }

    /// <summary>
    /// The SymbolTableNode is created for a particular block
    /// </summary>
    public class SymbolTableNode
    {
        public SymbolObject topScope;
        private ErrorList _error;

        public SymbolTableNode(ErrorList errors)
        {
            topScope = null;            
            _error = errors;

        }

        /// <summary>
        /// The scope is open and initialize the topScope
        /// </summary>
        public void OpenScope()
        {
            SymbolObject scope = new SymbolObject();

            scope.Name = ""; 
            scope.Type = "ScopeType";
            scope.Locals = null;
            scope.Next = topScope;

            topScope = scope;
        }

        /// <summary>
        /// Close a current scope
        /// </summary>
        public void CloseScope()
        {
            topScope = topScope.Next;
        }
        /// <summary>
        /// Insert a new symbol object into current scope
        /// </summary>
        /// <param name="name">Name of the object</param>
        /// <param name="type">Type of the object (FQT)</param>
        public void Insert(string name, string type, bool isArr, bool isConst, int ln, int col, string file)
        {
            SymbolObject previous, last, current = new SymbolObject();

            current.Name = name;
            current.Type = type;
            current.isArray = isArr;
            current.isConst = isConst;
  
            previous = topScope.Locals;
            last = null;
            bool _haserror = false;

            while (previous != null)
            {
                if (previous.Name == name)
                {
                    //Give and Error
                    _error.Add(new ErrorInfo(ErrorType.SymenticError,
                        "A local variable named '"+ name +"' is already defined in this scope", 
                        ln, col, file));
                    _haserror = true;
                }
                last = previous;
                previous = previous.Next;
            }

            if (!_haserror)//Find in parent scope
            {
                SymbolObject obj = Lookup(name);
                if (obj != null)
                    _error.Add(new ErrorInfo(ErrorType.SymenticError,
                        "A local variable named '"+name+"' cannot be declared in this scope because it would give a different meaning to '"+name+"', which is already used in a 'parent or current' scope to denote something else", 
                        ln, col, file));
                
            }

            //if there is no local object in current scope point it to topscope local
            if (last == null)
            {
                topScope.Locals = current;
            }
            else
            {
                last.Next = current;
            }
            
        }

        /// <summary>
        /// Looking up for the current object name
        /// </summary>
        /// <param name="name">The name of the object</param>
        public SymbolObject Lookup(string name)
        {
            SymbolObject obj, scope;

            scope = topScope;

            while (scope != null) //going down for all the scope
            {
                obj = scope.Locals;
                while (obj != null) //going down for each object in a scope
                {
                    if (obj.Name == name)
                        return obj;

                    obj = obj.Next;
                }

                scope = scope.Next;
            }

            return null;
        }



        //public static void CreateSymbolTableNode(LLVMSharpCompiler Compiler)
        //{
        //    if (Compiler.ClassHierarchy != null)
        //    {
        //        foreach (ClassHierarchyNode item in Compiler.ClassHierarchy.ClassHierarchyNodeCollection)
        //        {
        //            item.CreateSymbolTableForAll(Compiler);
        //        }
        //    }
        //}
        
    }
}
