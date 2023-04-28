using System;
using System.IO;
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
    public partial class CalibrationObjectDimensions : UserControl
    {
        bool errorCode = false; //ERROR CODE HANDLING CAN BECOME A CLASS/METHOD because I'm already using it twice
        string toBeWritten = "";
        float heightDim;
        float lengthDim;
        float widthDim;

        public CalibrationObjectDimensions()
        {
            InitializeComponent();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            toBeWritten += getLength();
            toBeWritten += getHeight();
            toBeWritten += getWidth();

/*            if (errorCode == true)
            {
                ErrorMessage em = new ErrorMessage();
                em.Show();
                errorCode = false;
                return;
            }*/
            //else 
            if (errorCode == false)
            {
                saveInfoToFile();
                //this.Close();
            }
        }

        private string getLength()
        {
            try
            {
                lengthDim = float.Parse(heightBox.Text);
                return lengthDim.ToString() + " in/cm\n";
            }
            catch
            {
                Console.WriteLine("bad input length");
                errorCode = true;
            }

            return null;
        }

        private string getHeight()
        {
            try
            {
                heightDim = float.Parse(heightBox.Text);
                return heightDim.ToString() + " in/cm\n";
            }
            catch
            {
                Console.WriteLine("bad input height");
                errorCode = true;
            }

            return null;
        }

        private string getWidth()
        {
            try
            {
                widthDim = float.Parse(heightBox.Text);
                return widthDim.ToString() + " in/cm\n";
            }
            catch
            {
                Console.WriteLine("bad input width");
                errorCode = true;
            }

            return null;
        }

        private void saveInfoToFile()
        {
            saveFileDialog1.ShowDialog();
            saveFileDialog1.Dispose();
        }

        private void lengthCm_CheckedChanged(object sender, EventArgs e)
        {
            widthCm.Checked = true;
            heightCm.Checked = true;
        }

        private void lengthIn_CheckedChanged(object sender, EventArgs e)
        {
            widthIn.Checked = true;
            heightIn.Checked = true;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            File.WriteAllText(saveFileDialog1.FileName, toBeWritten);
            Console.WriteLine(File.ReadAllText(saveFileDialog1.FileName));
        }
    }
}
