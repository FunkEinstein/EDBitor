using System.IO;
using System.Text.RegularExpressions;
using EDBitor.Controllers.Base;
using EDBitor.View;
using EDBitor.View.MessageBoxShowers;

namespace EDBitor.Controllers
{
    class EnterFileNameController : DialogController<EnterFileNameDialog, string>
    {
        private readonly WarningMessageBoxShower _messageBoxShower;

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

            var invalidChars = string.Join("", Path.GetInvalidFileNameChars());
            var pattern = $"^[{invalidChars}]+";
            var regex = new Regex(pattern);
            if (!regex.IsMatch(name))
            {
                _messageBoxShower.Show($"File name doesn't allow next chars {invalidChars}");
                return;
            }

            Result = name;
            Close();
        }
    }
}
