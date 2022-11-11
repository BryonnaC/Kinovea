using System;
using System.Windows.Forms;

namespace Analysistem
{
    /**
     * This class Program is the entrypoint into the application
     * It loads and presents the GUI
     * From there, the user interacts with the GUI
     * which then interacts with the Controller
     * which then gets its methods from the Model
     * which leverages the functionality of other specialized classes
     * 
     * this description may change, those last two lines make little sense
     */
    static class AnalysistemProgram    //consider renaming to something more useful? 
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
