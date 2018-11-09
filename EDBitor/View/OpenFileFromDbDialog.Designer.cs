namespace EDBitor.View
{
    partial class OpenFileFromDbDialog
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
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FileList
            // 
            this.FileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileList.DisplayMember = "FileInfo";
            this.FileList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileList.FormattingEnabled = true;
            this.FileList.ItemHeight = 20;
            this.FileList.Location = new System.Drawing.Point(0, 0);
            this.FileList.Margin = new System.Windows.Forms.Padding(5);
            this.FileList.Name = "FileList";
            this.FileList.Size = new System.Drawing.Size(434, 444);
            this.FileList.TabIndex = 0;
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenFileButton.Location = new System.Drawing.Point(176, 462);
            this.OpenFileButton.Margin = new System.Windows.Forms.Padding(5);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(75, 30);
            this.OpenFileButton.TabIndex = 1;
            this.OpenFileButton.Text = "Open";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            // 
            // OpenFileFromDbDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(432, 503);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.FileList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenFileFromDbDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open file from DB";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox FileList;
        public System.Windows.Forms.Button OpenFileButton;
    }
}