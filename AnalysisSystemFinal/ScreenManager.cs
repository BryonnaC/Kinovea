using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Kinovea.Camera;
using Kinovea.ScreenManager;
using Kinovea.FileBrowser;
using Kinovea.Services;
using Kinovea.Video;
using AnalysystemTakeTwo;
using AnalysisSystemFinal.UserInterface;

namespace AnalysisSystemFinal
{
    class ScreenManager : ScreenManagerKernel
    {
        MainFrame mainFrame;

        PropertiesPopUp properties;

        ChooseAScreen chooseScreen = new ChooseAScreen();
        RecordingControl recordingControl = new RecordingControl();
        VideoScreenControl videoControl = new VideoScreenControl();
        CameraSourceViewer availableCamerasScreen;
        CustomSplashscreen customSplash = new CustomSplashscreen();
        DashboardControl dashboardScr = new DashboardControl();

        ToolStripButtonManager dashNav = new ToolStripButtonManager();
        //CameraSourceViewer availableCamerasScreen;
        private int whichScreen = 0;

        private int whichTemplate = 0;

        //TODO should make state machine for UI windows

        public ScreenManager()
        {
            //smKernel = new ScreenManagerKernel();
            mainFrame = new MainFrame();
            //LoadSplashScreen();
            mainFrame.Show();

            FormsHelper.SetMainForm(mainFrame);

            CameraTypeManager.CameraLoadAsked += CameraTypeManager_CameraLoadAsked;
            //ChooseAScreen.ButtonClicked += ChooseAScreen_ButtonClicked;
            CustomSplashscreen.StartDashboard += CustomSplashscreen_StartDashboard;
            ToolStripButtonManager.ToolBarClick += MainNav_ToolBarClick;
            VideoTypeManager.VideoLoadAsked += VideoTypeManager_VideoLoadAsked;
            RecordingControl.ReturnToDash += RecordingControl_ReturnToDash;
            ToolStripButtonManager.TrajectoryClick += ToolStrip_TrajectoryClick;
            ToolStripButtonManager.CalibrationClick += ToolStrip_CalibrationClick;
            ToolStripButtonManager.LegTemplateClick += ToolStrip_LegTemplateClick;
            ToolStripButtonManager.TrackerClick += ToolStrip_SingleTrackClick;
            ToolStripButtonManager.ImportPositionClick += ToolStrip_ImportPositionClick;
            ToolStripButtonManager.ImportForceClick += ToolStrip_ImportForceClick;
        }

        private void ToolStrip_ImportForceClick()
        {
            ForceSelector fs = new ForceSelector();
            fs.ShowDialog();
            fs.Dispose();
        }

        private void ToolStrip_ImportPositionClick()
        {
            PositionDataSelection ps = new PositionDataSelection();
            ps.ShowDialog();
            ps.Dispose();
        }

        private void ToolStrip_SingleTrackClick()
        {
            whichTemplate = 3;
            PlayerScreenUserInterface.MouseClicked += PlayerScreenUI_MouseClicked;
        }

        private void ToolStrip_LegTemplateClick()
        {
            //We need mouse position which is best found by using PlayerScreenUserInterface
            whichTemplate = 2;
            PlayerScreenUserInterface.MouseClicked += PlayerScreenUI_MouseClicked;
        }

        private void ToolStrip_CalibrationClick()
        {
            //We need mouse position which is best found by using PlayerScreenUserInterface
            whichTemplate = 1;
            PlayerScreenUserInterface.MouseClicked += PlayerScreenUI_MouseClicked;
        }

