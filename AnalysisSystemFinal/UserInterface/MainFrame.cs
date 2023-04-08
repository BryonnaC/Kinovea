using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnalysisSystemFinal;

namespace AnalysystemTakeTwo
{
    public partial class MainFrame : Form
    {
        public static event EventHandler<WindowResizeEventArgs> WindowResize;
        public MainFrame()
        {
            InitializeComponent();
            //System.Windows.Forms.Form.WindowState = FormWindowState.Maximized;
            this.Text = "Biomechanical Analysis Suite";
        }

        private void MainFrame_Load(object sender, EventArgs e)
        {

        }

        private void MainFrame_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainFrame_SizeChanged(object sender, EventArgs e)
        {
            WindowResize?.Invoke(this, new WindowResizeEventArgs(this.Width, this.Height));
        }
    }
}
