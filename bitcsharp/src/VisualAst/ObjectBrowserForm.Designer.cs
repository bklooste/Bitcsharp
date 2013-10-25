namespace LLVMSharp.VisualAst
{
    partial class ObjectBrowserForm
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
            this.lbClass = new System.Windows.Forms.ListBox();
            this.lbStruct = new System.Windows.Forms.ListBox();
            this.lbEnum = new System.Windows.Forms.ListBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tvObjectHierarchy = new System.Windows.Forms.TreeView();
            this.label4 = new System.Windows.Forms.Label();
            this.lbObjectLayout = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbMethod = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbClass
            // 
            this.lbClass.FormattingEnabled = true;
            this.lbClass.Location = new System.Drawing.Point(12, 31);
            this.lbClass.Name = "lbClass";
            this.lbClass.Size = new System.Drawing.Size(235, 225);
            this.lbClass.TabIndex = 0;
            this.lbClass.DoubleClick += new System.EventHandler(this.lbClass_DoubleClick);
            // 
            // lbStruct
            // 
            this.lbStruct.FormattingEnabled = true;
            this.lbStruct.Location = new System.Drawing.Point(12, 285);
            this.lbStruct.Name = "lbStruct";
            this.lbStruct.Size = new System.Drawing.Size(235, 147);
            this.lbStruct.TabIndex = 1;
            this.lbStruct.DoubleClick += new System.EventHandler(this.lbStruct_DoubleClick);
            // 
            // lbEnum
            // 
            this.lbEnum.FormattingEnabled = true;
            this.lbEnum.Location = new System.Drawing.Point(263, 30);
            this.lbEnum.Name = "lbEnum";
            this.lbEnum.Size = new System.Drawing.Size(228, 225);
            this.lbEnum.TabIndex = 2;
            this.lbEnum.DoubleClick += new System.EventHandler(this.lbEnum_DoubleClick);
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(263, 268);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(228, 163);
            this.txtInfo.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Classes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Structs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(260, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Enums";
            // 
            // tvObjectHierarchy
            // 
            this.tvObjectHierarchy.Location = new System.Drawing.Point(506, 30);
            this.tvObjectHierarchy.Name = "tvObjectHierarchy";
            this.tvObjectHierarchy.Size = new System.Drawing.Size(282, 401);
            this.tvObjectHierarchy.TabIndex = 7;
            this.tvObjectHierarchy.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvObjectHierarchy_NodeMouseClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(503, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Class Hierarchy";
            // 
            // lbObjectLayout
            // 
            this.lbObjectLayout.FormattingEnabled = true;
            this.lbObjectLayout.Location = new System.Drawing.Point(794, 258);
            this.lbObjectLayout.Name = "lbObjectLayout";
            this.lbObjectLayout.Size = new System.Drawing.Size(214, 173);
            this.lbObjectLayout.TabIndex = 9;
            this.lbObjectLayout.DoubleClick += new System.EventHandler(this.lbObjectLayout_DoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(794, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Object Layout";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(794, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Method Table";
            // 
            // lbMethod
            // 
            this.lbMethod.FormattingEnabled = true;
            this.lbMethod.HorizontalScrollbar = true;
            this.lbMethod.Location = new System.Drawing.Point(794, 31);
            this.lbMethod.Name = "lbMethod";
            this.lbMethod.ScrollAlwaysVisible = true;
            this.lbMethod.Size = new System.Drawing.Size(214, 199);
            this.lbMethod.TabIndex = 11;
            this.lbMethod.DoubleClick += new System.EventHandler(this.lbMethod_DoubleClick);
            // 
            // ObjectBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 443);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbMethod);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbObjectLayout);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tvObjectHierarchy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.lbEnum);
            this.Controls.Add(this.lbStruct);
            this.Controls.Add(this.lbClass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ObjectBrowserForm";
            this.Text = "LLVM# - Object Browser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbClass;
        private System.Windows.Forms.ListBox lbStruct;
        private System.Windows.Forms.ListBox lbEnum;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView tvObjectHierarchy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lbObjectLayout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox lbMethod;
    }
}