using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Kinovea.Services;

namespace Kinovea.Root
{
    public partial class AnalysistemMainWindow : KinoveaMainWindow
    {
        #region Members
        private RootKernel rootKernel;
        private SupervisorUserInterface supervisorView;
        private bool fullScreen;
        private Rectangle memoBounds;
        private FormWindowState memoWindowState;
        private const string EXTERNAL_COMMAND_IDENTIFIER = "Kinovea";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public AnalysistemMainWindow(RootKernel rootKernel) : base (rootKernel)
        {
            //InitializeComponent();
            log.Debug("Creating main UI window.");

            this.rootKernel = rootKernel;
            InitializeComponent();

            string title = "Analysistem";
            if (!string.IsNullOrEmpty(Software.InstanceName))
                title += string.Format(" [{0}]", Software.InstanceName);

            this.Text = title;

            this.FormClosing += AnalysistemMainWindow_FormClosing;
            supervisorView = new SupervisorUserInterface(rootKernel);
            this.Controls.Add(supervisorView);
            supervisorView.Dock = DockStyle.Fill;
            supervisorView.BringToFront();

            log.DebugFormat("Restoring window state: {0}, window rectangle: {1}", PreferencesManager.GeneralPreferences.WindowState, PreferencesManager.GeneralPreferences.WindowRectangle);
            if (Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(PreferencesManager.GeneralPreferences.WindowRectangle)))
            {
                // The screen it was on is still here, move it to this screen and then restore the state.
                this.StartPosition = FormStartPosition.Manual;
                this.DesktopBounds = PreferencesManager.GeneralPreferences.WindowRectangle;
                this.WindowState = PreferencesManager.GeneralPreferences.WindowState;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }

            EnableCopyData();
        }

        private void AnalysistemMainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            PreferencesManager.GeneralPreferences.WindowState = this.WindowState;
            PreferencesManager.GeneralPreferences.WindowRectangle = this.DesktopBounds;
            PreferencesManager.Save();
        }

        private void UserInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = rootKernel.CloseSubModules();
        }

        private void EnableCopyData()
        {
            NativeMethods.CHANGEFILTERSTRUCT changeFilter = new NativeMethods.CHANGEFILTERSTRUCT();
            changeFilter.size = (uint)Marshal.SizeOf(changeFilter);
            changeFilter.info = 0;
            if (!NativeMethods.ChangeWindowMessageFilterEx(this.Handle, NativeMethods.WM_COPYDATA, NativeMethods.ChangeWindowMessageFilterExAction.Allow, ref changeFilter))
            {
                int error = Marshal.GetLastWin32Error();
                log.ErrorFormat("Error while trying to enable WM_COPYDATA: {0}", error);
            }
        }
    }
}
