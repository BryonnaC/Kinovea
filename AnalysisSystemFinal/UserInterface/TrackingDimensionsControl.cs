using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kinovea.Services;

namespace AnalysisSystemFinal.UserInterface
{
    public partial class TrackingDimensionsControl : UserControl
    {
        private TrackingProfile trackingProfile;

        public TrackingDimensionsControl()
        {
            InitializeComponent();

            cmbBlockWindowUnit.Items.Add("Percentage");
            cmbBlockWindowUnit.Items.Add("Pixels");
            cmbSearchWindowUnit.Items.Add("Percentage");
            cmbSearchWindowUnit.Items.Add("Pixels");

            trackingProfile = PreferencesManager.PlayerPreferences.TrackingProfile;
        }

        private void cmbBlockWindowUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackingProfile.BlockWindowUnit = (TrackerParameterUnit)cmbBlockWindowUnit.SelectedIndex;
        }

        private void cmbSearchWindowUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackingProfile.SearchWindowUnit = (TrackerParameterUnit)cmbSearchWindowUnit.SelectedIndex;
        }

        private void tbBlockWidth_TextChanged(object sender, EventArgs e)
        {
            int width;
            bool parsed = ExtractTrackerParameter(tbBlockWidth, out width);
            if (!parsed)
                return;

            trackingProfile.BlockWindow = new Size(width, trackingProfile.BlockWindow.Height);
        }

        private void tbBlockHeight_TextChanged(object sender, EventArgs e)
        {
            int height;
            bool parsed = ExtractTrackerParameter(tbBlockHeight, out height);
            if (!parsed)
                return;

            trackingProfile.BlockWindow = new Size(trackingProfile.BlockWindow.Width, height);
        }

        private void tbSearchWidth_TextChanged(object sender, EventArgs e)
        {
            int width;
            bool parsed = ExtractTrackerParameter(tbSearchWidth, out width);
            if (!parsed)
                return;

            trackingProfile.SearchWindow = new Size(width, trackingProfile.SearchWindow.Height);
        }

        private void tbSearchHeight_TextChanged(object sender, EventArgs e)
        {
            int height;
            bool parsed = ExtractTrackerParameter(tbSearchHeight, out height);
            if (!parsed)
                return;

            trackingProfile.SearchWindow = new Size(trackingProfile.SearchWindow.Width, height);
        }

        private bool ExtractTrackerParameter(TextBox tb, out int value)
        {
            int v;
            bool parsed = int.TryParse(tb.Text, out v);
            tbBlockWidth.ForeColor = parsed ? Color.Black : Color.Red;
            value = parsed ? v : 10;
            return parsed;
        }

        private void save_Click(object sender, EventArgs e)
        {
            PreferencesManager.PlayerPreferences.TrackingProfile = trackingProfile;
        }
    }
}
