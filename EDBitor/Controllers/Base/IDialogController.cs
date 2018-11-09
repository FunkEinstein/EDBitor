using System.Windows.Forms;

namespace EDBitor.Controllers.Base
{
    interface IDialogController<TResult>
    {
        TResult OpenDialog(Form form);
        void Close(TResult result);
    }
}
