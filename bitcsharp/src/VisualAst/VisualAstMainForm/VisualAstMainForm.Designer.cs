namespace LLVMSharp.VisualAst
{
    partial class VisualAstMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.phase1LexAndParseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phase2BuildASTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phase3BuildObjectHierarchyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.phase4TypeCheckingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuGenerateCode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGenerateCodeToNewWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelColumn = new System.Windows.Forms.ToolStripStatusLabel();
            this.tvFiles = new System.Windows.Forms.TreeView();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.lblSourceCode = new System.Windows.Forms.Label();
            this.tvAstNode = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAstNodeInfo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lvErrors = new System.Windows.Forms.ListView();
            this.ErrorIndexNumer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.File = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Line = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.toolTipFileExplorer = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStripFileExplorer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOpenFolderLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripAstNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemGoToCode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStripFileExplorer.SuspendLayout();
            this.contextMenuStripAstNode.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.compilerToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(932, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProjectToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Enabled = false;
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.O)));
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.openToolStripMenuItem.Text = "&Open File";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(215, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // compilerToolStripMenuItem
            // 
            this.compilerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildAllToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.toolStripMenuItem1,
            this.phase1LexAndParseToolStripMenuItem,
            this.phase2BuildASTToolStripMenuItem,
            this.phase3BuildObjectHierarchyToolStripMenuItem,
            this.toolStripSeparator1,
            this.phase4TypeCheckingToolStripMenuItem,
            this.toolStripSeparator2,
            this.mnuGenerateCode});
            this.compilerToolStripMenuItem.Name = "compilerToolStripMenuItem";
            this.compilerToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.compilerToolStripMenuItem.Text = "&Compiler";
            // 
            // buildAllToolStripMenuItem
            // 
            this.buildAllToolStripMenuItem.Name = "buildAllToolStripMenuItem";
            this.buildAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.B)));
            this.buildAllToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.buildAllToolStripMenuItem.Text = "Build All";
            this.buildAllToolStripMenuItem.Click += new System.EventHandler(this.buildAllToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(281, 6);
            // 
            // phase1LexAndParseToolStripMenuItem
            // 
            this.phase1LexAndParseToolStripMenuItem.Name = "phase1LexAndParseToolStripMenuItem";
            this.phase1LexAndParseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.phase1LexAndParseToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.phase1LexAndParseToolStripMenuItem.Text = "Phase &1 - Lex and Parse";
            this.phase1LexAndParseToolStripMenuItem.Click += new System.EventHandler(this.phase1LexAndParseToolStripMenuItem_Click);
            // 
            // phase2BuildASTToolStripMenuItem
            // 
            this.phase2BuildASTToolStripMenuItem.Name = "phase2BuildASTToolStripMenuItem";
            this.phase2BuildASTToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.phase2BuildASTToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.phase2BuildASTToolStripMenuItem.Text = "Phase 2 - Build AST";
            this.phase2BuildASTToolStripMenuItem.Click += new System.EventHandler(this.phase2BuildASTToolStripMenuItem_Click);
            // 
            // phase3BuildObjectHierarchyToolStripMenuItem
            // 
            this.phase3BuildObjectHierarchyToolStripMenuItem.Name = "phase3BuildObjectHierarchyToolStripMenuItem";
            this.phase3BuildObjectHierarchyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.phase3BuildObjectHierarchyToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.phase3BuildObjectHierarchyToolStripMenuItem.Text = "Phase 3 - Build Object Hierarchy";
            this.phase3BuildObjectHierarchyToolStripMenuItem.Click += new System.EventHandler(this.phase3BuildObjectHierarchyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(281, 6);
            // 
            // phase4TypeCheckingToolStripMenuItem
            // 
            this.phase4TypeCheckingToolStripMenuItem.Name = "phase4TypeCheckingToolStripMenuItem";
            this.phase4TypeCheckingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.phase4TypeCheckingToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.phase4TypeCheckingToolStripMenuItem.Text = "Phase 4 - Type Checking";
            this.phase4TypeCheckingToolStripMenuItem.Click += new System.EventHandler(this.phase4TypeCheckingToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(281, 6);
            // 
            // mnuGenerateCode
            // 
            this.mnuGenerateCode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGenerateCodeToNewWindow});
            this.mnuGenerateCode.Name = "mnuGenerateCode";
            this.mnuGenerateCode.Size = new System.Drawing.Size(284, 22);
            this.mnuGenerateCode.Text = "Generate Code";
            // 
            // mnuGenerateCodeToNewWindow
            // 
            this.mnuGenerateCodeToNewWindow.Name = "mnuGenerateCodeToNewWindow";
            this.mnuGenerateCodeToNewWindow.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.N)));
            this.mnuGenerateCodeToNewWindow.Size = new System.Drawing.Size(237, 22);
            this.mnuGenerateCodeToNewWindow.Text = "To New Window";
            this.mnuGenerateCodeToNewWindow.Click += new System.EventHandler(this.mnuGenerateCodeToNewWindow_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objectBrowserToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // objectBrowserToolStripMenuItem
            // 
            this.objectBrowserToolStripMenuItem.Name = "objectBrowserToolStripMenuItem";
            this.objectBrowserToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.J)));
            this.objectBrowserToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.objectBrowserToolStripMenuItem.Text = "Object Browser";
            this.objectBrowserToolStripMenuItem.Click += new System.EventHandler(this.objectBrowserToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText,
            this.toolStripProgressBar1,
            this.toolStripStatusLabelRow,
            this.toolStripStatusLabelColumn});
            this.statusStrip1.Location = new System.Drawing.Point(0, 612);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(932, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "stautsStrip";
            // 
            // statusText
            // 
            this.statusText.AutoSize = false;
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(300, 17);
            this.statusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabelRow
            // 
            this.toolStripStatusLabelRow.AutoSize = false;
            this.toolStripStatusLabelRow.Name = "toolStripStatusLabelRow";
            this.toolStripStatusLabelRow.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripStatusLabelRow.Size = new System.Drawing.Size(60, 17);
            this.toolStripStatusLabelRow.Text = "Ln :";
            this.toolStripStatusLabelRow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelColumn
            // 
            this.toolStripStatusLabelColumn.AutoSize = false;
            this.toolStripStatusLabelColumn.Name = "toolStripStatusLabelColumn";
            this.toolStripStatusLabelColumn.Size = new System.Drawing.Size(100, 17);
            this.toolStripStatusLabelColumn.Text = "Col:";
            this.toolStripStatusLabelColumn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tvFiles
            // 
            this.tvFiles.Location = new System.Drawing.Point(12, 53);
            this.tvFiles.Name = "tvFiles";
            this.tvFiles.Size = new System.Drawing.Size(167, 413);
            this.tvFiles.TabIndex = 2;
            this.tvFiles.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvFiles_NodeMouseDoubleClick);
            this.tvFiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tvFiles_MouseMove);
            this.tvFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvFiles_MouseUp);
            // 
            // txtSource
            // 
            this.txtSource.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSource.Location = new System.Drawing.Point(191, 53);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSource.Size = new System.Drawing.Size(402, 413);
            this.txtSource.TabIndex = 3;
            this.txtSource.WordWrap = false;
            this.txtSource.Click += new System.EventHandler(this.txtSource_Click);
            this.txtSource.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSource_KeyUp);
            // 
            // lblSourceCode
            // 
            this.lblSourceCode.AutoSize = true;
            this.lblSourceCode.Location = new System.Drawing.Point(192, 34);
            this.lblSourceCode.Name = "lblSourceCode";
            this.lblSourceCode.Size = new System.Drawing.Size(69, 13);
            this.lblSourceCode.TabIndex = 4;
            this.lblSourceCode.Text = "Source Code";
            // 
            // tvAstNode
            // 
            this.tvAstNode.Location = new System.Drawing.Point(602, 53);
            this.tvAstNode.Name = "tvAstNode";
            this.tvAstNode.Size = new System.Drawing.Size(320, 331);
            this.tvAstNode.TabIndex = 5;
            this.tvAstNode.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvAstNode_NodeMouseClick);
            this.tvAstNode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvAstNode_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(599, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Ast Nodes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(599, 387);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Ast Node Info";
            // 
            // txtAstNodeInfo
            // 
            this.txtAstNodeInfo.Location = new System.Drawing.Point(602, 403);
            this.txtAstNodeInfo.Multiline = true;
            this.txtAstNodeInfo.Name = "txtAstNodeInfo";
            this.txtAstNodeInfo.ReadOnly = true;
            this.txtAstNodeInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAstNodeInfo.Size = new System.Drawing.Size(318, 197);
            this.txtAstNodeInfo.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "File Explorer";
            // 
            // lvErrors
            // 
            this.lvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ErrorIndexNumer,
            this.Description,
            this.File,
            this.Line,
            this.Column});
            this.lvErrors.FullRowSelect = true;
            this.lvErrors.Location = new System.Drawing.Point(12, 485);
            this.lvErrors.Name = "lvErrors";
            this.lvErrors.Size = new System.Drawing.Size(584, 124);
            this.lvErrors.TabIndex = 10;
            this.lvErrors.UseCompatibleStateImageBehavior = false;
            this.lvErrors.View = System.Windows.Forms.View.Details;
            this.lvErrors.DoubleClick += new System.EventHandler(this.lvErrors_DoubleClick);
            // 
            // ErrorIndexNumer
            // 
            this.ErrorIndexNumer.Text = "";
            this.ErrorIndexNumer.Width = 30;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 283;
            // 
            // File
            // 
            this.File.Text = "File";
            this.File.Width = 105;
            // 
            // Line
            // 
            this.Line.Text = "Line";
            // 
            // Column
            // 
            this.Column.Text = "Column";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 469);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Errors";
            // 
            // contextMenuStripFileExplorer
            // 
            this.contextMenuStripFileExplorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpenFolderLocation});
            this.contextMenuStripFileExplorer.Name = "contextMenuStripFileExplorer";
            this.contextMenuStripFileExplorer.Size = new System.Drawing.Size(284, 26);
            // 
            // toolStripMenuItemOpenFolderLocation
            // 
            this.toolStripMenuItemOpenFolderLocation.Name = "toolStripMenuItemOpenFolderLocation";
            this.toolStripMenuItemOpenFolderLocation.Size = new System.Drawing.Size(283, 22);
            this.toolStripMenuItemOpenFolderLocation.Text = "Open &File Location in Windows Explorer";
            // 
            // contextMenuStripAstNode
            // 
            this.contextMenuStripAstNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemGoToCode});
            this.contextMenuStripAstNode.Name = "contextMenuStripAstNode";
            this.contextMenuStripAstNode.Size = new System.Drawing.Size(138, 26);
            // 
            // ToolStripMenuItemGoToCode
            // 
            this.ToolStripMenuItemGoToCode.Name = "ToolStripMenuItemGoToCode";
            this.ToolStripMenuItemGoToCode.Size = new System.Drawing.Size(137, 22);
            this.ToolStripMenuItemGoToCode.Text = "Go To Code";
            // 
            // VisualAstMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 634);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lvErrors);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAstNodeInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSourceCode);
            this.Controls.Add(this.tvAstNode);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.tvFiles);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "VisualAstMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LLVM# Visualizer";
            this.Resize += new System.EventHandler(this.VisualAstMainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStripFileExplorer.ResumeLayout(false);
            this.contextMenuStripAstNode.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TreeView tvFiles;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label lblSourceCode;
        private System.Windows.Forms.TreeView tvAstNode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAstNodeInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lvErrors;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader ErrorIndexNumer;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ColumnHeader File;
        private System.Windows.Forms.ColumnHeader Line;
        private System.Windows.Forms.ColumnHeader Column;
        private System.Windows.Forms.ToolTip toolTipFileExplorer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFileExplorer;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenFolderLocation;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRow;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelColumn;
        private System.Windows.Forms.ToolStripMenuItem compilerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phase1LexAndParseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem phase2BuildASTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildAllToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAstNode;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemGoToCode;
        private System.Windows.Forms.ToolStripMenuItem phase3BuildObjectHierarchyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuGenerateCode;
        private System.Windows.Forms.ToolStripMenuItem mnuGenerateCodeToNewWindow;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phase4TypeCheckingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}