using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LLVMSharp.Compiler;
using System.Xml.Linq;

namespace LLVMSharp.VisualAst
{
    public partial class VisualAstMainForm
    {
        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] files = OpenFile(true);
            if (files == null)
                return;
            InitializeOpen(GetSourceFilsFromProject(files[0]));
        }

        private string[] GetSourceFilsFromProject(string projectFile)
        {
            List<string> files = new List<string>();

            //XDocument xdoc = XDocument.Load(projectFile);

            return files.ToArray();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] files = OpenFile(false);
            if (files == null)
                return;
            InitializeOpen(files);
        }

        private string[] OpenFile(bool isOpenProject)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (isOpenProject)
            {
                ofd.Title = "Open Project";
                ofd.Filter = "C# Project Files (*.csproj)|*.csproj|All Files (*.*)|*.*";
                ofd.Multiselect = false;
                ofd.DefaultExt = "csproj";
            }
            else
            {
                ofd.Title = "Open File";
                ofd.Filter = "C# Source Files (*.cs)|*.cs|All Files (*.*)|*.*";
                ofd.Multiselect = true;
                ofd.DefaultExt = "cs";
            }

            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.AddExtension = true;

            DialogResult result = ofd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
                return null;

            return ofd.FileNames;
        }

        private void InitializeOpen(string[] files)
        {
            ResetControls();
            if (files.Length == 0)
                return;

            Options opt = new Options();
            opt.SourceFiles = files;
            compiler = new LLVMSharpCompiler(opt);

            AddFilesToFileExplorer();

            compiler.Errors.ErrorAdded += new ErrorHandler(Errors_ErrorAdded);

            phase1LexAndParseToolStripMenuItem.Enabled = true;
            buildAllToolStripMenuItem.Enabled = true;
        }

        void Errors_ErrorAdded(object o, ErrorArgs e)
        {
            lvErrors.Invoke(new MethodInvoker(() =>
            {
                ListViewItem item = new ListViewItem((lvErrors.Items.Count + 1).ToString());
                lvErrors.Items.Add(item);
                item.SubItems.Add(e.errorInfo.message);
                item.SubItems.Add(System.IO.Path.GetFileName(e.errorInfo.fileName));
                item.SubItems.Add(e.errorInfo.line.ToString());
                item.SubItems.Add(e.errorInfo.col.ToString());
            }));
        }

        private void AddFilesToFileExplorer()
        {
            string[] files = compiler.Options.SourceFiles;
            TreeNode rootNode = files.Length == 1 ? new TreeNode("File") : new TreeNode("Files");
            tvFiles.Nodes.Add(rootNode);
            TreeNode corlib = null;
            if (compiler.Options.LinkCoreLib)
            {
                corlib = new TreeNode("corlib");
                rootNode.Nodes.Add(corlib);
            }

            string corlibDir = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "corlib");
            foreach (string file in files)
            {
                TreeNode temp = NewNode(Path.GetFileNameWithoutExtension(file), file);
                if (file.StartsWith(corlibDir))
                    corlib.Nodes.Add(temp);
                else
                    rootNode.Nodes.Add(temp);
            }
            rootNode.Expand();
        }

        private void ResetControls()
        {
            tvFiles.Nodes.Clear();

            lblSourceCode.Text = "Source Code";
            txtSource.Text = "";
            toolStripStatusLabelRow.Text = "Ln: ";
            toolStripStatusLabelColumn.Text = "Col: ";

            ResetControlsPartially();

            buildAllToolStripMenuItem.Enabled = false;
        }

        private void _ResetControlsPartially()
        {
            resetToolStripMenuItem.Enabled = false;
            phase1LexAndParseToolStripMenuItem.Enabled = false;
            phase2BuildASTToolStripMenuItem.Enabled = false;
            phase3BuildObjectHierarchyToolStripMenuItem.Enabled = false;
            phase4TypeCheckingToolStripMenuItem.Enabled = false;
            mnuGenerateCode.Enabled = false;
            mnuGenerateCodeToNewWindow.Enabled = false;
            objectBrowserToolStripMenuItem.Enabled = false;


            tvAstNode.Nodes.Clear();
            txtAstNodeInfo.Text = "";

            lvErrors.Items.Clear();

            phase1LexAndParseToolStripMenuItem.Enabled = true;
        }
        private void ResetControlsPartially()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    _ResetControlsPartially();
                }));
            }
            else
                _ResetControlsPartially();
        }

        private TreeNode NewNode(string text, object tag)
        {
            TreeNode node = new TreeNode(text);
            node.Tag = tag;
            node.Expand();
            return node;
        }

    }
}
