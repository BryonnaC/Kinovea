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
        ChooseAScreen chooseScreen = new ChooseAScreen();
        RecordingControl recordingControl = new RecordingControl();
        VideoScreenControl videoControl = new VideoScreenControl();
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
            ToolStripButtonManager.CalibrationClick += ToolStrip_CalibrationClick;
        }

        private void ToolStrip_CalibrationClick()
        {
            /*            PlayerScreen ps = new PlayerScreen();
                        VideoFrame frame = ps.frameServer.VideoReader.Current;
                        ps.frameServer.Metadata.TrackabilityManager.Track(frame);*/
            //VideoFrame frame = frameServer.VideoReader.Current;

            //okay lets try something different
            //let's subscribe to our own position click
            VideoScreenControl.SelectPoint += VideoScreen_SelectCalibPoint;

            //I don't think that is gonna work let's do this

            //base.screenList[whichScreen].view.m_DescaledMouse;
            //base.screenList[whichScreen].view.
            PlayerScreenUserInterface.MouseClicked += PSUI_MouseClicked;
        }

        private void PSUI_MouseClicked(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(base.screenList[whichScreen].view.m_DescaledMouse);
            Console.WriteLine("alright, I'm in.");

            //CheckCustomDecodingSize(true);

            /*            Color color = TrackColorCycler.Next();
                        DrawingStyle style = new DrawingStyle();
                        style.Elements.Add("color", new StyleElementColor(color));
                        style.Elements.Add("line size", new StyleElementLineSize(3));
                        style.Elements.Add("track shape", new StyleElementTrackShape(TrackShape.Solid));

                        DrawingTrack track = new DrawingTrack(e.Location, 0, style);
                        track.Status = TrackStatus.Edit;

                        if (DrawingAdding != null)
                            DrawingAdding(this, new DrawingEventArgs(track, m_FrameServer.Metadata.TrackManager.Id));*/
            /*            PlayerScreen ps = new PlayerScreen();
                        ps.view.mnuDirectTrack_Click(sender, e);*/
            //base.ImitateClick(sender, e);
            if (screenList[0] is PlayerScreen)
            {
                PlayerScreen ps = (PlayerScreen)screenList[0];
                ps.view.mnuDirectTrack_Click(sender, e);

                ps.view.m_DescaledMouse.X -= 60;
                ps.view.m_DescaledMouse.Y += 395;
                ps.view.mnuDirectTrack_Click(sender, e);

                ps.view.m_DescaledMouse.X += 115;
                ps.view.mnuDirectTrack_Click(sender, e);
            }

            PlayerScreenUserInterface.MouseClicked -= PSUI_MouseClicked;
        }

        private void VideoScreen_SelectCalibPoint(object sender, MouseEventArgs e)
        {
            //handle the marker placement
            Console.WriteLine(e.Location);
            //and unsub again
            VideoScreenControl.SelectPoint -= VideoScreen_SelectCalibPoint;
        }

        private void ToolStrip_TrajectoryClick(object sender, EventArgs e)
        {
            //I use this way of delegating so that UI component that starts this chain
            //does not require an extra dependency
            //since the screen manager already has access to the function I need with its own inheritance
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
            dashboardScr.panel1.Controls.Add(videoControl);
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
            await Task.Delay(2000);
        }

        public void CreateCaptureScreen()
        {
            videoControl.panel1.Controls.Clear();
            videoControl.panel1.Controls.Add(base.screenList[whichScreen].UI);
        }

        private void CreateVideoScreen()
        {
            dashNav.PopulateButtonToolStrip(videoControl.toolStrip1);
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
