using System;
using System.IO;
using System.Windows.Forms;
using EDBitor.Controllers.Base;
using EDBitor.Model;
using EDBitor.View;
using EDBitor.View.MessageBoxShowers;
using FileInfo = EDBitor.Model.FileInfo;

namespace EDBitor.Controllers
{
    class EditorController : FormController<EditorForm>
    {
        private class FileStatus
        {
            public int? Id;
            public string FileName;
            public bool? IsTextChanged;
            public string Text
            {
                get { return _textBox.Text; }
                set { _textBox.Text = value; }
            }

            private readonly RichTextBox _textBox;

            public FileStatus(RichTextBox textBox)
            {
                _textBox = textBox;
            }
        }

        private const string NewFileHeader = "New file";
        private const string FileChangedMarker = "*";

        private readonly WarningMessageBoxShower _messageBoxShower;

        private Storage _storage;
        private FileStatus _currentFileStatus;

        public EditorController()
        {
            _messageBoxShower = new WarningMessageBoxShower();
        }

        #region View controller

        protected override void Initialized()
        {
            _storage = App.Model.Storage;
            _currentFileStatus = new FileStatus(Form.EditorTextBox);
        }

        protected override void Subscribe()
        {
            Form.FormClosed += OnExit;

            Form.CreateMenuItem.Click += OnCreateFile;
            Form.OpenFromDiskMenuItem.Click += OnOpenFileFromDisk;
            Form.OpenFromDBMenuItem.Click += OnOpenFileFromDb;
            Form.SaveMenuItem.Click += OnSaveFile;
            Form.ExitMenuItem.Click += OnExit;

            Form.EditorTextBox.TextChanged += OnTextChanged;
        }

        #endregion

        #region Create file

        private void OnCreateFile(object sender, EventArgs e)
        {
            SetFileId(null);
            SetFileName(null);
            SetText(string.Empty);
        }

        #endregion

        #region Open file

        private void OnOpenFileFromDisk(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            var result = fileDialog.ShowDialog(Form);
            if (result != DialogResult.OK)
                return;

            var filePath = fileDialog.FileName;
            OpenFile(filePath);
        }

        private void OnOpenFileFromDb(object sender, EventArgs e)
        {
            var fileInfo = App.Locator.OpenDialog<OpenFileController, FileInfo?>(Form);
            if (!fileInfo.HasValue)
                return;

            OpenFile(fileInfo.Value);
        }

        private void OpenFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            SetFileName(fileName);

            var text = File.ReadAllText(filePath);
            SetText(text);
        }

        private void OpenFile(FileInfo info)
        {
            if (!info.Id.HasValue)
                throw new InvalidOperationException("File id can't be null for open operation");

            SetFileId(info.Id.Value);
            SetFileName(info.FileName);

            var text = _storage.GetFile(info.Id.Value);
            SetText(text);
        }

        #endregion

        #region Save file

        private void OnSaveFile(object sender, EventArgs e)
        {
            var text = _currentFileStatus.Text;
            if (!string.IsNullOrEmpty(_currentFileStatus.FileName) 
                && !string.IsNullOrEmpty(text))
            {
                _messageBoxShower.Show("Here is no file to save");
                return;
            }

            var id = _currentFileStatus.Id;
            if (id.HasValue)
                UpdateFile(id.Value, text);
            else
                AddFile(text);

            MarkAsUnchanged();
        }

        private void UpdateFile(int id, string text)
        {
            _storage.UpdateFile(id, text);
        }

        private void AddFile(string text)
        {
            var fileName = App.Locator.OpenDialog<EnterFileNameController, string>(Form);
            if (string.IsNullOrEmpty(fileName))
                return;

            SetFileName(fileName);

            // ReSharper disable once PossibleInvalidOperationException
            // _currentFileInfo.Value must be initialized by SetFileInfo
            var id = _storage.AddFile(fileName, text);
            SetFileId(id);
        }

        #endregion

        #region Text changing

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!_currentFileStatus.IsTextChanged.HasValue) // means that text is set but not changed
            {
                _currentFileStatus.IsTextChanged = false;
                return;
            }

            if (_currentFileStatus.IsTextChanged.Value)
                return;

            MarkAsChanged();
        }

        private void MarkAsChanged()
        {
            Form.Text = $@"{Form.Text} {FileChangedMarker}";
            _currentFileStatus.IsTextChanged = true;
        }

        private void MarkAsUnchanged()
        {
            _currentFileStatus.IsTextChanged = false;
        }

        #endregion

        #region Set file info

        public void SetFileId(int? id)
        {
            _currentFileStatus.Id = id;
        }

        public void SetFileName(string fileName)
        {
            _currentFileStatus.FileName = fileName;
            Form.Text = fileName ?? NewFileHeader;
        }

        public void SetText(string text)
        {
            _currentFileStatus.IsTextChanged = null; // means that text is set but not changed
            _currentFileStatus.Text = text;
        }

        #endregion

        #region Exit

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

    }
}
