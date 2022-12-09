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
        int sex = 0;
        int age;
        float weightLbs;
        int heightFt;
        int heightIn;
        bool errorCode = false;

        //0 for decline, 1 for male (1st is the worst), 2 for female (second is the best) (the childrens rhyme)
        private const int femSex = 2;
        private const int malSex = 1;
        private const int declineSex = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            determineSex();
            getAge();
            getWeight();
            getHeight();

            if(errorCode == true)
            {
                return;
            }
            else if(errorCode == false)
            {
                saveInfoToFile();
                this.Close();
            }
            errorCode = false;
        }

        private void ageInput_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void saveInfoToFile()
        {

        }

        private void getHeight()
        {
            try
            {
                heightFt = Int32.Parse(feetInput.Text);
                heightIn = Int32.Parse(inchInput.Text);
            }
            catch
            {
                Console.WriteLine("wow, you only exist in two dimensions?");
                errorCode = true;
            }
        }

        private void getWeight()
        {
            if (imperialWeight.Checked)
            {

            }
            else if (metricWeight.Checked)
            {

            }

            try
            {
                weightLbs = float.Parse(lbsInput.Text);
            }
            catch
            {
                Console.WriteLine("YOU BROKE THE SCALE?!?!");
                errorCode = true;
            }
        }

        private void getAge()
        {
            try
            {
                age = Int32.Parse(ageInput.Text);
            }
            catch
            {
                Console.WriteLine("AGE IS A NUMBER");
                errorCode = true;
            }
            
        }

        private void determineSex()
        {
            if (femaleRadio.Checked)
            {
                sex = femSex;
            }
            if (maleRadio.Checked)
            {
                sex = malSex;
            }
            else if (declineRadio.Checked)
            {
                sex = declineSex;
            }
        }
    }
}
