using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analysistem;
using Kinovea.ScreenManager;

namespace Kinovea.Root
{
    class AnalysisController : Controller
    {
        //Controller controller = new Controller();
        //public delegate void MyFunction();
        CaptureScreen capScreen = new CaptureScreen();
        UserControl1 infoForm;
        
        public void TestClick()
        {
            //will probably need to put call to start Kinovea recording in here
            //because anything in analysistem project/namespace will not be able to access it
            // can we pass it in as a callback?
            // e.g., base.RunDemo(StartRecording); then just pass it through to our Record?
            base.RunDemo(capScreen.View_ToggleRecording);

            //capScreen.View_ToggleRecording();
            
            Console.WriteLine("hey cloin");
        }

        public UserControl1 InformationClick()
        {
            infoForm = base.GetUserInformationForm();
            Console.WriteLine("We're getting the form!");
            return infoForm;
        }

        public void InfoDemoClick()
        {
            base.InfoWindow();
        }

        public void CalibrateDemoClick()
        {
            base.CalibrateWindow();
        }
    }
}
