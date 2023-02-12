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

        }

        private void CameraSourceViewer_Load(object sender, EventArgs e)
        {

        }
    }
}
