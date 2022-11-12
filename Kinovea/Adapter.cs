using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analysistem;

namespace Kinovea.Root
{
    class Adapter : Controller
    {
        //Controller controller = new Controller();

        public void TestClick()
        {
            base.StartSimultaneousRecording();
        }
    }
}
