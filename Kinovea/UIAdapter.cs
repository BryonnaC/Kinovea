using System;
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
        #endregion

        public UIAdapter(int version) : base(version)
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
            ExtendMenu(aMainWindow.menuStrip1);
            GetAdditionalMenus(aMainWindow.menuStrip1);
            ExtendToolBar(aMainWindow.toolStrip1);
            //ExtendStatusBar(aMainWindow.statusStrip);
            ExtendUI2();

            log.Debug("Register global services offered at Root level.");

            FormsHelper.SetMainForm(aMainWindow);
        }

        public new void Launch()
        {
            /*screenManager.RecoverCrash();
            screenManager.LoadDefaultWorkspace();*/

            log.Debug("Calling Application.Run() to boot up the UI.");
            Application.Run(aMainWindow);
        }

        public void ExtendUI2()
        {
            fileBrowser.ExtendUI();
            updater.ExtendUI();
            screenManager.ExtendUI();

            //aMainWindow.PlugUI(fileBrowser.UI, screenManager.UI);
            //aMainWindow.SupervisorControl.buttonCloseExplo.BringToFront();
        }

        private void GetAdditionalMenus(ToolStrip toolStrip)
        {
            #region Analysis
            mnuAnalysis.Text = "Analysis";
            mnuPlaceholder.Text = "Placeholder";
            mnuAnalysis.DropDownItems.AddRange(new ToolStripItem[] { mnuPlaceholder });
            mnuPlaceholder.Click += MnuPlaceholder_Click;
            #endregion

            MenuStrip thisMenuStrip = new MenuStrip();
            thisMenuStrip.Items.AddRange(new ToolStripItem[] { mnuAnalysis });
            thisMenuStrip.AllowMerge = true;

            ToolStripManager.Merge(thisMenuStrip, toolStrip);
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
            AnalysisController aL = new AnalysisController();
            aL.TestClick();

        }
        #endregion

    }
}
