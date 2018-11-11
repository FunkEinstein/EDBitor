using System.Windows.Forms;

namespace EDBitor.View.MessageBoxShowers
{
    class QuestionMessageBoxShower : MessageBoxShower
    {
        protected override string Title => "Question";
        protected override MessageBoxIcon MessageBoxIcon => MessageBoxIcon.Question;
        protected override MessageBoxButtons MessageBoxButtons => MessageBoxButtons.YesNo;
    }
}
