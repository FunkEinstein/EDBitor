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
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteCurrentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteFileFromDBMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SeparatorMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WaitPanel = new System.Windows.Forms.Panel();
            this.waitLable = new System.Windows.Forms.Label();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BeautifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            this.WaitPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // EditorTextBox
            // 
            this.EditorTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditorTextBox.Location = new System.Drawing.Point(0, 28);
            this.EditorTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.EditorTextBox.Name = "EditorTextBox";
            this.EditorTextBox.Size = new System.Drawing.Size(622, 405);
            this.EditorTextBox.TabIndex = 0;
            this.EditorTextBox.Text = "";
            this.EditorTextBox.WordWrap = false;
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.formatToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(622, 28);
            this.menu.TabIndex = 1;
            this.menu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateMenuItem,
            this.OpenMenuItem,
            this.deleteMenuItem,
            this.SaveMenuItem,
            this.SeparatorMenuItem,
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(44, 24);
            this.FileMenuItem.Text = "File";
            // 
            // CreateMenuItem
            // 
            this.CreateMenuItem.Name = "CreateMenuItem";
            this.CreateMenuItem.Size = new System.Drawing.Size(128, 26);
            this.CreateMenuItem.Text = "Create";
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFromDiskMenuItem,
            this.OpenFromDBMenuItem});
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.Size = new System.Drawing.Size(128, 26);
            this.OpenMenuItem.Text = "Open";
            // 
            // OpenFromDiskMenuItem
            // 
            this.OpenFromDiskMenuItem.Name = "OpenFromDiskMenuItem";
            this.OpenFromDiskMenuItem.Size = new System.Drawing.Size(211, 26);
            this.OpenFromDiskMenuItem.Text = "Open file from disk";
            // 
            // OpenFromDBMenuItem
            // 
            this.OpenFromDBMenuItem.Name = "OpenFromDBMenuItem";
            this.OpenFromDBMenuItem.Size = new System.Drawing.Size(211, 26);
            this.OpenFromDBMenuItem.Text = "Open file from DB";
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteCurrentMenuItem,
            this.DeleteFileFromDBMenuItem});
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(128, 26);
            this.deleteMenuItem.Text = "Delete";
            // 
            // DeleteCurrentMenuItem
            // 
            this.DeleteCurrentMenuItem.Name = "DeleteCurrentMenuItem";
            this.DeleteCurrentMenuItem.Size = new System.Drawing.Size(213, 26);
            this.DeleteCurrentMenuItem.Text = "Delete current";
            // 
            // DeleteFileFromDBMenuItem
            // 
            this.DeleteFileFromDBMenuItem.Name = "DeleteFileFromDBMenuItem";
            this.DeleteFileFromDBMenuItem.Size = new System.Drawing.Size(213, 26);
            this.DeleteFileFromDBMenuItem.Text = "Delete file from DB";
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.Size = new System.Drawing.Size(128, 26);
            this.SaveMenuItem.Text = "Save";
            // 
            // SeparatorMenuItem
            // 
            this.SeparatorMenuItem.Name = "SeparatorMenuItem";
            this.SeparatorMenuItem.Size = new System.Drawing.Size(125, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(128, 26);
            this.ExitMenuItem.Text = "Exit";
            // 
            // WaitPanel
            // 
            this.WaitPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WaitPanel.Controls.Add(this.waitLable);
            this.WaitPanel.Location = new System.Drawing.Point(224, 164);
            this.WaitPanel.Name = "WaitPanel";
            this.WaitPanel.Size = new System.Drawing.Size(200, 100);
            this.WaitPanel.TabIndex = 2;
            // 
            // waitLable
            // 
            this.waitLable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.waitLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waitLable.Location = new System.Drawing.Point(0, 0);
            this.waitLable.Name = "waitLable";
            this.waitLable.Size = new System.Drawing.Size(200, 100);
            this.waitLable.TabIndex = 0;
            this.waitLable.Text = "Please wait";
            this.waitLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BeautifyMenuItem});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.formatToolStripMenuItem.Text = "Format";
            // 
            // BeautifyMenuItem
            // 
            this.BeautifyMenuItem.Name = "BeautifyMenuItem";
            this.BeautifyMenuItem.Size = new System.Drawing.Size(216, 26);
            this.BeautifyMenuItem.Text = "Beautify";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 433);
            this.Controls.Add(this.WaitPanel);
            this.Controls.Add(this.EditorTextBox);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "EditorForm";
            this.Text = "EDBtor";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.WaitPanel.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
        public System.Windows.Forms.ToolStripMenuItem DeleteCurrentMenuItem;
        public System.Windows.Forms.ToolStripMenuItem DeleteFileFromDBMenuItem;
        public System.Windows.Forms.Panel WaitPanel;
        private System.Windows.Forms.Label waitLable;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem BeautifyMenuItem;
    }
}

