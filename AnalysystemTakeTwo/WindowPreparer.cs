using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysystemTakeTwo
{
    class WindowPreparer
    {
        private MainFrame mainWindow;
        private ChooseAScreen firstControl;
        public void OnInit()
        {
            mainWindow.Controls.Add(firstControl);
        }

    }
}
