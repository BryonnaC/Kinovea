using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kinovea.Camera;
using Kinovea.ScreenManager;
using Kinovea.FileBrowser;
using Kinovea.Services;
using Kinovea.Video;
using AnalysystemTakeTwo;

namespace AnalysisSystemFinal
{
    class ScreenManager : ScreenManagerKernel
    {
        MainFrame mainFrame;
        ChooseAScreen chooseScreen = new ChooseAScreen();
        RecordingControl recordingControl = new RecordingControl();
        CameraSourceViewer availableCamerasScreen;
        CustomSplashscreen customSplash = new CustomSplashscreen();
        DashboardControl dashboardScr = new DashboardControl();

        ToolStripButtonManager dashNav = new ToolStripButtonManager();
        //CameraSourceViewer availableCamerasScreen;
        private int whichScreen = 0;

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
            ToolStripButtonManager.ToolBarClick += DashboardNav_ToolBarClick;
            VideoTypeManager.VideoLoadAsked += VideoTypeManager_VideoLoadAsked;
            RecordingControl.ReturnToDash += RecordingControl_ReturnToDash;
            ToolStripButtonManager.TrajectoryClick += ToolStrip_TrajectoryClick;
        }

        private void ToolStrip_TrajectoryClick(object sender, EventArgs e)
        {
            base.mnuTrajectoryAnalysis_OnClick(sender, e);
        }

        #region EventHandlers
        private void DashboardNav_ToolBarClick(object sender, ButtonClickedEventArgs e)
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
            }
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
            dashboardScr.panel1.Controls.Add(recordingControl);
            //mainFrame.Controls.Add(recordingControl);
            //recordingControl.Controls.Clear();
            whichScreen = 0;
            DoLoadCameraInScreen(e.Source, e.Target);
        }

        //this was for testing
/*        private void ChooseAScreen_ButtonClicked(object sender, ButtonClickedEventArgs e)
        {
            switch (e.buttonNumber)
            {
                case 1:
                    ShowCamChooser();
                    break;
                case 2:
                    Console.WriteLine("Not Implemented");
                    break;
                case 3:
                    Console.WriteLine("Not Implemented");
                    break;
                case 4:
                    ShowSubjectInfoScreen();
                    break;
            }
        }*/

        new private void VideoTypeManager_VideoLoadAsked(object sender, VideoLoadAskedEventArgs e)
        {
            /*            mainFrame.Controls.Clear();
                        mainFrame.Controls.Add(recordingControl);*/
            dashboardScr.panel1.Controls.Clear();
            dashboardScr.PageTitle.Text = "Video Playback Phase";
            dashboardScr.panel1.Controls.Add(recordingControl);
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
            await Task.Delay(2000);
        }

        public void CreateCaptureScreen()
        {
            recordingControl.panel1.Controls.Clear();
            recordingControl.panel1.Controls.Add(base.screenList[whichScreen].UI);
        }

        private void CreateVideoScreen()
        {
            dashNav.PopulateButtonToolStrip(recordingControl.buttonToolStrip);
            recordingControl.panel1.Controls.Clear();
            recordingControl.panel1.Controls.Add(base.screenList[whichScreen].UI);
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

/*        public void ShowSubjectInfoScreen()
        {
            SubjectInformationScreen subInfoScr = new SubjectInformationScreen();
            mainFrame.Controls.Remove(chooseScreen);
            mainFrame.Controls.Add(subInfoScr);
        }*/

        public void HideCurrentScreen()
        {
            mainFrame.Controls.Clear();
        }

        public void FirstSwitchToDashboard()
        {
            HideCurrentScreen();
            dashNav.PopulateToolBar(dashboardScr.toolStrip1);

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
            FileBrowserUserInterface fileBrowsUI = new FileBrowserUserInterface();
            dashboardScr.panel2.Controls.Clear();
            dashboardScr.PageTitle.Text = "Video Selection Phase";
            dashboardScr.panel2.Controls.Add(fileBrowsUI);
        }

        private void ShowSubjectInfoCollection()
        {
            SubjectInformationScreen subjInfoScr = new SubjectInformationScreen();
            dashboardScr.panel1.Controls.Clear();
            dashboardScr.PageTitle.Text = "Information Collection Phase";
            dashboardScr.panel1.Controls.Add(subjInfoScr);
        }

        private void ShowCalibrationObjForm()
        {
            CalibrationObjectDimensions calibObjControl = new CalibrationObjectDimensions();
            dashboardScr.panel1.Controls.Clear();
            dashboardScr.PageTitle.Text = "Calibration Object Phase";
            dashboardScr.panel1.Controls.Add(calibObjControl);
        }

        #endregion

        #region Not Permanent/Not In Use
        public void ShowCaptureScreen()
        {

        }

        public void ShowPlaybackScreen()
        {

        }

        public void ShowCamChooser()
        {
            mainFrame.Controls.Remove(chooseScreen);
            availableCamerasScreen = new CameraSourceViewer();
            mainFrame.Controls.Add(availableCamerasScreen);
        }

        public void ShowInitialScreen()
        {
            mainFrame.Controls.Remove(customSplash);
            mainFrame.Controls.Add(chooseScreen);

            mainFrame.Show();
        }
        #endregion
    }
}
