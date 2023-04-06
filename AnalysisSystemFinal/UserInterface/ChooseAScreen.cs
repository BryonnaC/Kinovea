using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kinovea.ScreenManager;
using Kinovea.Camera;
using AnalysisSystemFinal;

namespace AnalysystemTakeTwo
{
    public partial class ChooseAScreen : UserControl
    {
        //ScreenManager screenManger = new ScreenManager();
        public static event EventHandler<ButtonClickedEventArgs> ButtonClicked;
        private int buttonClicked = 0;

        public ChooseAScreen()
        {
            InitializeComponent();
        }

        private void LiveVideo_Click(object sender, EventArgs e)
        {
            Console.WriteLine("You want /live/ video? Okay then.");

            CameraTypeManager.StartDiscoveringCameras();
            
            //screenManger.ShowCamChooser();

            buttonClicked = 1;
            ButtonClicked?.Invoke(this, new ButtonClickedEventArgs(buttonClicked));
        }

        private void SelectVideo_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Pre-recorded it is.");

            buttonClicked = 2;
            ButtonClicked?.Invoke(this, new ButtonClickedEventArgs(buttonClicked));
        }

        private void CalibrationObject_Click(object sender, EventArgs e)
        {
            Console.WriteLine("I don't know if this button belongs here, but here she lay for now. \n" +
                "You wanna enter calibration dimensions");

            buttonClicked = 3;
            ButtonClicked?.Invoke(this, new ButtonClickedEventArgs(buttonClicked));
        }

        private void IntakeInfo_Click(object sender, EventArgs e)
        {
            Console.WriteLine("I also don't think this button belongs next to the top ones, but info collection ahoy");
            //screenManger.HideCurrentScreen();
            //screenManger.ShowSubjectInfoScreen();

            buttonClicked = 4;
            ButtonClicked?.Invoke(this, new ButtonClickedEventArgs(buttonClicked));
        }

        private void ChooseAScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
