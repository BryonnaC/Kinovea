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

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] {  mnuInfoCollection, mnuCalibrationObject, mnuDataGraphing });
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        private void MnuDataGraphing_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuCalibrationObject_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuInfoCollection_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
