using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EDBitor.Controllers.Base;
using EDBitor.Model;
using EDBitor.Parsers;
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

        private const string FormTitle = "EDBitor";
        private const string NewFileHeader = "New file";
        private const string FileChangedMarker = "*";

        private readonly MessageBoxShower _questionShower;
        private readonly WarningMessageBoxShower _warningShower;

        private Storage _storage;
        private FileStatus _currentFileStatus;

        public EditorController()
        {
            _questionShower = new QuestionMessageBoxShower();
            _warningShower = new WarningMessageBoxShower();
        }

        #region View controller

        protected override void Initialized()
        {
            _storage = App.Model.Storage;
            _currentFileStatus = new FileStatus(Form.EditorTextBox);

            CreateNewFile();
        }

        protected override void Subscribe()
        {
            Form.FormClosed += OnExit;

            Form.CreateMenuItem.Click += OnCreateFile;

            Form.OpenFromDiskMenuItem.Click += OnOpenFileFromDisk;
            Form.OpenFromDBMenuItem.Click += OnOpenFileFromDb;

            Form.SaveMenuItem.Click += OnSaveFile;

            Form.ExitMenuItem.Click += OnExit;

            Form.DeleteCurrentMenuItem.Click += OnDeleteCurrentMenuItemClick;
            Form.DeleteFileFromDBMenuItem.Click += OnDeleteFileFromDBMenuItemClick;

            Form.BeautifyMenuItem.Click += OnBeautifyMenuItemClick;

            Form.EditorTextBox.TextChanged += OnTextChanged;
        }
        
        protected override HashSet<Control> VisibleUntilBlock()
        {
            return new HashSet<Control> { Form.WaitPanel };
        }

        #endregion

        #region Create file

        private void OnCreateFile(object sender, EventArgs e)
        {
            if (_currentFileStatus.IsTextChanged.HasValue
                && _currentFileStatus.IsTextChanged.Value)
            {
                var result = _questionShower.Show("Do you want save changes?");
                if (result == DialogResult.Yes)
                    SaveFile();
            }

            CreateNewFile();
        }

        private void CreateNewFile()
        {
            SetFileId(null);
            SetFileName(null);
            SetText(string.Empty);
        }

        #endregion

        #region Delete file

        private async void OnDeleteCurrentMenuItemClick(object sender, EventArgs e)
        {
            var id = _currentFileStatus.Id;

            CreateNewFile();

            if (id.HasValue)
                await _storage.DeleteFile(id.Value);
        }

        private async void OnDeleteFileFromDBMenuItemClick(object sender, EventArgs e)
        {
            var fileInfo = App.Locator.OpenDialog<SelectFileFromDbController, FileInfo?>(Form);
            if (!fileInfo.HasValue)
                return;

            if (!fileInfo.Value.Id.HasValue)
                throw new InvalidOperationException("File id can't be null for open operation");

            var id = fileInfo.Value.Id.Value;
            await _storage.DeleteFile(id);
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
            var fileInfo = App.Locator.OpenDialog<SelectFileFromDbController, FileInfo?>(Form);
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

        private async void OpenFile(FileInfo fileInfo)
        {
            if (!fileInfo.Id.HasValue)
                throw new InvalidOperationException("File id can't be null for open operation");

            SetFileId(fileInfo.Id.Value);
            SetFileName(fileInfo.FileName);

            BlockUi();
            var text = await _storage.GetFile(fileInfo.Id.Value);
            SetText(text);
            UnblockUi();
        }

        #endregion

        #region Save file

        private void OnSaveFile(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            var text = _currentFileStatus.Text;
            var id = _currentFileStatus.Id;
            if (id.HasValue
                && string.IsNullOrEmpty(_currentFileStatus.FileName))
                throw new InvalidOperationException("File can't has id and doesn't have name");

            if (id.HasValue)
                UpdateFile(id.Value, text);
            else
                AddFile(text);

            MarkAsUnchanged();
        }

        private async void UpdateFile(int id, string text)
        {
            await _storage.UpdateFile(id, text);
        }

        private async void AddFile(string text)
        {
            var fileName = App.Locator.OpenDialog<EnterFileNameController, string>(Form);
            if (string.IsNullOrEmpty(fileName))
                return;

            SetFileName(fileName);

            BlockUi();
            var id = await _storage.AddFile(fileName, text);
            SetFileId(id);
            UnblockUi();
        }

        #endregion

        #region Beautify

        private void OnBeautifyMenuItemClick(object sender, EventArgs e)
        {
            var text = _currentFileStatus.Text;
            if (string.IsNullOrEmpty(text))
                _warningShower.Show("I can't beautify emptiness");

            var parser = new XmlParser();
            parser.Parse(text);
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
            _currentFileStatus.IsTextChanged = true;
            Form.Text = FormatFormTitle(_currentFileStatus.FileName, _currentFileStatus.IsTextChanged.Value);
        }

        private void MarkAsUnchanged()
        {
            _currentFileStatus.IsTextChanged = false;
            Form.Text = FormatFormTitle(_currentFileStatus.FileName, _currentFileStatus.IsTextChanged.Value);
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
            Form.Text = FormatFormTitle(fileName);
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

        #region Helpers

        private static string FormatFormTitle(string fileName = null, bool isFileChanged = false)
        {
            if (!isFileChanged)
                return $@"{FormTitle} - {fileName ?? NewFileHeader}";

            return $@"{FormTitle} - {fileName ?? NewFileHeader} {FileChangedMarker}";
        }

        #endregion
    }
}
