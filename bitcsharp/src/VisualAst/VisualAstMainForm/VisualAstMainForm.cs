using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LLVMSharp.Compiler;
using System.Threading;

namespace LLVMSharp.VisualAst
{
    public partial class VisualAstMainForm : Form
    {
        private LLVMSharpCompiler compiler = null;

        private VisualAstMainForm()
        {
            InitializeComponent();
            ResizeStatusBar();
            compiler = null;
        }

        public VisualAstMainForm(string[] args)
            : this()
        {
            //compiler = new LLVMSharpCompiler(args);
            InitializeOpen(args);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void VisualAstMainForm_Resize(object sender, EventArgs e)
        {
            //ResizeStatusBar();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compiler.Reset();
            ResetControlsPartially();

        }

        private void tvAstNode_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = tvAstNode.GetNodeAt(e.X, e.Y);

            if (node != null)
            {
                if (node.Tag != null)
                    txtAstNodeInfo.Text = node.Tag.ToString();
            }
        }

        private void lvErrors_DoubleClick(object sender, EventArgs e)
        {
            int errorIndex = int.Parse(lvErrors.SelectedItems[0].SubItems[0].Text);
            ErrorInfo errorInfo = (ErrorInfo)compiler.Errors[errorIndex - 1];
            LoadFileToSourceView(errorInfo.fileName);
            txtSource.Focus();
            txtSourceGoTo(errorInfo.line, errorInfo.col);
        }


    }
}
