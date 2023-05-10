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
        private ToolStripMenuItem mnuProperties = new ToolStripMenuItem();
        private ToolStripMenuItem mnuVideo = new ToolStripMenuItem();
        private ToolStripMenuItem mnuCaclulations = new ToolStripMenuItem();
        
        private ToolStripMenuItem mnuCalibrationTemplate = new ToolStripMenuItem();
        private ToolStripMenuItem mnu12PointLegTemplate = new ToolStripMenuItem();
        private ToolStripMenuItem mnuTrackingMarker = new ToolStripMenuItem();
        private ToolStripMenuItem mnuGenerateTrackingGraph = new ToolStripMenuItem();
        private ToolStripMenuItem mnuImportForce = new ToolStripMenuItem();

        private ToolStripDropDownButton openVid = new ToolStripDropDownButton();
        private ToolStripDropDownButton openCam = new ToolStripDropDownButton();
        private ToolStripDropDownButton openAnnotation = new ToolStripDropDownButton();
        private ToolStripDropDownButton saveAnnotation = new ToolStripDropDownButton();
        private ToolStripDropDownButton subInf = new ToolStripDropDownButton();
        private ToolStripDropDownButton calibrationObj = new ToolStripDropDownButton();
        private ToolStripDropDownButton trackWindow = new ToolStripDropDownButton();
        private ToolStripDropDownButton importForce = new ToolStripDropDownButton();
        private ToolStripDropDownButton importMarkerData = new ToolStripDropDownButton();

        private ToolStripDropDownButton placeCaliTemp = new ToolStripDropDownButton();
        private ToolStripDropDownButton saveCaliPos = new ToolStripDropDownButton();

        //private ToolStripDropDownButton linearKine = new ToolStripDropDownButton();

        public static event EventHandler<ButtonClickedEventArgs> ToolBarClick;
        private int number = 0;

        public static event EventHandler<EventArgs> TrajectoryClick;
        public static event Action CalibrationClick;
        public static event Action LegTemplateClick;
        public static event Action TrackerClick;
        public static event Action ImportForceClick;
        public static event Action ImportPositionClick;

        public ToolStripButtonManager()
        {

        }

        public void PopulateVideoToolStrip(ToolStrip toolStrip)
        {
            toolStrip.AllowMerge = true;

            mnuCalibrationTemplate.Text = "Calibration Object";
            placeCaliTemp.Text = "Track Calibration Object";
            placeCaliTemp.Click += placeCaliTemp_Click;
            saveCaliPos.Text = "Save Object Position";
            saveCaliPos.Click += saveCaliPos_Click;
            saveCaliPos.Width = 120;

            mnuCalibrationTemplate.DropDownItems.Add(placeCaliTemp);
            mnuCalibrationTemplate.DropDownItems.Add(saveCaliPos);

            mnu12PointLegTemplate.Text = "Leg 12 Points";
            mnuTrackingMarker.Text = "Single Marker";
            mnuGenerateTrackingGraph.Text = "Plot Position";
            mnuImportForce.Text = "Add Force Data";

            mnu12PointLegTemplate.Click += mnuLeg12pointsTemplate_Click;
            mnuTrackingMarker.Click += mnuTrackingMarker_Click;
            mnuGenerateTrackingGraph.Click += mnuGeneratePositionGraph_Click;
            mnuImportForce.Click += mnuImportForce_Click;

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] { mnuTrackingMarker, mnuCalibrationTemplate, mnu12PointLegTemplate, mnuGenerateTrackingGraph, mnuImportForce});
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        public void PopulateStaticToolBar(ToolStrip toolStrip)
        {
            toolStrip.AllowMerge = true;
            openVid.Text = "Open Video...";
            openVid.Click += DropDownVideo_Click;
            openCam.Text = "Open Camera...";
            openCam.Click += DropDownCam_Click;
            openAnnotation.Text = "Open Annotation...";
            openAnnotation.Click += DropDownOpenAnnotation_Click;
            saveAnnotation.Text = "Save Annotation...";
            saveAnnotation.Click += DropDownSaveAnnotation_Click;

            subInf.Text = "Subject Info";
            subInf.Click += DropDownSub_Click;
            calibrationObj.Text = "Calibration Obj";
            calibrationObj.Click += DropDownCali_Click;
            trackWindow.Text = "Marker Properties";
            trackWindow.Click += DropDownTrackWindow_Click;
            importForce.Text = "Import Force Data";
            importForce.Click += DropDownImpForce_Click;
            importMarkerData.Text = "Use Existing Position Data";
            importMarkerData.Click += DropDownImpMarkers_Click;
            
            importMarkerData.Width = 130;
            openVid.Width = 100;
            subInf.Width = 90;

            mnuProperties.Text = "Properties";
            mnuProperties.DropDownItems.Add(trackWindow);
            mnuProperties.DropDownItems.Add(subInf);
            mnuProperties.DropDownItems.Add(calibrationObj);
            mnuProperties.Click += MnuProperties_Click;

            mnuVideo.Text = "Video";
            mnuVideo.DropDownItems.Add(openVid);
            mnuVideo.DropDownItems.Add(openCam);
            mnuVideo.DropDownItems.Add(openAnnotation);
            mnuVideo.DropDownItems.Add(saveAnnotation);

            mnuVideo.Click += mnuVideo_Click;

            mnuCaclulations.Text = "Calculations";
            mnuCaclulations.DropDownItems.Add(importForce);
            mnuCaclulations.DropDownItems.Add(importMarkerData);
            

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] {  mnuProperties, mnuVideo, mnuCaclulations });
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        private void DropDownSaveAnnotation_Click(object sender, EventArgs e)
        {
            number = 8;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }

        private void DropDownOpenAnnotation_Click(object sender, EventArgs e)
        {
            number = 7;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
        }

        private void mnuImportForce_Click(object sender, EventArgs e)
        {
            ImportForceClick?.Invoke();
        }

        private void DropDownImpMarkers_Click(object sender, EventArgs e)
        {
            ImportPositionClick?.Invoke();
        }

        private void DropDownImpForce_Click(object sender, EventArgs e)
        {
            ImportForceClick?.Invoke();
        }

        private void mnuGeneratePositionGraph_Click(object sender, EventArgs e)
        {
            TrajectoryClick?.Invoke(sender, e);
        }

        private void saveCaliPos_Click(object sender, EventArgs e)
        {
            TrajectoryClick?.Invoke(sender, e);
        }

        private void mnuTrackingMarker_Click(object sender, EventArgs e)
        {
            TrackerClick?.Invoke();
        }

        private void mnuLeg12pointsTemplate_Click(object sender, EventArgs e)
        {
            LegTemplateClick?.Invoke();
        }

        private void placeCaliTemp_Click(object sender, EventArgs e)
        {
            CalibrationClick?.Invoke();
        }

        private void DropDownTrackWindow_Click(object sender, EventArgs e)
        {
            number = 6;
            ToolBarClick?.Invoke(this, new ButtonClickedEventArgs(number));
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

        private void MnuProperties_Click(object sender, EventArgs e)
        {
            //just open drop down
        }
    }
}
