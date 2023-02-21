using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysystemTakeTwo.UserInterface
{
    class NaviClickEventArgs : EventArgs
    {
        public readonly int navigationChoice;

        public NaviClickEventArgs(int naviChoice)
        {
            this.navigationChoice = naviChoice;
        }
    }
}
