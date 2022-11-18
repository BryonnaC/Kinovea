using System;
using System.Windows.Forms;

namespace CombineCSV
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

            var combineCSV = new CombineCSV();
            combineCSV.MaximizeBox = false;
            combineCSV.MinimizeBox = false;

            Application.Run(combineCSV);
        }
    }
}
