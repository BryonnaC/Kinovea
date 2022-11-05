using System;
using SynchRecording;    //TODO move SyncRecording to the Analysistem namespace
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
            #region SynchRecording DEMO
            bool isStart = true; // you set this depending on if you want to start/stop recording
            Data dataROn = Synchr.Record(isStart); // Record() for syncing via Kinovea.Record() and clicking SparkVue <-- the function we will likely actually use
            Data dataROff = Synchr.Record(!isStart); // "
            Data dataRTOn = Synchr.RecordThreads(isStart); // " <-- same as above but with threads; needs thorough comparison/further optimization if possible
            Data dataRTOff = Synchr.RecordThreads(!isStart); // "

            Console.WriteLine("ROn: {0}us -- [ {1}% ]", dataROn.delay, dataROn.target.confidence);
            Console.WriteLine("ROff: {0}us -- [ {1}% ]", dataROff.delay, dataROff.target.confidence);
            Console.WriteLine("RTOn: {0}us -- [ {1}% ]", dataRTOn.delay, dataRTOn.target.confidence);
            Console.WriteLine("RTOff: {0}us -- [ {1}% ]", dataRTOff.delay, dataRTOff.target.confidence);
            #endregion
        }

        // play with the Data elsewhere
        //public Data StartSimultaneousRecording()
        //{
        //    return Synchr.StartRecording();
        //}
        // OR
        //public void StartSimultaneousRecording()
        //{
        //    (Target sparkvueInfo, double delay) = Synchr.StartRecording();
        //    // play with the Data here!
        //}
        // same goes for stopping => Synchr.StopRecording()
    }
}
