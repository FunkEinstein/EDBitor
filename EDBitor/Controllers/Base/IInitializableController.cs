using System.Windows.Forms;

namespace EDBitor.Controllers.Base
{
    interface IInitializableController
    {
        void Initialize(Form form, EDBitorApp app);
    }
}
