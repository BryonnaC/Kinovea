using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Analysistem
{
    public partial class Form1 : Form
    {
        Controller controller = new Controller();
        public Form1()
        {
            InitializeComponent();
            
        }

        //Record (Simultaneously) button
        private void button1_Click(object sender, EventArgs e)
        {
            controller.StartSimultaneousRecording();
        }
    }
}
