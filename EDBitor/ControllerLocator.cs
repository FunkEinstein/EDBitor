using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EDBitor.Controllers;
using EDBitor.Controllers.Base;
using EDBitor.View;

namespace EDBitor
{
    class ControllerLocator
    {
        private readonly Dictionary<Type, Func<Form>> _formBuilders;
        private readonly List<FormController> _openedControllers;

        public ControllerLocator()
        {
            _formBuilders = BuildFactories();
            _openedControllers = new List<FormController>();
        }

        #region Open/Close

        public void Open<TController>()
            where TController : FormController, new()
        {
            var builder = _formBuilders[typeof(TController)];
            var form = builder();
            var controller = new TController();
            Initialize(controller, form);

            _openedControllers.Add(controller);
            var openable = controller as IFormController;
            openable.Open();
        }

        public void Close(FormController controller)
        {
            var closable = controller as IFormController;
            closable.Close();

            _openedControllers.RemoveAll(c => c == controller);
        }

        public TResult OpenDialog<TController, TResult>(Form parentForm)
            where TController : DialogController<TResult>, new ()
        {
            var builder = _formBuilders[typeof(TController)];
            var form = builder();
            var controller = new TController();

            var openable = controller as IDialogController<TResult>;
            if (openable == null)
                throw new InvalidOperationException("Invalid dialog context");

            Initialize(controller, form);

            _openedControllers.Add(controller);
            return openable.OpenDialog(parentForm);
        }

        public void Close<TResult>(DialogController<TResult> controller, TResult result)
        {
            var closable = controller as IDialogController<TResult>;
            closable.Close(result);

            _openedControllers.RemoveAll(c => c == controller);
        }

        #endregion

        private Dictionary<Type, Func<Form>> BuildFactories()
        {
            return new Dictionary<Type, Func<Form>>
            {
                { typeof(EditorController), () => new EditorForm() },
                { typeof(OpenFileController), () => new OpenFileFromDbDialog() },
                { typeof(EnterFileNameController), () => new EnterFileNameDialog() },
            };
        }

        private void Initialize<TForm>(FormController controller, TForm form)
            where TForm : Form, new ()
        {
            var initializable = controller as IInitializableController;
            initializable.Initialize(form, EDBitorApp.Instance);
        }
    }
}
