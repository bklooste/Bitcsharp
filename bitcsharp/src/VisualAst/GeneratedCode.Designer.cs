namespace LLVMSharp.VisualAst
{
    partial class GeneratedCode
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
            this.txtGeneratedCode = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveAsLLVMBitCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runUsingLliToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtGeneratedCode
            // 
            this.txtGeneratedCode.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGeneratedCode.Location = new System.Drawing.Point(12, 27);
            this.txtGeneratedCode.Multiline = true;
            this.txtGeneratedCode.Name = "txtGeneratedCode";
            this.txtGeneratedCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtGeneratedCode.Size = new System.Drawing.Size(770, 462);
            this.txtGeneratedCode.TabIndex = 0;
            this.txtGeneratedCode.WordWrap = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.runToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(794, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveAsLLVMBitCodeToolStripMenuItem,
            this.saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(367, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(364, 6);
            // 
            // saveAsLLVMBitCodeToolStripMenuItem
            // 
            this.saveAsLLVMBitCodeToolStripMenuItem.Name = "saveAsLLVMBitCodeToolStripMenuItem";
            this.saveAsLLVMBitCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.saveAsLLVMBitCodeToolStripMenuItem.Size = new System.Drawing.Size(367, 22);
            this.saveAsLLVMBitCodeToolStripMenuItem.Text = "Save As LLVM &Bit Code";
            this.saveAsLLVMBitCodeToolStripMenuItem.Click += new System.EventHandler(this.saveAsLLVMBitCodeToolStripMenuItem_Click);
            // 
            // saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem
            // 
            this.saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem.Name = "saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem";
            this.saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.B)));
            this.saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem.Size = new System.Drawing.Size(367, 22);
            this.saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem.Text = "Save As LLVM Bit Code Linked with Corlib";
            this.saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem.Click += new System.EventHandler(this.saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runUsingLliToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // runUsingLliToolStripMenuItem
            // 
            this.runUsingLliToolStripMenuItem.Name = "runUsingLliToolStripMenuItem";
            this.runUsingLliToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runUsingLliToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.runUsingLliToolStripMenuItem.Text = "Run using lli";
            this.runUsingLliToolStripMenuItem.Click += new System.EventHandler(this.runUsingLliToolStripMenuItem_Click);
            // 
            // GeneratedCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 501);
            this.Controls.Add(this.txtGeneratedCode);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "GeneratedCode";
            this.Text = "LLVM# Generated Code";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGeneratedCode;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveAsLLVMBitCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runUsingLliToolStripMenuItem;
    }
}