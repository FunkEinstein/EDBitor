using System.Windows.Forms;

namespace EDBitor.View.MessageBoxShowers
{
    class WarningMessageBoxShower : MessageBoxShower
    {
        private const string _warning = "Warning";
        private MessageBoxIcon _messageBoxIcon = MessageBoxIcon.Warning;

        private MessageBoxButtons _messageBoxButtons;

        public WarningMessageBoxShower()
            : this(MessageBoxButtons.OK)
        { }

        public WarningMessageBoxShower(MessageBoxButtons messageBoxButtons)
        {
            _messageBoxButtons = messageBoxButtons;
        }

        public override void Show(string message)
        {
            MessageBox.Show(message, _warning, _messageBoxButtons, _messageBoxIcon);
        }
    }
}
