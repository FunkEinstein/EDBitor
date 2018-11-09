using System;
using System.Windows.Forms;
using EDBitor.Controllers.Base;
using EDBitor.Model;
using EDBitor.View;
using EDBitor.View.MessageBoxShowers;

namespace EDBitor.Controllers
{
    class OpenFileController : DialogController<OpenFileFromDbDialog, FileInfo?>
    {
        private class ListItem
        {
            public int Id;
            public string FileName;

            public ListItem(FileInfo info)
            {
                // ReSharper disable once PossibleInvalidOperationException
                // Id in this context must be not null
                Id = info.Id.Value;
                FileName = info.FileName;
            }
        }

        private readonly WarningMessageBoxShower _messageBoxShower;

        private Storage _storage;

        public OpenFileController()
        {
            _messageBoxShower = new WarningMessageBoxShower();
        }

        #region Dialog controller

        protected override void Initialized()
        {
            _storage = App.Model.Storage;
        }

        protected override void Subscribe()
        {
            Form.OpenFileButton.Click += OnOpenFileButtonClick;
        }

        protected override void BeforeShowView()
        {
            FillList();
        }
        
        #endregion

        #region View event handlers

        private void OnOpenFileButtonClick(object sender, EventArgs e)
        {
            var selectedItem = Form.FileList.SelectedItem;
            if (selectedItem == null)
            {
                _messageBoxShower.Show("File doesn't selected!");
                return;
            }

            var listItem = (ListItem) selectedItem;
            Result = new FileInfo(listItem.Id, listItem.FileName);
            Close();
        }

        #endregion

        #region Helpers

        private void FillList()
        {
            var infos = _storage.GetFileList();
            for (int i = 0; i < infos.Count; i++)
            {
                var item = new ListItem(infos[i]);
                Form.FileList.Items.Add(item);
            }
        }

        #endregion
    }
}
