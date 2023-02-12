using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysystemTakeTwo
{
    class ScreenManager
    {
        MainFrame mainFrame = new MainFrame();
        CameraSourceViewer availableCamerasScreen;

        public void ShowInitialScreen()
        {
            //Build screen
            ChooseAScreen chooseScreen = new ChooseAScreen();
            if(availableCamerasScreen == null)
            {
                availableCamerasScreen = new CameraSourceViewer();
            }

            mainFrame.Controls.Add(chooseScreen);

            mainFrame.Show();
        }

        public void ShowCamChooser()
        {
            //mainFrame.Controls.Remove()
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
