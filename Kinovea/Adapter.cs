﻿using System;
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
            //will probably need to put call to start Kinovea recording in here
            //because anything in analysistem project/namespace will not be able to access it
            base.StartSimultaneousRecording();
        }
    }
}
