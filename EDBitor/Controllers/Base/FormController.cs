using System.Windows.Forms;

namespace EDBitor.Controllers.Base
{
    abstract class FormController : IInitializableController, IFormController
    {
        protected EDBitorApp App { get; private set; }
        protected Form Form { get; private set; }

        void IInitializableController.Initialize(Form form, EDBitorApp app)
        {
            Form = form;
            App = app;
            Initialized();
        }

        void IFormController.Open()
        {
            Subscribe();

            BeforeShowView();
            Form.Show();
        }

        void IFormController.Close()
        {
            Form.Close();
        }

        protected void Close()
        {
            App.Locator.Close(this);
        }

        protected virtual void Initialized()
        { }

        protected abstract void Subscribe();

        protected virtual void BeforeShowView()
        { }
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
