using System.Text.RegularExpressions;
using EDBitor.Controllers.Base;
using EDBitor.View;
using EDBitor.View.MessageBoxShowers;

namespace EDBitor.Controllers
{
    class EnterFileNameController : DialogController<EnterFileNameDialog, string>
    {
        // Based on https://docs.microsoft.com/ru-ru/windows/desktop/FileIO/naming-a-file
        private const string FileNamePattern =
            "^\\A(?!(?:COM[0-9]|CON|LPT[0-9]|NUL|PRN|AUX|com[0-9]|con|lpt[0-9]|nul|prn|aux)|[\\s\\.])" +
            "[^\\/:*\"?<>|]{1,254}\\z";

        private readonly MessageBoxShower _messageBoxShower;

        public EnterFileNameController()
        {
            _messageBoxShower = new WarningMessageBoxShower();
        }

        protected override void Subscribe()
        {
            Form.OkButton.Click += OkButtonClick;
        }

        private void OkButtonClick(object sender, System.EventArgs e)
        {
            var name = Form.NameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                _messageBoxShower.Show("Name can't be null or empty string");
                return;
            }

            var regex = new Regex(FileNamePattern);
            if (!regex.IsMatch(name))
            {
                _messageBoxShower.Show("Invalid file name");
                return;
            }

            Result = name;
            Close();
        }
    }
}
