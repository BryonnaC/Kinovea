using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinovea.Camera;
using Kinovea.ScreenManager;

namespace AnalysystemTakeTwo
{
    class ScreenManager : ScreenManagerKernel
    {
        MainFrame mainFrame;
        ChooseAScreen chooseScreen = new ChooseAScreen();
        CustomCaptureScreen customCapScr = new CustomCaptureScreen();
        CameraSourceViewer availableCamerasScreen;
        //ScreenManagerKernel smKernel;
        //CameraSourceViewer availableCamerasScreen;

        public ScreenManager()
        {
            //smKernel = new ScreenManagerKernel();
            mainFrame = new MainFrame();
            CameraTypeManager.CameraLoadAsked += CameraTypeManager_CameraLoadAsked;
            ChooseAScreen.ButtonClicked += ChooseAScreen_ButtonClicked;
        }

        public void CreateCaptureScreen()
        {
            mainFrame.Controls.Add(base.screenList[0].UI);
        }

        private void CameraTypeManager_CameraLoadAsked(object source, CameraLoadAskedEventArgs e)
        {
            DoLoadCameraInScreen(e.Source, e.Target);
        }

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

        new private void DoLoadCameraInScreen(CameraSummary summary, int targetScreen)
        {
            mainFrame.Controls.Remove(availableCamerasScreen);
            base.DoLoadCameraInScreen(summary, targetScreen);
            CreateCaptureScreen();
        }

        public void ShowInitialScreen()
        {
            mainFrame.Controls.Add(chooseScreen);

            mainFrame.Show();
        }

        public void ShowCamChooser()
        {
            mainFrame.Controls.Remove(chooseScreen);
            availableCamerasScreen = new CameraSourceViewer();
            mainFrame.Controls.Add(availableCamerasScreen);
        }

        public void ShowCaptureScreen()
        {

        }

        public void ShowPlaybackScreen()
        {

        }

        public void ShowSubjectInfoScreen()
        {
            SubjectInformationScreen subInfoScr = new SubjectInformationScreen();
            mainFrame.Controls.Add(subInfoScr);
        }

        public void HideCurrentScreen()
        {
            //Clears all controls, not the most useful long term, but fine for now
            mainFrame.Controls.Clear();
            mainFrame.Show();
        }
    }
}
