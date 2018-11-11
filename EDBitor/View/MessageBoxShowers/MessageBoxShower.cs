using System.Windows.Forms;

namespace EDBitor.View.MessageBoxShowers
{
    abstract class MessageBoxShower
    {
        protected abstract string Title { get; }
        protected abstract MessageBoxIcon MessageBoxIcon { get; }
        protected abstract MessageBoxButtons MessageBoxButtons { get; }

        public DialogResult Show(string message)
        {
            return MessageBox.Show(message, Title, MessageBoxButtons, MessageBoxIcon);
        }
    }
}
