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
    public partial class RecordingControl : UserControl
    {
        public static event Action ReturnToDash;

        public RecordingControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReturnToDash?.Invoke();
        }
    }
}
