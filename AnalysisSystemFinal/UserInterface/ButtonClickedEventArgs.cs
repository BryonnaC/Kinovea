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
}
