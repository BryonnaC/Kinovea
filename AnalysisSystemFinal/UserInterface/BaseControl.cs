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
    public partial class BaseControl : UserControl
    {
        public BaseControl()
        {
            InitializeComponent();
            MainFrame.WindowResize += OnWindowResize;
        }

        protected void OnWindowResize(object sender, WindowResizeEventArgs e)
        {
            this.Width = e.width;
            this.Height = e.height;

        }
    }
}
