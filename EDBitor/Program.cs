using System;
using System.Windows.Forms;

namespace EDBitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            EDBitorApp.Instance.Start();

            Application.Run();
        }
    }
}
