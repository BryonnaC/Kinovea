﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnalysisSystemFinal;

namespace AnalysystemTakeTwo
{
    public partial class CustomSplashscreen : UserControl
    {
        public static event Action StartDashboard;

        public CustomSplashscreen()
        {
            InitializeComponent();
            MainFrame.WindowResize += OnWindowResize;
        }
        protected void OnWindowResize(object sender, WindowResizeEventArgs e)
        {
            this.Width = e.width;
            this.Height = e.height;

        }
        private void goButton_Click(object sender, EventArgs e)
        {
            StartDashboard?.Invoke();
        }
    }
}
