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
    public partial class UserControl1 : UserControl
    {
        int age;
        int heightFt;
        int heightIn;
        float weightLbs;
        float weightKgs;
        int sex;
        //0 for decline, 1 for male (1st is the worst), 2 for female (second is the best) (the childrens rhyme)
        private const int femSex = 2;
        private const int malSex = 1;
        private const int declineSex = 0;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            age = Int32.Parse(ageInput.Text);
            heightFt = Int32.Parse(feetInput.Text);
            heightIn = Int32.Parse(inchInput.Text);
            weightLbs = float.Parse(lbsInput.Text);

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

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
