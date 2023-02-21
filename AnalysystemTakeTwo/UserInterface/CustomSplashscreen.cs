using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysystemTakeTwo
{
    public partial class CustomSplashscreen : UserControl
    {
        public static event Action StartDashboard;

        public CustomSplashscreen()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            StartDashboard?.Invoke();
        }
    }
}
