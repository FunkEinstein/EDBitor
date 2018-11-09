using System.Windows.Forms;

namespace EDBitor.Controllers.Base
{
    abstract class DialogController<TResult> : FormController, IDialogController<TResult>
    {
        protected TResult Result;

        TResult IDialogController<TResult>.OpenDialog(Form form)
        {
            Subscribe();

            BeforeShowView();
            Form.ShowDialog(form);
            return Result;
        }

        void IDialogController<TResult>.Close(TResult result)
        {
            Result = result;
            Form.Close();
        }

        protected void Close()
        {
            App.Locator.Close(this, Result);
        }
    }

    abstract class DialogController<TForm, TResult> : DialogController<TResult>
        where TForm : Form
    {
        protected new TForm Form
        {
            get { return base.Form as TForm; }
        }
    }
}
