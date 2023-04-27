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
    public partial class PositionDataSelection : Form
    {
        private bool isHorizontal;
        public static event EventHandler<PositionFileEventArgs> ImportPositionData;
        public PositionDataSelection()
        {
            InitializeComponent();
        }

        private void openHoriz_click(object sender, EventArgs e)
        {
            isHorizontal = true;
            openFileDialog1.ShowDialog();
            openFileDialog1.Dispose();
        }

        private void openVert_Click(object sender, EventArgs e)
        {
            isHorizontal = false;
            openFileDialog1.ShowDialog();
            openFileDialog1.Dispose();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (isHorizontal)
            {
                this.textBox1.Text = openFileDialog1.FileName;
            }
            else
            {
                this.textBox2.Text = openFileDialog1.FileName;
            }

            if (!this.textBox1.Text.Equals(null) && !this.textBox2.Text.Equals(null))
            {
                ImportPositionData?.Invoke(sender, new PositionFileEventArgs(textBox1.Text, textBox2.Text));
            }
            else
            {
                return;
            }

        }
    }
}
