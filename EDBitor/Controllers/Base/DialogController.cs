using System.Windows.Forms;

namespace EDBitor.Controllers.Base
{
    abstract class DialogController<TResult> : FormController, IDialogController<TResult>
    {
        protected TResult Result;

        #region Open/Close

        TResult IDialogController<TResult>.OpenDialog(Form form)
        {
            SubscribeToUserClosing();
            Subscribe();

            if (IsFormClosed)
                return Result;

            var controls = VisibleUntilBlock();
            if (controls != null)
                foreach (var control in controls)
                    control.Visible = false;

            Form.ShowDialog(form);
            return Result;
        }

        void IDialogController<TResult>.Close(TResult result)
        {
            Result = result;

            if (!IsFormClosed)
                Form.Close();

            IsFormClosed = true;
        }

        protected new void Close()
        {
            App.Instantiator.Close(this, Result);
        }

        #endregion

        #region Subscribing

        private void SubscribeToUserClosing()
        {
            Form.FormClosing += OnFormClosing;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsFormClosed)
                return;

            if (e.CloseReason == CloseReason.UserClosing)
            {
                IsFormClosed = true;
                Close();
            }
        }

        #endregion
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
