using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Kinovea.ScreenManager;

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

        private ToolStripDropDownButton openVid = new ToolStripDropDownButton();
        private ToolStripDropDownButton openCam = new ToolStripDropDownButton();
        private ToolStripDropDownButton subInf = new ToolStripDropDownButton();
        private ToolStripDropDownButton calibrationObj = new ToolStripDropDownButton();
        private ToolStripDropDownButton trackWindow = new ToolStripDropDownButton();

        private ToolStripDropDownButton placeCaliTemp = new ToolStripDropDownButton();
        private ToolStripDropDownButton saveCaliPos = new ToolStripDropDownButton();

        //private ToolStripDropDownButton linearKine = new ToolStripDropDownButton();

        public static event EventHandler<ButtonClickedEventArgs> ToolBarClick;
        private int number = 0;

        public static event EventHandler<EventArgs> TrajectoryClick;
        public static event Action CalibrationClick;
        public static event Action LegTemplateClick;

        public ToolStripButtonManager()
        {

        }

        public void PopulateButtonToolStrip(ToolStrip toolStrip)
        {
            toolStrip.AllowMerge = true;

            mnucalibrationTemplate.Text = "Calibration Object";
            placeCaliTemp.Text = "Track Calibration Object";
            placeCaliTemp.Click += placeCaliTemp_Click;
            saveCaliPos.Text = "Save Object Position";
            saveCaliPos.Click += saveCaliPos_Click;
            mnucalibrationTemplate.DropDownItems.Add(placeCaliTemp);
            mnucalibrationTemplate.DropDownItems.Add(saveCaliPos);

            //mnucalibrationTemplate.Image = Image.FromFile("C:/Users/Bryonna/Documents/GitHub/Kinovea/AnalysisSystemFinal/Resources/triangle.png");
            mnuleg12pointsTemplate.Text = "Leg 12 Points";
            mnutrackingMarker.Text = "Single Marker";
            mnuGeneratePositionGraph.Text = "Graph Position";

            //mnucalibrationTemplate.Click += mnuCalibrationTemplate_Click;
            mnuleg12pointsTemplate.Click += mnuLeg12pointsTemplate_Click;
            mnutrackingMarker.Click += mnuTrackingMarker_Click;
            mnuGeneratePositionGraph.Click += mnuGeneratePositionGraph_Click;

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] { mnucalibrationTemplate, mnuleg12pointsTemplate, mnuGeneratePositionGraph});
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        private void mnuGeneratePositionGraph_Click(object sender, EventArgs e)
        {
            /*            PlayerScreen ps = new PlayerScreen();
                        ps.ShowTrajectoryAnalysis();*/

            TrajectoryClick?.Invoke(sender, e);
        }

        private void saveCaliPos_Click(object sender, EventArgs e)
        {
            /*            PlayerScreen ps = new PlayerScreen();
                        ps.ShowTrajectoryAnalysis();*/

            TrajectoryClick?.Invoke(sender, e);
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
            LegTemplateClick?.Invoke();
        }

        private void placeCaliTemp_Click(object sender, EventArgs e)
        {
            //maybe make a call to handle it in screenmanager
            CalibrationClick?.Invoke();
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
            openVid.Text = "Open Video...";
            openCam.Text = "Open Camera...";
            subInf.Text = "Subject Info";
            calibrationObj.Text = "Calibration Obj";
            trackWindow.Text = "Marker Properties";
            //openVid.Size.Width = 50;

            mnuInfoCollection.Text = "Subject Info";
            mnuInfoCollection.Click += mnuInfoCollection_Click;

            mnuCalibrationObject.Text = "Calibration Object";
            mnuCalibrationObject.Click += mnuCalibrationObject_Click;

            mnuDataGraphing.Text = "Properties";
            mnuDataGraphing.DropDownItems.Add(trackWindow);
            mnuDataGraphing.DropDownItems.Add(subInf);
            mnuDataGraphing.DropDownItems.Add(calibrationObj);
            mnuDataGraphing.Click += MnuDataGraphing_Click;

            mnuFileBrowser.Text = "Video";
            mnuFileBrowser.DropDownItems.Add(openVid);
            mnuFileBrowser.DropDownItems.Add(openCam);

            mnuFileBrowser.Click += MnuFileBrowser_Click;

/*            mnuRecord.Text = "Record Live Video";
            mnuRecord.Click += MnuRecord_Click;*/

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] {  mnuInfoCollection, mnuDataGraphing, mnuFileBrowser });
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
