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

    public class NaviClickEventArgs : EventArgs
    {
        public readonly int navigationChoice;

        public NaviClickEventArgs(int naviChoice)
        {
            this.navigationChoice = naviChoice;
        }
    }

    public class ForceFileEventArgs : EventArgs
    {
        public readonly string filePath;

        public ForceFileEventArgs(string path)
        {
            this.filePath = path;
        }
    }

    public class PositionFileEventArgs : EventArgs
    {
        public readonly string horizPath;
        public readonly string vertPath;

        public PositionFileEventArgs(string hpath, string vpath)
        {
            this.horizPath = hpath;
            this.vertPath = vpath;
        }
    }
}
