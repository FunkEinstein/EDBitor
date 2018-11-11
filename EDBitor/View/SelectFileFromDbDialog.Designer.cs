namespace EDBitor.View
{
    partial class SelectFileFromDbDialog
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
            this.FileList = new System.Windows.Forms.ListBox();
            this.SelectFileButton = new System.Windows.Forms.Button();
            this.WaitPanel = new System.Windows.Forms.Panel();
            this.WaitLable = new System.Windows.Forms.Label();
            this.WaitPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileList
            // 
            this.FileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileList.DisplayMember = "FileName";
            this.FileList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileList.FormattingEnabled = true;
            this.FileList.ItemHeight = 20;
            this.FileList.Location = new System.Drawing.Point(0, 0);
            this.FileList.Margin = new System.Windows.Forms.Padding(5);
            this.FileList.Name = "FileList";
            this.FileList.Size = new System.Drawing.Size(434, 444);
            this.FileList.TabIndex = 0;
            // 
            // SelectFileButton
            // 
            this.SelectFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectFileButton.Location = new System.Drawing.Point(176, 462);
            this.SelectFileButton.Margin = new System.Windows.Forms.Padding(5);
            this.SelectFileButton.Name = "SelectFileButton";
            this.SelectFileButton.Size = new System.Drawing.Size(75, 30);
            this.SelectFileButton.TabIndex = 1;
            this.SelectFileButton.Text = "Select";
            this.SelectFileButton.UseVisualStyleBackColor = true;
            // 
            // WaitPanel
            // 
            this.WaitPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WaitPanel.Controls.Add(this.WaitLable);
            this.WaitPanel.Location = new System.Drawing.Point(116, 201);
            this.WaitPanel.Name = "WaitPanel";
            this.WaitPanel.Size = new System.Drawing.Size(200, 100);
            this.WaitPanel.TabIndex = 3;
            // 
            // WaitLable
            // 
            this.WaitLable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WaitLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WaitLable.Location = new System.Drawing.Point(0, 0);
            this.WaitLable.Name = "WaitLable";
            this.WaitLable.Size = new System.Drawing.Size(200, 100);
            this.WaitLable.TabIndex = 0;
            this.WaitLable.Text = "Please wait";
            this.WaitLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectFileFromDbDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(432, 503);
            this.Controls.Add(this.WaitPanel);
            this.Controls.Add(this.SelectFileButton);
            this.Controls.Add(this.FileList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectFileFromDbDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select file from db";
            this.WaitPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox FileList;
        public System.Windows.Forms.Button SelectFileButton;
        public System.Windows.Forms.Panel WaitPanel;
        private System.Windows.Forms.Label WaitLable;
    }
}