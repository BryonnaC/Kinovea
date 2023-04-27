using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysisSystemFinal.UserInterface
{
    public partial class ForceSelector : Form
    {
        public static event EventHandler<ForceFileEventArgs> ImportForce;

        public ForceSelector()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ImportForce?.Invoke(sender, new ForceFileEventArgs(openFileDialog1.FileName));
            this.textBox1.Text = openFileDialog1.FileName;
        }

        private void openForce_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            openFileDialog1.Dispose();
        }
    }
}
