using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisSystemFinal
{
    public class ButtonClickedEventArgs : EventArgs
    {
        public readonly int buttonNumber;

        public ButtonClickedEventArgs(int button_number)
        {
            this.buttonNumber = button_number;
        }
    }

    public class WindowResizeEventArgs : EventArgs
    {
        public readonly int width;
        public readonly int height;

        public WindowResizeEventArgs(int w, int h)
        {
            this.width = w;
            this.height = h;
        }
    }

    class NaviClickEventArgs : EventArgs
    {
        public readonly int navigationChoice;

        public NaviClickEventArgs(int naviChoice)
        {
            this.navigationChoice = naviChoice;
        }
    }
}
