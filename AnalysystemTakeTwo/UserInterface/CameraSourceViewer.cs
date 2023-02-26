using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kinovea.Camera;

namespace AnalysystemTakeTwo
{
    public partial class CameraSourceViewer : UserControl
    {
        ScreenManager screenManager = new ScreenManager();
        private bool firstTime = true;
        List<CameraSummary> summaries = new List<CameraSummary>();
        CameraSummary summy;

        public CameraSourceViewer()
        {
            InitializeComponent();
            //camOption.Hide();

            CameraTypeManager.CamerasDiscovered += CameraTypeManager_CamerasDiscovered;
        }

        private void CameraTypeManager_CamerasDiscovered(object sender, CamerasDiscoveredEventArgs e)
        {
            // This is where we want to add clickable buttons for opening camera & capture - doesn't have to be a mini viewer like Kinovea has
            /*            if (firstTime)
                        {
                            //camOption.Show();

                            screenManager.ShowCamChooser();

                            firstTime = false;
                        }*/

            summaries = e.Summaries;

        }

        private void CameraSourceViewer_Load(object sender, EventArgs e)
        {

        }

        private void camOption_Click (object sender, EventArgs e) 
        {
            //CameraTypeManager.LoadCamera();
            //DoLoadCameraInScreen(e.Source, e.Target);

            //I want to launch camera here by pressing button
            if(summaries != null)
            {
                CameraTypeManager.LoadCamera(summaries[0], -1);
            }
            else
            {
                
            }

            try{
                CameraTypeManager.LoadCamera(summaries[0], -1);
            }
            catch(Exception)
            {
                Console.WriteLine("You don't have a camera plugged in, do you?");
                throw;
            }

        }
    }
}
