using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LLVMSharp.Compiler.Ast.Types
{
    public class AstChildClassCollection : ArrayList
    {
        public override int Add(object value)
        {
            if (!(value is AstClass))
                throw new ArgumentException("You can add type of only AstClass.");
            return base.Add(value);
        }
        public int Add(AstClass value)
        {
            return base.Add(value);
        }
    }
}
