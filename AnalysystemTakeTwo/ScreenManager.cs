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
        MainFrame mainFrame = new MainFrame();
        CustomCaptureScreen customCapScr = new CustomCaptureScreen();
        //ScreenManagerKernel smKernel;
        //CameraSourceViewer availableCamerasScreen;

        public ScreenManager()
        {
            //smKernel = new ScreenManagerKernel();
            CameraTypeManager.CameraLoadAsked += CameraTypeManager_CameraLoadAsked;
        }

        public void CreateCaptureScreen()
        {
            mainFrame.Controls.Add(base.screenList[0].UI);
            mainFrame.Show();
        }

        private void CameraTypeManager_CameraLoadAsked(object source, CameraLoadAskedEventArgs e)
        {
            DoLoadCameraInScreen(e.Source, e.Target);
        }

        new private void DoLoadCameraInScreen(CameraSummary summary, int targetScreen)
        {
            /*original code from Kinovea UI
            if (summary == null)
                return;
            LoaderCamera.LoadCameraInScreen(this, summary, targetScreen);*//*

            if(summary == null)
            {
                return;
            }

            //else load camera me boy
            customCapScr.LoadCamera(summary, null);*/
            base.DoLoadCameraInScreen(summary, targetScreen);
            CreateCaptureScreen();
        }

        public void ShowInitialScreen()
        {
            //Build screen
            ChooseAScreen chooseScreen = new ChooseAScreen();
/*            if(availableCamerasScreen == null)
            {
                availableCamerasScreen = new CameraSourceViewer();
            }*/

            mainFrame.Controls.Add(chooseScreen);

            mainFrame.Show();
        }

        public void ShowCamChooser()
        {
            //mainFrame.Controls.Remove()
            CameraSourceViewer availableCamerasScreen = new CameraSourceViewer();
            mainFrame.Controls.Add(availableCamerasScreen);

            mainFrame.Show();
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
            //mainFrame.Hide();
            mainFrame.Show();
        }

        public void HideCurrentScreen()
        {
            //Clears all controls, not the most useful long term, but fine for now
            mainFrame.Controls.Clear();
            mainFrame.Show();
        }
    }
}
