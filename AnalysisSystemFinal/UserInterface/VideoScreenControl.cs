using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnalysystemTakeTwo;

namespace AnalysisSystemFinal.UserInterface
{
    public partial class VideoScreenControl : UserControl
    {
        public static event EventHandler<MouseEventArgs> SelectPoint;

        public VideoScreenControl()
        {
            InitializeComponent();
            MainFrame.WindowResize += OnWindowResize;
        }

        protected void OnWindowResize(object sender, WindowResizeEventArgs e)
        {
            panel1.Height = SizeHelper.GetNewDimension(this.Height, e.height, panel1.Height);
            panel1.Width = SizeHelper.GetNewDimension(this.Width, e.width, panel1.Width);

            this.Width = e.width;
            this.Height = e.height;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //get a mouse click so we can try to figure out where to place a calibration object in one click
            SelectPoint?.Invoke(sender, e);
        }
    }
}
