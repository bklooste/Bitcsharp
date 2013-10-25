using System.Collections;

namespace LLVMSharp.Compiler.UsingDirectives
{
    /// <summary>
    /// Using Directives
    /// </summary>
    public class UsingDirective
    {
        /// <summary>
        /// Head of linked list.
        /// </summary>
        private UsingObject _head;

        public UsingDirective()
        {
            _head = new UsingObject();
        }

        /// <summary>
        /// Opens a Namespace block
        /// </summary>
        /// <param name="ns">Name of the Namespace</param>
        public void OpenScope(string ns)
        {
            UsingObject newRoot = new UsingObject();

            // adds in head
            newRoot.Next = _head;

            if (string.IsNullOrEmpty(_head.Name))
                newRoot.Name = ns;
            else
                newRoot.Name = _head.Name + "." + ns;

            _head = newRoot;
        }

        /// <summary>
        /// Exit from Namespace block
        /// </summary>
        public void CloseScope()
        {
            if (_head != null)
            {   // remove head
                UsingObject tmp = _head;
                _head = _head.Next;
                tmp = null;
            }
        }

        /// <summary>
        /// Insert using directives
        /// </summary>
        /// <param name="usingDirectives">Name of the using directive</param>
        public void Insert(string usingDirectives)
        {
            _head.UsingDirectives.Add(usingDirectives);
        }

        /// <summary>
        /// Get list of namespaces valid at the current momemnt.
        /// </summary>
        /// <returns></returns>
        public string[] Namespaces
        {
            get
            {
                ArrayList list = new ArrayList();
                UsingObject curr = _head;
                while (curr != null)
                {
                    if (!string.IsNullOrEmpty(curr.Name))
                        list.Add(curr.Name);

                    foreach (string usingDirective in curr.UsingDirectives)
                        list.Add(usingDirective);

                    curr = curr.Next;
                }
                // need to optimize this.
                string[] strList = new string[list.Count];

                for (int i = 0; i < list.Count; i++)
                    strList[i] = list[i].ToString();

                return strList;
            }
        }

    }
}
