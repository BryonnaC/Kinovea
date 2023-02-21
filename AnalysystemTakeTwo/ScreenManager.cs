﻿using System;
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

namespace AnalysystemTakeTwo
{
    class ScreenManager : ScreenManagerKernel
    {
        MainFrame mainFrame;
        ChooseAScreen chooseScreen = new ChooseAScreen();
        RecordingControl recordingControl = new RecordingControl();
        CameraSourceViewer availableCamerasScreen;
        CustomSplashscreen customSplash = new CustomSplashscreen();
        DashboardControl dashboardScr = new DashboardControl();
        //ScreenManagerKernel smKernel;
        //CameraSourceViewer availableCamerasScreen;

        //TODO should make state machine for UI windows

        public ScreenManager()
        {
            //smKernel = new ScreenManagerKernel();
            mainFrame = new MainFrame();
            LoadSplashScreen();
            mainFrame.Show();

            FormsHelper.SetMainForm(mainFrame);

            CameraTypeManager.CameraLoadAsked += CameraTypeManager_CameraLoadAsked;
            ChooseAScreen.ButtonClicked += ChooseAScreen_ButtonClicked;
            CustomSplashscreen.StartDashboard += CustomSplashscreen_StartDashboard;
            DashboardNavigation.ToolBarClick += DashboardNav_ToolBarClick;
            VideoTypeManager.VideoLoadAsked += VideoTypeManager_VideoLoadAsked;
            RecordingControl.ReturnToDash += RecordingControl_ReturnToDash;

            //Application.Run();
        }

        #region EventHandlers
        private void DashboardNav_ToolBarClick(object sender, ButtonClickedEventArgs e)
        {
            switch (e.buttonNumber)
            {
                case 1:
                case 2:
                case 3:
                    return;
                case 4:
                    PrepareFileBrowser();
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
            mainFrame.Controls.Clear();
            mainFrame.Controls.Add(recordingControl);
            DoLoadCameraInScreen(e.Source, e.Target);
        }

        //this was for testing
        private void ChooseAScreen_ButtonClicked(object sender, ButtonClickedEventArgs e)
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
        }

        new private void VideoTypeManager_VideoLoadAsked(object sender, VideoLoadAskedEventArgs e)
        {
            mainFrame.Controls.Clear();
            mainFrame.Controls.Add(recordingControl);
            DoLoadMovieInScreen(e.Path, e.Target);
        }
        #endregion

        #region Not Event Handlers
        private void LoadSplashScreen()
        {
            mainFrame.Controls.Add(customSplash);
        }

        public void CreateCaptureScreen()
        {
            recordingControl.panel1.Controls.Clear();
            recordingControl.panel1.Controls.Add(base.screenList[0].UI);
        }

        private void CreateVideoScreen()
        {
            recordingControl.panel1.Controls.Clear();
            recordingControl.panel1.Controls.Add(base.screenList[0].UI);
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

        public void ShowSubjectInfoScreen()
        {
            SubjectInformationScreen subInfoScr = new SubjectInformationScreen();
            mainFrame.Controls.Remove(chooseScreen);
            mainFrame.Controls.Add(subInfoScr);
        }

        public void HideCurrentScreen()
        {
            mainFrame.Controls.Clear();
        }

        private void FirstSwitchToDashboard()
        {
            DashboardNavigation dashNav = new DashboardNavigation();
            dashNav.PopulateToolBar(dashboardScr.toolStrip1);

            ScreenManagerUserInterface scrMgUI = new ScreenManagerUserInterface();

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
            dashboardScr.panel1.Controls.Clear();
            dashboardScr.panel1.Controls.Add(fileBrowsUI);
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
