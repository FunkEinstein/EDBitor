using System.Windows.Forms;

namespace EDBitor.View.MessageBoxShowers
{
    class WarningMessageBoxShower : MessageBoxShower
    {
        protected override string Title => "Warning";
        protected override MessageBoxIcon MessageBoxIcon => MessageBoxIcon.Warning;
        protected override MessageBoxButtons MessageBoxButtons => MessageBoxButtons.OK;
    }
}
