using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysystemTakeTwo
{
    class DashboardNavigation
    {
        private ToolStripMenuItem mnuInfoCollection = new ToolStripMenuItem();
        private ToolStripMenuItem mnuCalibrationObject = new ToolStripMenuItem();
        private ToolStripMenuItem mnuDataGraphing = new ToolStripMenuItem();
        private ToolStripMenuItem mnuFileBrowser = new ToolStripMenuItem();

        public static event EventHandler<ButtonClickedEventArgs> ToolBarClick;
        private int number = 0;

        public DashboardNavigation()
        {

        }

        public void PopulateToolBar(ToolStrip toolStrip)
        {
            toolStrip.AllowMerge = true;

            mnuInfoCollection.Text = "Subject Info";
            mnuInfoCollection.Click += mnuInfoCollection_Click;

            mnuCalibrationObject.Text = "Calibration Object";
            mnuCalibrationObject.Click += mnuCalibrationObject_Click;

            mnuDataGraphing.Text = "Graph Data";
            mnuDataGraphing.Click += MnuDataGraphing_Click;

            mnuFileBrowser.Text = "Open File Browser";
            mnuFileBrowser.Click += MnuFileBrowser_Click;

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] {  mnuInfoCollection, mnuCalibrationObject, mnuDataGraphing, mnuFileBrowser });
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        private void MnuFileBrowser_Click(object sender, EventArgs e)
        {
            number = 4;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }

        public void ChangeHelpfulMessage()
        {
            //todo: want messages to guide user through basic use of the system
        }

        private void MnuDataGraphing_Click(object sender, EventArgs e)
        {
            Console.WriteLine("IMPLEMENT ME!");
        }

        private void mnuCalibrationObject_Click(object sender, EventArgs e)
        {
            Console.WriteLine("IMPLEMENT ME!");
        }

        private void mnuInfoCollection_Click(object sender, EventArgs e)
        {
            Console.WriteLine("IMPLEMENT ME!");
            //throw new NotImplementedException();
        }
    }
}
