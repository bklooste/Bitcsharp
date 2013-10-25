using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.VisualAst
{
    public partial class VisualAstMainForm
    {
        private void tvFiles_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the node at the current mouse pointer location.
            TreeNode node = tvFiles.GetNodeAt(e.X, e.Y);

            // Set a ToolTip only if the mouse pointer is actually paused on a node.
            if (node != null)
            {
                // Verify that the tag property is not "null".
                if (node.Tag != null)
                {
                    // Change the ToolTip only if the pointer moved to a new node.
                    if (node.Tag.ToString() != toolTipFileExplorer.GetToolTip(tvFiles))
                        toolTipFileExplorer.SetToolTip(tvFiles, node.Tag.ToString());
                }
                else
                {
                    toolTipFileExplorer.SetToolTip(tvFiles, "");
                }
            }
            else // Pointer is not over a node so clear the ToolTip.
            {
                toolTipFileExplorer.SetToolTip(tvFiles, "");
            }
        }

        private void tvFiles_MouseUp(object sender, MouseEventArgs e)
        {
            // if right button click
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                TreeNode node = tvFiles.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    if (node.Tag != null)
                    {
                        //contextMenuStripFileExplorer.Items[0].Text = node.Tag.ToString();
                        toolStripMenuItemOpenFolderLocation.Click +=
                            (object sender2, EventArgs e2) =>
                            {
                                string folderPath = System.IO.Path.GetDirectoryName(node.Tag.ToString());
                                string windir = Environment.GetEnvironmentVariable("WINDIR");
                                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                                prc.StartInfo.FileName = windir + @"\explorer.exe";
                                prc.StartInfo.Arguments = folderPath;
                                prc.Start();
                            };
                        contextMenuStripFileExplorer.Show(tvFiles, new Point(e.X, e.Y));
                    }
                }
            }
        }

        private void tvAstNode_MouseUp(object sender, MouseEventArgs e)
        {
            // if right button click
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                TreeNode node = tvAstNode.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    if (node.Tag != null)
                    {
                        if (!(node.Tag is AstProgram))
                        {
                            string path = ""; int l = 0, c = 0;
                            if (node.Tag is AstNode)
                            {
                                AstNode astNode = (AstNode)node.Tag;
                                path = astNode.Path;
                                l = astNode.LineNumber;
                                c = astNode.ColumnNumber;
                            }
                            else if (node.Tag is AstSourceFile)
                                path = ((AstSourceFile)node.Tag).FileName;

                            ToolStripMenuItemGoToCode.Click += (object sender2, EventArgs e2) =>
                            {
                                LoadFileToSourceView(path);
                                if (node.Tag is AstNode)
                                {
                                    txtSourceGoTo(l, c);
                                    txtSource.Focus();
                                    toolStripStatusLabelRow.Text = "Ln: " + l;
                                    toolStripStatusLabelColumn.Text = "Col: " + c;
                                }
                            };
                            contextMenuStripAstNode.Show(tvAstNode, e.X, e.Y);
                        }
                    }
                }
            }
        }

        private void LoadFileToSourceView(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                MessageBox.Show("File not Found", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                // WatchFile(fileName);

                txtSource.Text = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
                lblSourceCode.Text = "Source Code for " + System.IO.Path.GetFileNameWithoutExtension(fileName);
            }
        }

        private FileSystemWatcher fileSystemWatcher;
        private void WatchFile(string fileName)
        {
            fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(fileName));
            fileSystemWatcher.Filter = Path.GetFileName(fileName);
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_Changed);
        }

        void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileName = e.FullPath;
            using (StreamReader reader = new StreamReader(fileName))
            {
                txtSource.Text = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
                lblSourceCode.Text = "Source Code for " + System.IO.Path.GetFileNameWithoutExtension(fileName);
            }
        }

        private void tvFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = tvFiles.GetNodeAt(e.X, e.Y);
            if (node != null)
            {
                if (node.Tag != null)
                    LoadFileToSourceView(node.Tag.ToString());
            }
        }

        private int txtSourceColumn
        {
            get { return txtSource.SelectionStart - txtSource.GetFirstCharIndexOfCurrentLine() + 1; }
        }

        private int txtSourceLine
        {
            get { return txtSource.GetLineFromCharIndex(txtSource.SelectionStart) + 1; }
        }

        public void txtSourceGoTo(int line, int column)
        {
            if (line < 1 || column < 1 || txtSource.Lines.Length < line)
                return;

            txtSource.SelectionStart = txtSource.GetFirstCharIndexFromLine(line - 1) + column - 1;
            txtSource.SelectionLength = txtSource.Text.IndexOf(Environment.NewLine, txtSource.SelectionStart) - txtSource.SelectionStart;
        }

        private void txtSource_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelRow.Text = "Ln: " + txtSourceLine;
            toolStripStatusLabelColumn.Text = "Col: " + txtSourceColumn;
        }

        private void txtSource_KeyUp(object sender, KeyEventArgs e)
        {
            toolStripStatusLabelRow.Text = "Ln: " + (txtSourceLine);
            toolStripStatusLabelColumn.Text = "Col: " + (txtSourceColumn);
        }

        private void objectBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectBrowserForm frm = null;
            try
            {
                frm = new ObjectBrowserForm(compiler.ClassHashtable, compiler.StructHashtable, compiler.EnumHashtable);
                frm.ClassHierarchy = compiler.ClassHierarchy;
            }
            catch { }
            if (frm != null)
                frm.Show();
            else
            {
                MessageBox.Show("Object Hierarchy has not been generated. Please fix your errors.");
            }
        }

        private void ResizeStatusBar()
        {
            /*
            int n = statusStrip1.Items.Count;

            if (n > 1)
            {
                int nRightWidth = 0;

                for (int i = 1; i < n; i++)
                {
                    nRightWidth += statusStrip1.Items[i].Width;
                }

                if (300 <= statusStrip1.Width-nRightWidth)
                {
                    statusStrip1.Items[0].Width = statusStrip1.Width - nRightWidth;
                }
            }*/

        }
    }
}
