using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EDBitor.Controllers.Base;
using EDBitor.Model;
using EDBitor.View;
using EDBitor.View.MessageBoxShowers;

namespace EDBitor.Controllers
{
    class SelectFileFromDbController : DialogController<SelectFileFromDbDialog, FileInfo?>
    {
        private class ListItem
        {
            public int Id;
            public string FileName { get; } // for list view

            public ListItem(FileInfo info)
            {
                // ReSharper disable once PossibleInvalidOperationException
                // Id in this context must be not null
                Id = info.Id.Value;
                FileName = info.FileName;
            }
        }

        private readonly MessageBoxShower _messageBoxShower;

        private Storage _storage;

        public SelectFileFromDbController()
        {
            _messageBoxShower = new WarningMessageBoxShower();
        }

        #region Dialog controller

        protected override void Initialized()
        {
            _storage = App.Model.Storage;
            FillList();
        }

        protected override void Subscribe()
        {
            Form.SelectFileButton.Click += OnOpenFileButtonClick;
        }

        protected override HashSet<Control> VisibleUntilBlock()
        {
            return new HashSet<Control> { Form.WaitPanel };
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

        private async void FillList()
        {
            BlockUi();
            var infos = await _storage.GetFileList();
            UnblockUi();

            if (infos.Count == 0)
            {
                _messageBoxShower.Show("No file in db");
                Close();
            }

            for (int i = 0; i < infos.Count; i++)
            {
                var item = new ListItem(infos[i]);
                Form.FileList.Items.Add(item);
            }
        }

        #endregion
    }
}
