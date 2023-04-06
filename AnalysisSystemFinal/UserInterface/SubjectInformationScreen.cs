using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysystemTakeTwo
{
    public partial class SubjectInformationScreen : UserControl
    {
        public SubjectInformationScreen()
        {
            InitializeComponent();
        }

        int sex = 0;
        int age;
        float weightLbs;
        float weightKgs;
        int heightFt;
        int heightIn;
        float heightCm;
        bool errorCode = false;
        string toBeWritten = "";

        //0 for decline, 1 for male (1st is the worst), 2 for female (second is the best) (the childrens rhyme)
        private const int femSex = 2;
        private const int malSex = 1;
        private const int declineSex = 0;

        private void nextButton_Click(object sender, EventArgs e)
        {
            toBeWritten += determineSex();
            toBeWritten += getAge();
            toBeWritten += getWeight();
            toBeWritten += getHeight();

            if (errorCode == true)
            {
/*                ErrorMessage em = new ErrorMessage();
                em.Show();
                errorCode = false;*/
                return;
            }
            else if (errorCode == false)
            {
                saveInfoToFile();
                //this.Close();
            }

        }

        private void saveInfoToFile()
        {
            string desiredFileName = @"C:\Users\bryy_\Documents\informationCollection.txt";

            File.WriteAllText(desiredFileName, toBeWritten);
            Console.WriteLine(File.ReadAllText(desiredFileName));
        }

        private string getHeight()
        {
            if (imperialHeight.Checked)
            {
                try
                {
                    heightFt = Int32.Parse(feetInput.Text);
                    heightIn = Int32.Parse(inchInput.Text);
                    return heightFt.ToString()
                        + " ft " + heightIn.ToString() + " in \n";
                }
                catch
                {
                    Console.WriteLine("wow, you only exist in two dimensions?");
                    errorCode = true;
                }
            }
            else if (metricHeight.Checked)
            {
                try
                {
                    heightCm = float.Parse(feetInput.Text);
                    return heightCm.ToString() + " cm \n";
                }
                catch
                {
                    Console.WriteLine("wow, you only exist in two dimensions?");
                    errorCode = true;
                }
            }

            return null;

        }

        private string getWeight()
        {
            if (imperialWeight.Checked)
            {
                try
                {
                    weightKgs = 0.0f;
                    weightLbs = float.Parse(weightInput.Text);
                    return weightLbs.ToString() + " lbs \n";
                }
                catch
                {
                    Console.WriteLine("YOU BROKE THE SCALE?!?!");
                    errorCode = true;
                }
            }
            else if (metricWeight.Checked)
            {
                try
                {
                    weightLbs = 0.0f;
                    weightKgs = float.Parse(weightInput.Text);
                    return weightKgs.ToString() + " kgs \n";
                }
                catch
                {
                    Console.WriteLine("YOU BROKE THE SCALE?!?!");
                    errorCode = true;
                }
            }

            return null;
        }

        private string getAge()
        {
            try
            {
                age = Int32.Parse(ageInput.Text);
                return age.ToString() + " yrs \n";
            }
            catch
            {
                Console.WriteLine("AGE IS A NUMBER");
                errorCode = true;
            }

            return null;
        }

        private string determineSex()
        {
            if (femaleRadio.Checked)
            {
                sex = femSex;
                return "Female \n";
            }
            if (maleRadio.Checked)
            {
                sex = malSex;
                return "Male \n";
            }
            else if (declineRadio.Checked)
            {
                sex = declineSex;
                return "Decline to Answer Sex \n";
            }

            return null;
        }

        private void metricHeight_CheckedChanged_1(object sender, EventArgs e)
        {
            inchInput.Hide();
        }

        private void imperialHeight_CheckedChanged_1(object sender, EventArgs e)
        {
            inchInput.Show();
        }

        private void ageInput_Click(object sender, EventArgs e)
        {
            ageInput.Text = "";
        }

        private void weightInput_Click(object sender, EventArgs e)
        {
            weightInput.Text = "";
        }

        private void feetInput_Click_1(object sender, EventArgs e)
        {
            feetInput.Text = "";
        }

        private void inchInput_Click_1(object sender, EventArgs e)
        {
            inchInput.Text = "";
        }

        private void inchInput_TabIndexChanged_1(object sender, EventArgs e)
        {
            inchInput.Text = "";
        }
    }
}
