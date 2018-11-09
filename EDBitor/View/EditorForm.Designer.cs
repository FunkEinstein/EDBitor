namespace EDBitor.View
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.EditorTextBox = new System.Windows.Forms.RichTextBox();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFromDiskMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFromDBMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SeparatorMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // editorRichTextBox
            // 
            this.EditorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditorTextBox.Location = new System.Drawing.Point(0, 31);
            this.EditorTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.EditorTextBox.Name = "EditorTextBox";
            this.EditorTextBox.Size = new System.Drawing.Size(622, 403);
            this.EditorTextBox.TabIndex = 0;
            this.EditorTextBox.Text = "";
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(622, 28);
            this.menu.TabIndex = 1;
            this.menu.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateMenuItem,
            this.OpenMenuItem,
            this.SaveMenuItem,
            this.SeparatorMenuItem,
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(44, 24);
            this.FileMenuItem.Text = "File";
            // 
            // createToolStripMenuItem
            // 
            this.CreateMenuItem.Name = "CreateMenuItem";
            this.CreateMenuItem.Size = new System.Drawing.Size(216, 26);
            this.CreateMenuItem.Text = "Create";
            // 
            // openMenuItem
            // 
            this.OpenMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFromDiskMenuItem,
            this.OpenFromDBMenuItem});
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.Size = new System.Drawing.Size(216, 26);
            this.OpenMenuItem.Text = "Open";
            // 
            // openFromDiskMenuItem
            // 
            this.OpenFromDiskMenuItem.Name = "OpenFromDiskMenuItem";
            this.OpenFromDiskMenuItem.Size = new System.Drawing.Size(216, 26);
            this.OpenFromDiskMenuItem.Text = "Open file from disk";
            // 
            // openFromDBMenuItem
            // 
            this.OpenFromDBMenuItem.Name = "OpenFromDBMenuItem";
            this.OpenFromDBMenuItem.Size = new System.Drawing.Size(216, 26);
            this.OpenFromDBMenuItem.Text = "Open file from DB";
            // 
            // saveMenuItem
            // 
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.Size = new System.Drawing.Size(216, 26);
            this.SaveMenuItem.Text = "Save";
            // 
            // separatorMenuItem
            // 
            this.SeparatorMenuItem.Name = "SeparatorMenuItem";
            this.SeparatorMenuItem.Size = new System.Drawing.Size(213, 6);
            // 
            // exitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(216, 26);
            this.ExitMenuItem.Text = "Exit";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 433);
            this.Controls.Add(this.EditorTextBox);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "EditorForm";
            this.Text = "EDBtor";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox EditorTextBox;
        public System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        public System.Windows.Forms.ToolStripMenuItem OpenFromDiskMenuItem;
        public System.Windows.Forms.ToolStripMenuItem OpenFromDBMenuItem;
        public System.Windows.Forms.ToolStripSeparator SeparatorMenuItem;
        public System.Windows.Forms.ToolStripMenuItem SaveMenuItem;
        public System.Windows.Forms.ToolStripMenuItem CreateMenuItem;
        public System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
        public System.Windows.Forms.ToolStripMenuItem ExitMenuItem;

        private System.Windows.Forms.MenuStrip menu;
    }
}

