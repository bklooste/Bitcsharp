using System;
using System.Windows.Forms;
using System.Collections;
using LLVMSharp.Compiler;
using System.Threading;
using LLVMSharp.Compiler.Ast;
using System.Text;

namespace LLVMSharp.VisualAst
{
    public partial class ObjectBrowserForm : Form
    {
        private Hashtable _classhHashTable;
        private Hashtable _enumHashTable;
        private Hashtable _structHashTable;
        private Hashtable _methodTable;

        private ClassHierarchyNode _classHierarchy;
        public ClassHierarchyNode ClassHierarchy
        {
            get { return _classHierarchy; }
            set
            {
                _classHierarchy = value;
                Thread t = new Thread(new ThreadStart(
                    () =>
                    {
                        GenerateTreeViewForClassHierarchy();
                    })
                );
                t.Start();
            }
        }

        private TreeNode NewNode(string text, object tag)
        {
            TreeNode node = new TreeNode(text);
            node.Tag = tag;
            node.Expand();
            return node;
        }

        private void GenerateTreeViewForClassHierarchy()
        {
            // Generate for root node
            TreeNode node = NewNode(ClassHierarchy.FullClassName, ClassHierarchy);
            if (tvObjectHierarchy.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    tvObjectHierarchy.Nodes.Add(node);
                }));
            }
            else
                tvObjectHierarchy.Nodes.Add(node);

            GenerateTreeNodeForClassHierarchy(ClassHierarchy.ClassHierarchyNodeCollection, node);
        }

        private void GenerateTreeNodeForClassHierarchy(ClassHierarchyNodeCollection classHierarchyNodeCollection, TreeNode p)
        {
            if (classHierarchyNodeCollection != null || classHierarchyNodeCollection.Count > 0)
            {
                foreach (ClassHierarchyNode item in classHierarchyNodeCollection)
                {
                    TreeNode node = NewNode(item.FullClassName, item);
                    if (tvObjectHierarchy.InvokeRequired)
                    {
                        tvObjectHierarchy.Invoke(new MethodInvoker(() =>
                        {
                            p.Nodes.Add(node);
                            p.Expand();
                        }));
                    }
                    else
                    {
                        p.Nodes.Add(node);
                        p.Expand();
                    }
                    GenerateTreeNodeForClassHierarchy(item.ClassHierarchyNodeCollection, node);
                }
            }
        }

        public ObjectBrowserForm(Hashtable classHashTable, Hashtable structHashtable, Hashtable enumHashtable)
        {
            InitializeComponent();

            _classhHashTable = classHashTable;
            _enumHashTable = enumHashtable;
            _structHashTable = structHashtable;
            //_methodTable = methodTable;

            AddItemsToListbox(lbClass, classHashTable);
            AddItemsToListbox(lbEnum, enumHashtable);
            AddItemsToListbox(lbStruct, structHashtable);
            // AddItemsToListbox(lbMethod,methodTable);
        }

        private void AddItemsToListbox(ListBox lb, Hashtable ht)
        {
            foreach (var item in ht.Keys)
            {
                lb.Items.Add(item);
            }
        }
        private void lbMethod_DoubleClick(object sender, EventArgs e)//edited
        {
            if (lbMethod.SelectedItem != null)
            {
                TagListBox i = (TagListBox)lbMethod.SelectedItem;
                txtInfo.Text = i.Tag.ToString();
            }
        }
        //edited

        private void lbClass_DoubleClick(object sender, EventArgs e)
        {
            if (lbClass.SelectedItem != null)
            {
                AstClass astClass = (AstClass)_classhHashTable[lbClass.SelectedItem.ToString()];
                txtInfo.Text = astClass.ToString();
                PopulateMethodTable(astClass.methodTable);
                PopulateObjectLayout(astClass.ObjectLayout);
            }
        }

        private void lbStruct_DoubleClick(object sender, EventArgs e)
        {
            if (lbStruct.SelectedItem != null)
            {
                txtInfo.Text = _structHashTable[lbStruct.SelectedItem.ToString()].ToString();
                AstStruct astStruct = (AstStruct)_structHashTable[lbStruct.SelectedItem.ToString()];
                PopulateMethodTable(astStruct.methodTable);
                PopulateObjectLayout(astStruct.ObjectLayout);
            }
        }

        private void lbEnum_DoubleClick(object sender, EventArgs e)
        {
            if (lbEnum.SelectedItem != null)
                txtInfo.Text = _enumHashTable[lbEnum.SelectedItem.ToString()].ToString();
        }

        private void tvObjectHierarchy_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = tvObjectHierarchy.GetNodeAt(e.X, e.Y);

            if (node != null)
            {
                if (node.Tag != null)
                    txtInfo.Text = node.Tag.ToString();
            }
        }

        private void PopulateObjectLayout(Hashtable hashtable)
        {
            lbObjectLayout.Items.Clear();

            foreach (string item in hashtable.Keys)
            {
                TagListBox i = new TagListBox() { Tag = hashtable[item] };
                StringBuilder sb = new StringBuilder(30);
                sb.Append(((AstField)i.Tag).Name);
                if (item.StartsWith("t_"))
                    sb.Append(" [ this -  ");
                else
                    sb.Append(" [ base -  ");
                sb.Append(item.Substring(2, item.Length - 2));
                sb.Append(" ]");
                i.Name = sb.ToString();
                lbObjectLayout.Items.Add(i);
            }
        }

        class TagListBox
        {
            public string Name { get; set; }
            public object Tag { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        //private void PopulateMethodTable(Hashtable hashtable)
        //{
        //    lbMethod.Items.Clear();
        //    foreach (string item in hashtable.Keys)
        //    {
        //        lbMethod.Items.Add(item);

        //    }
        //}
        private void PopulateMethodTable(Hashtable hashtable)
        {
            lbMethod.Items.Clear();

            foreach (string item in hashtable.Keys)
            {
                TagListBox i = new TagListBox() { Tag = hashtable[item] };
                StringBuilder sb = new StringBuilder(30);
     
                //sb.Append(item);
                ////if (item.StartsWith("t_"))
                ////    sb.Append(" [ this -  ");
                ////else
                ////    sb.Append(" [ base -  ");
                //sb.Append(item.Substring(2, item.Length - 2));
                //sb.Append(" ]");
                i.Name = item;
                lbMethod.Items.Add(i);
            }
        }
        private void lbObjectLayout_DoubleClick(object sender, EventArgs e)
        {
            if (lbObjectLayout.SelectedItem != null)
            {
                TagListBox i = (TagListBox)lbObjectLayout.SelectedItem;
                txtInfo.Text = i.Tag.ToString();
            }
        }
    }
}