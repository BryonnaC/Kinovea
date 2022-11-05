using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncRecording;    //TODO move SyncRecording to the Analysistem namespace
//OR to a more general toolbox namespace -> tbd

namespace Analysistem
{   
    /**
     * This class is the UI's main point of entry.
     * It handles all callback functions and maps to the proper functionality 
     */
    class Controller
    {
        public void StartSimultaneousRecording()
        {
            //TODO put call to clicker.cs in this function
            // PARAMETERS BELOW ARE FOR DEMO PURPOSES
            bool isStart = true; // you set this depending on if you want to start/stop recording
            Data dataCT = SyncR.ClickTargets(isStart); // ClickTargets() for syncing via autoclicker <-- basically just for testing atm
            Data dataR = SyncR.Record(!isStart); // Record() for syncing via Kinovea.Record() and clicking SparkVue <-- the function we will likely actually use
            Data dataRT = SyncR.RecordThreads(isStart); // " <-- same as above but with threads; kinda seems unnecessary

            Console.WriteLine("CT: {0}us -- [ {1}%, {2}% ]", dataCT.delay, dataCT.targets[0].conf, dataCT.targets[1].conf);
            Console.WriteLine("R: {0}us -- [ {1}% ]", dataR.delay, dataR.targets[1].conf);
            Console.WriteLine("RT: {0}us -- [ {1}% ]", dataRT.delay, dataRT.targets[1].conf);
        }
    }
}
