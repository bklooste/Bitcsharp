using System.Collections;
using System.Text;

namespace LLVMSharp.Compiler.CodeGenerators.LLVM
{
    public class Index
    {
        public string Type;
        public string Idx;
    }

    /// <summary>
    ///  &lt;result> = GetLementPtr &lt;type>* &lt;pointer>
    /// </summary>
    public class GetElementPtr : Instruction
    {
        public string Result;
        public string PointerType;
        public string PointerValue;
        public ArrayList Indices;

        public GetElementPtr(Module module)
            : base(module)
        {
            Indices = new ArrayList();
        }

        public override string EmitCode()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Result))
            {
                sb.Append(Result);
                sb.Append(" = ");
            }
            sb.Append("getelementptr ");
            if (string.IsNullOrEmpty(Result))
                sb.Append("(");
            sb.Append(PointerType);
            sb.Append("* ");
            sb.Append(PointerValue);

            for (int i = 0; i < Indices.Count; i++)
            {
                Index idx = (Index)Indices[i];
                sb.Append(", ");
                sb.Append(idx.Type);
                sb.Append(" ");
                sb.Append(idx.Idx);
            }

            if (string.IsNullOrEmpty(Result))
            if (string.IsNullOrEmpty(Result))
                sb.Append(")");

            return sb.ToString();
        }
    }
}