        #region Psuedo-template for leg or calibration tracking
        private void PlayerScreenUI_MouseClicked(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(base.screenList[whichScreen].view.m_DescaledMouse);
            Console.WriteLine("alright, I'm in.");

            PlayerScreen ps = base.activeScreen as PlayerScreen;
            if (ps == null) return;

            switch (whichTemplate)
            {
                case 0:
                    return;
                case 1:
                    //front plane calibration triangle
                    ps.view.mnuDirectTrack_Click(sender, e);

                    ps.view.m_DescaledMouse.X -= 150;
                    ps.view.m_DescaledMouse.Y += 395;
                    ps.view.mnuDirectTrack_Click(sender, e);

                    ps.view.m_DescaledMouse.X += 300;
                    ps.view.m_DescaledMouse.Y += 40;
                    ps.view.mnuDirectTrack_Click(sender, e);

                    ps.view.m_DescaledMouse.X -= 150;
                    ps.view.m_DescaledMouse.Y -= 295;
                    ps.view.mnuDirectTrack_Click(sender, e);

                    //side plane calibration triangle
                    ps.view.m_DescaledMouse.X -= 800;
                    ps.view.m_DescaledMouse.Y -= 150;
                    ps.view.mnuDirectTrack_Click(sender, e);

                    ps.view.m_DescaledMouse.X -= 150;
                    ps.view.m_DescaledMouse.Y += 395;
                    ps.view.mnuDirectTrack_Click(sender, e);

                    ps.view.m_DescaledMouse.X += 300;
                    ps.view.m_DescaledMouse.Y += 40;
                    ps.view.mnuDirectTrack_Click(sender, e);

                    ps.view.m_DescaledMouse.X -= 150;
                    ps.view.m_DescaledMouse.Y -= 295;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    break;
                case 2:
                    //#1 - start with tibia
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#2
                    //ps.view.m_DescaledMouse.X -= 40;
                    ps.view.m_DescaledMouse.Y += 400;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#3
                    ps.view.m_DescaledMouse.X -= 40;
                    //ps.view.m_DescaledMouse.Y -= 400;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#4
                    //ps.view.m_DescaledMouse.X -= 85;
                    ps.view.m_DescaledMouse.Y -= 400;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#5 - tibia side
                    ps.view.m_DescaledMouse.X -= 60;
                    //ps.view.m_DescaledMouse.Y += 400;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#6
                    //ps.view.m_DescaledMouse.X -= 0;
                    ps.view.m_DescaledMouse.Y += 400;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#7 - start femur
                    ps.view.m_DescaledMouse.X += 160;
                    ps.view.m_DescaledMouse.Y -= 900;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#8
                    //ps.view.m_DescaledMouse.X -= 30;
                    ps.view.m_DescaledMouse.Y += 250;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#9
                    ps.view.m_DescaledMouse.X -= 50;
                    //ps.view.m_DescaledMouse.Y -= 250;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#10
                    //ps.view.m_DescaledMouse.X += 30;
                    ps.view.m_DescaledMouse.Y -= 250;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#11 - femur side
                    ps.view.m_DescaledMouse.X -= 60;
                    //ps.view.m_DescaledMouse.Y += 250;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    //#12
                    //ps.view.m_DescaledMouse.X -= 0;
                    ps.view.m_DescaledMouse.Y += 250;
                    ps.view.mnuDirectTrack_Click(sender, e);
                    break;
                case 3:
                    ps.view.mnuDirectTrack_Click(sender, e);
                    break;
            }

            PlayerScreenUserInterface.MouseClicked -= PlayerScreenUI_MouseClicked;
            whichTemplate = 0;
        }
        #endregion

        private void ToolStrip_TrajectoryClick(object sender, EventArgs e)
        {
            PlayerScreen ps = base.activeScreen as PlayerScreen;
            if (ps == null)
                return;

            ps.ShowTrajectoryAnalysisCUSTOM();
        }

        #region EventHandlers
        private void MainNav_ToolBarClick(object sender, ButtonClickedEventArgs e)
        {
            switch (e.buttonNumber)
            {
                case 1:
                    ShowSubjectInfoCollection();
                    return;
                case 2:
                    ShowCalibrationObjForm();
                    return;
                case 3:
                    return;
                case 4:
                    PrepareFileBrowser();
                    return;
                case 5:
                    dashboardScr.panel1.Controls.Clear();
                    HideCurrentScreen();
                    FirstSwitchToDashboard();
                    return;
                case 6:
                    ShowTrackWindowProperties();
                    return;
                case 7:
                    PlayerScreen ps = base.activeScreen as PlayerScreen;
                    if (ps == null) return;
                    ps.view.btnOpenAnnotations();
                    //ps.view.OpenAnnotationsAsked?.Invoke(this, EventArgs.Empty);
                    return;
                case 8:
                    PlayerScreen ps2 = base.activeScreen as PlayerScreen;
                    if (ps2 == null) return;
                    ps2.view.btnSaveAnnotations_Click(sender, EventArgs.Empty);
                    return;
            }
        }

        private void ShowTrackWindowProperties()
        {
            properties = new PropertiesPopUp();

            TrackingDimensionsControl trackingDims = new TrackingDimensionsControl();
            properties.mainPanel.Controls.Add(trackingDims);

            properties.ShowDialog();
            properties.Dispose();
        }

        private void RecordingControl_ReturnToDash()
        {
            HideCurrentScreen();
            BackToDashboard();
        }

