using System.Collections.Generic;
using System.Windows.Forms;

namespace EDBitor.Controllers.Base
{
    abstract class FormController : IInitializableController, IFormController
    {
        protected EDBitorApp App { get; private set; }
        protected Form Form { get; private set; }
        protected bool IsFormClosed;

        #region Initialization

        void IInitializableController.Initialize(Form form, EDBitorApp app)
        {
            Form = form;
            App = app;

            Initialized();
        }

        protected virtual void Initialized()
        { }

        #endregion

        #region Open/Close

        void IFormController.Open()
        {
            SubscribeToUserClosing();
            Subscribe();

            if (IsFormClosed)
                return;

            var controls = VisibleUntilBlock();
            if (controls != null)
                foreach (var control in controls)
                    control.Visible = false;

            Form.Show();
        }

        void IFormController.Close()
        {
            if (!IsFormClosed)
                Form.Close();

            IsFormClosed = true;
        }

        protected void Close()
        {
            App.Instantiator.Close(this);
        }

        #endregion

        #region Subscribing

        protected abstract void Subscribe();

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

        #region Blocking

        /// <summary>
        /// Describe set of elements that visible only on block
        /// </summary>
        /// <returns>Set of elements that visible only on block</returns>
        protected virtual HashSet<Control> VisibleUntilBlock()
        {
            return null;
        }

        protected void BlockUi()
        {
            var visibleUntilBlock = VisibleUntilBlock();

            foreach (Control control in Form.Controls)
            {
                if (visibleUntilBlock == null || !visibleUntilBlock.Contains(control))
                    control.Enabled = false;
            }

            if (visibleUntilBlock != null)
                foreach (var control in visibleUntilBlock)
                    control.Visible = true;
        }

        protected void UnblockUi()
        {
            var visibleUntilBlock = VisibleUntilBlock();

            foreach (Control control in Form.Controls)
            {
                if (visibleUntilBlock == null || !visibleUntilBlock.Contains(control))
                    control.Enabled = true;
            }

            if (visibleUntilBlock != null)
                foreach (var control in visibleUntilBlock)
                    control.Visible = false;
        }

        #endregion
    }

    abstract class FormController<TForm> : FormController
        where TForm : Form
    {
        protected new TForm Form
        {
            get { return base.Form as TForm; }
        }
    }
}
