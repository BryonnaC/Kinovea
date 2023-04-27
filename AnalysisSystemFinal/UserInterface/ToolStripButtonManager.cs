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
        private ToolStripMenuItem mnuProperties = new ToolStripMenuItem();
        private ToolStripMenuItem mnuVideo = new ToolStripMenuItem();

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

        public void PopulateVideoToolStrip(ToolStrip toolStrip)
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

        public void PopulateStaticToolBar(ToolStrip toolStrip)
        {
            toolStrip.AllowMerge = true;
            openVid.Text = "Open Video...";
            openVid.Click += DropDownVideo_Click;
            openCam.Text = "Open Camera...";
            openCam.Click += DropDownCam_Click;
            subInf.Text = "Subject Info";
            subInf.Click += DropDownSub_Click;
            calibrationObj.Text = "Calibration Obj";
            calibrationObj.Click += DropDownCali_Click;
            trackWindow.Text = "Marker Properties";

            openVid.Width = 80;
            subInf.Width = 90;

            mnuProperties.Text = "Properties";
            mnuProperties.DropDownItems.Add(trackWindow);
            mnuProperties.DropDownItems.Add(subInf);
            mnuProperties.DropDownItems.Add(calibrationObj);
            mnuProperties.Click += MnuProperties_Click;

            mnuVideo.Text = "Video";
            mnuVideo.DropDownItems.Add(openVid);
            mnuVideo.DropDownItems.Add(openCam);

            mnuVideo.Click += mnuVideo_Click;

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] {  mnuProperties, mnuVideo });
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        private void DropDownCali_Click(object sender, EventArgs e)
        {
            number = 2;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }

        private void DropDownSub_Click(object sender, EventArgs e)
        {
            number = 1;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }

        private void DropDownCam_Click(object sender, EventArgs e)
        {
            number = 5;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }

        private void DropDownVideo_Click(object sender, EventArgs e)
        {
            //open file browser to choose a video to load 
            number = 4;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }

        private void mnuVideo_Click(object sender, EventArgs e)
        {
            //Just open the drop down
        }

        public void ChangeHelpfulMessage()
        {
            //todo: want messages to guide user through basic use of the system
        }

        private void MnuProperties_Click(object sender, EventArgs e)
        {
            //just open drop down
        }
    }
}