        private void CustomSplashscreen_StartDashboard()
        {
            //ShowInitialScreen();
            HideCurrentScreen();
            FirstSwitchToDashboard();
        }

        private void CameraTypeManager_CameraLoadAsked(object source, CameraLoadAskedEventArgs e)
        {
            //mainFrame.Controls.Clear();
            dashboardScr.panel1.Controls.Clear();
            dashboardScr.PageTitle.Text = "Live Recording Phase";
            dashboardScr.panel1.Controls.Add(videoControl);
            //mainFrame.Controls.Add(recordingControl);
            //recordingControl.Controls.Clear();
            whichScreen = 0;
            DoLoadCameraInScreen(e.Source, e.Target);
        }

        private void VideoTypeManager_VideoLoadAsked(object sender, VideoLoadAskedEventArgs e)
        {
            /*            mainFrame.Controls.Clear();
                        mainFrame.Controls.Add(recordingControl);*/
            dashboardScr.panel1.Controls.Clear();
            dashboardScr.PageTitle.Text = "Video Playback Phase";
            dashboardScr.panel1.Controls.Add(videoControl);
            whichScreen = 0;
            DoLoadMovieInScreen(e.Path, e.Target);
        }
        #endregion

        #region Not Event Handlers
        public void LoadSplashScreen()
        {
            mainFrame.Controls.Add(customSplash);
        }

        public static async Task DelaySplashScreen()
        {
            await Task.Delay(500);
        }

        public void CreateCaptureScreen()
        {
            videoControl.panel1.Controls.Clear();
            videoControl.panel1.Controls.Add(base.screenList[whichScreen].UI);
        }

        private void CreateVideoScreen()
        {
            dashNav.PopulateVideoToolStrip(videoControl.toolStrip1);
            videoControl.panel1.Controls.Clear();
            videoControl.panel1.Controls.Add(base.screenList[whichScreen].UI);
        }
        new private void DoLoadMovieInScreen(string path, int targetScreen)
        {
            base.DoLoadMovieInScreen(path, targetScreen);
            CreateVideoScreen();
        }

        new private void DoLoadCameraInScreen(CameraSummary summary, int targetScreen)
        {
            //mainFrame.Controls.Remove(availableCamerasScreen);
            base.DoLoadCameraInScreen(summary, targetScreen);
            CreateCaptureScreen();
        }

        public void HideCurrentScreen()
        {
            mainFrame.Controls.Clear();
        }

        public void FirstSwitchToDashboard()
        {
            HideCurrentScreen();
            dashNav.PopulateStaticToolBar(dashboardScr.toolStrip1);

            ScreenManagerUserInterface scrMgUI = new ScreenManagerUserInterface();

            dashboardScr.PageTitle.Text = "Live Recording Phase";

            dashboardScr.panel1.Controls.Add(scrMgUI);
            mainFrame.Controls.Add(dashboardScr);
        }

        private void BackToDashboard()
        {
            dashboardScr.panel1.Controls.Clear();

            ScreenManagerUserInterface scrMgUI = new ScreenManagerUserInterface();
            dashboardScr.panel1.Controls.Add(scrMgUI);
            mainFrame.Controls.Add(dashboardScr);
        }

        private void PrepareFileBrowser()
        {
            FileSelection fs = new FileSelection();
            FileBrowserUserInterface fileBrowsUI = new FileBrowserUserInterface();
            ScreenManagerUserInterface scrMgUI = new ScreenManagerUserInterface();

            fs.panel1.Controls.Clear();
            fs.panel1.Controls.Add(scrMgUI);

            fs.panel2.Controls.Clear();
            fs.panel2.Controls.Add(fileBrowsUI);

            dashboardScr.panel1.Controls.Clear();
            dashboardScr.PageTitle.Text = "Video Selection Phase";
            dashboardScr.panel1.Controls.Add(fs);
        }

        private void ShowSubjectInfoCollection()
        {
            properties = new PropertiesPopUp();
            
            SubjectInformationScreen subjInfoScr = new SubjectInformationScreen();
            properties.mainPanel.Controls.Add(subjInfoScr);

            properties.ShowDialog();
            properties.Dispose();
        }

        private void ShowCalibrationObjForm()
        {
            properties = new PropertiesPopUp();

            CalibrationObjectDimensions calibObjControl = new CalibrationObjectDimensions();
            properties.mainPanel.Controls.Add(calibObjControl);

            properties.ShowDialog();
            properties.Dispose();
        }

        #endregion
    }
}
