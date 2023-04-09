using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysisSystemFinal
{
    class ToolStripButtonManager
    {
        private ToolStripMenuItem mnuInfoCollection = new ToolStripMenuItem();
        private ToolStripMenuItem mnuCalibrationObject = new ToolStripMenuItem();
        private ToolStripMenuItem mnuRecord = new ToolStripMenuItem();
        private ToolStripMenuItem mnuDataGraphing = new ToolStripMenuItem();
        private ToolStripMenuItem mnuFileBrowser = new ToolStripMenuItem();

        private Button calibrationTemplate = new Button();
        private Button leg12pointsTemplate = new Button();
        private Button trackingMarker = new Button();
        
        private ToolStripMenuItem mnucalibrationTemplate = new ToolStripMenuItem();
        private ToolStripMenuItem mnuleg12pointsTemplate = new ToolStripMenuItem();
        private ToolStripMenuItem mnutrackingMarker = new ToolStripMenuItem();
        private ToolStripMenuItem mnuGeneratePositionGraph = new ToolStripMenuItem();

        public static event EventHandler<ButtonClickedEventArgs> ToolBarClick;
        private int number = 0;

        public ToolStripButtonManager()
        {

        }

        public void PopulateButtonToolStrip(ToolStrip toolStrip)
        {
            toolStrip.AllowMerge = true;

            mnucalibrationTemplate.Text = "Triangle Template";
            mnuleg12pointsTemplate.Text = "Leg Tracking Template";
            mnutrackingMarker.Text = "Individual Tracker";
            mnuGeneratePositionGraph.Text = "Generate Position Data";

            mnucalibrationTemplate.Click += mnuCalibrationTemplate_Click;
            mnuleg12pointsTemplate.Click += mnuLeg12pointsTemplate_Click;
            mnutrackingMarker.Click += mnuTrackingMarker_Click;
            mnuGeneratePositionGraph.Click += mnuGeneratePositionGraph_Click;

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] { mnucalibrationTemplate, mnuleg12pointsTemplate, mnutrackingMarker, mnuGeneratePositionGraph});
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        private void mnuGeneratePositionGraph_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InitAnalysisButtons()
        {
/*            calibrationTemplate.Text = "Place Triangle Template";
            calibrationTemplate.Click += mnuCalibrationTemplate_Click;

            leg12pointsTemplate.Text = "Place Leg Tracking Template";
            leg12pointsTemplate.Click += mnuLeg12pointsTemplate_Click;

            trackingMarker.Text = "Place Individual Tracker";
            trackingMarker.Click += mnuTrackingMarker_Click;*/
        }

        private void mnuTrackingMarker_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuLeg12pointsTemplate_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuCalibrationTemplate_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void PlaceButtons(Panel buttonPanel)
        {
            InitAnalysisButtons();

            buttonPanel.Controls.Add(leg12pointsTemplate);
            buttonPanel.Controls.Add(calibrationTemplate);
            buttonPanel.Controls.Add(trackingMarker);
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

            mnuRecord.Text = "Record Live Video";
            mnuRecord.Click += MnuRecord_Click;

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] {  mnuInfoCollection, mnuCalibrationObject, mnuRecord, mnuDataGraphing, mnuFileBrowser });
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        private void MnuRecord_Click(object sender, EventArgs e)
        {
            number = 5;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
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
            number = 2;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }

        private void mnuInfoCollection_Click(object sender, EventArgs e)
        {
            number = 1;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }
    }
}
