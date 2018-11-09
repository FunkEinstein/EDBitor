using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EDBitor.Model;

namespace EDBitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void doSomethingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var compressor = new DataCompressor();
            var storage = new Storage(compressor);
            var text = Test(storage);
            richTextBox1.Text = text;
        }

        private string Test(Storage storage)
        {
            var files = storage.GetFileList();
            var file = files.Values.FirstOrDefault();

            // storage.DeleteFile(file);

            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return string.Empty;

            var path = dialog.FileName;
            var text = File.ReadAllText(path);
            storage.SaveFile(file, text);

            files = storage.GetFileList();
            file = files.Values.First();
            text = storage.GetFile(file);
            return text;
        }

    }
}
