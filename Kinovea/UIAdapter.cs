/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

using Kinovea.FileBrowser;
using Kinovea.Root.Languages;
using Kinovea.ScreenManager;
using Kinovea.ScreenManager.Languages;
using Kinovea.Services;
using Kinovea.Updater;
using Kinovea.Video;
using Kinovea.Camera;

namespace Kinovea.Root
{
    class UIAdapter : RootKernel
    {
        private AnalysisController aL = new AnalysisController();
        private AnalysistemMainWindow aMainWindow;

        #region Analysis Menu Items
        private ToolStripMenuItem mnuAnalysis = new ToolStripMenuItem();
        private ToolStripMenuItem mnuPlaceholder = new ToolStripMenuItem();
        private ToolStripMenuItem mnuInfoCollection = new ToolStripMenuItem();
        private ToolStripMenuItem mnuCalibrationObject = new ToolStripMenuItem();

        private ToolStripMenuItem mnuDataGraphing = new ToolStripMenuItem();

        //I'd like to replace the red recording button with this synchronous recording button
        //so for now it's not implemented here
        private ToolStripMenuItem mnuStartSynchRecording = new ToolStripMenuItem();

        //easily accessible capture screen button
        private ToolStripMenuItem mnuOpenCaptureScreen = new ToolStripMenuItem();

        //To encapsulate the button clicks necessary to begin recording -- this can/will be moved later
        private ToolStripMenuItem mnuVideoRecordingWrapper = new ToolStripMenuItem();
        #endregion

        private UserControl informationForm;

        public UIAdapter() : base()
        {
            log.Debug("Loading video readers.");
            List<Type> videoReaders = new List<Type>();
            videoReaders.Add(typeof(Video.Bitmap.VideoReaderBitmap));
            videoReaders.Add(typeof(Video.FFMpeg.VideoReaderFFMpeg));
            videoReaders.Add(typeof(Video.GIF.VideoReaderGIF));
            videoReaders.Add(typeof(Video.SVG.VideoReaderSVG));
            videoReaders.Add(typeof(Video.Synthetic.VideoReaderSynthetic));
            VideoTypeManager.LoadVideoReaders(videoReaders);

            log.Debug("Loading built-in camera managers.");
            CameraTypeManager.LoadCameraManager(typeof(Camera.DirectShow.CameraManagerDirectShow));
            CameraTypeManager.LoadCameraManager(typeof(Camera.HTTP.CameraManagerHTTP));
            CameraTypeManager.LoadCameraManager(typeof(Camera.FrameGenerator.CameraManagerFrameGenerator));

            log.Debug("Loading camera managers plugins.");
            CameraTypeManager.LoadCameraManagersPlugins();

            log.Debug("Loading tools.");
            ToolManager.LoadTools();

            BuildSubTree();
            aMainWindow = new AnalysistemMainWindow(this);
            NotificationCenter.RecentFilesChanged += NotificationCenter_RecentFilesChanged;
            NotificationCenter.FullScreenToggle += NotificationCenter_FullscreenToggle;
            NotificationCenter.StatusUpdated += (s, e) => statusLabel.Text = e.Status;
            NotificationCenter.PreferenceTabAsked += NotificationCenter_PreferenceTabAsked;

            log.Debug("Plug sub modules at UI extension points (Menus, Toolbars, Statusbar, Windows).");
            //new toolstrip
            ExtendMenuStrip2(aMainWindow.menuStrip2);

            ExtendMenu(aMainWindow.menuStrip1);
            //GetAdditionalMenus(aMainWindow.menuStrip1);
            ExtendToolBar(aMainWindow.toolStrip1);
            
            ExtendStatusBar(aMainWindow.statusStrip1);
            ExtendUI();

            log.Debug("Register global services offered at Root level.");

            FormsHelper.SetMainForm(aMainWindow);
        }
    
        //Build new MenuStrip so that we can eventually remove the old one
        private void ExtendMenuStrip2(ToolStrip toolStripNew)
        {
            toolStripNew.AllowMerge = true;
            GetAdditionalMenus(toolStripNew);
        }

        public new void Launch()
        {
            screenManager.RecoverCrash();
            screenManager.LoadDefaultWorkspace();

            log.Debug("Calling Application.Run() to boot up the UI.");
            Application.Run(aMainWindow);
        }

        public new void ExtendUI()
        {
            fileBrowser.ExtendUI();
            updater.ExtendUI();
            screenManager.ExtendUI();

            aMainWindow.PlugUI(fileBrowser.UI, screenManager.UI);
            aMainWindow.SupervisorControl.buttonCloseExplo.BringToFront();
        }

        private void GetAdditionalMenus(ToolStrip toolStrip)
        {
            #region Analysis
            mnuAnalysis.Text = "Analysis";
            mnuPlaceholder.Text = "Placeholder";
            mnuAnalysis.DropDownItems.AddRange(new ToolStripItem[] { mnuPlaceholder });
            mnuPlaceholder.Click += MnuPlaceholder_Click;
            #endregion

            mnuInfoCollection.Text = "Enter Subject Info";
            mnuInfoCollection.Click += mnuInfoCollection_Click;

            mnuCalibrationObject.Text = "Calibration Object";
            mnuCalibrationObject.Click += mnuCalibrationObject_Click;

            mnuDataGraphing.Text = "Graph Data";
            mnuDataGraphing.Click += MnuDataGraphing_Click;

            mnuStartSynchRecording.Text = "Record!";
            mnuStartSynchRecording.Click += MnuStartSynchRecord_Click;

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] { mnuAnalysis, mnuDataGraphing, mnuStartSynchRecording,
                mnuInfoCollection, mnuCalibrationObject });
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
        }

        private void mnuCalibrationObject_Click(object sender, EventArgs e)
        {
            aL.CalibrateDemoClick();
        }

        private void mnuInfoCollection_Click(object sender, EventArgs e)
        {
            //informationForm = aL.InformationClick();
            //aMainWindow.AddOwnedForm(informationForm);
            aL.InfoDemoClick();
           
        }

        private void MnuStartSynchRecord_Click(object sender, EventArgs e)
        {
            Console.WriteLine("You want to record two things at once?");
            aL.TestClick();
        }

        private void MnuDataGraphing_Click(object sender, EventArgs e)
        {
            Console.WriteLine("You want to graph data?");
        }

        public void AddToMenuStrip(MenuStrip menuStrip)
        {

        }

        public void AddToToolBar(ToolBar toolBar)
        {

        }

        #region Event Handlers
        private void MnuPlaceholder_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Hey, I'm a placeholder");
            aL.TestClick();

        }

 *//*       private new void GetModuleMenus(ToolStrip menu)
        {

        }*//*

        private new void mnuToggleFileExplorerOnClick(object sender, EventArgs e)
        {
            if (aMainWindow.SupervisorControl.IsExplorerCollapsed)   //TODO need to remake this function for subclass adapter
            {
               aMainWindow.SupervisorControl.ExpandExplorer(true);
            }
            else
            {
                aMainWindow.SupervisorControl.CollapseExplorer();
            }
        }
        #endregion

    }
}*/
