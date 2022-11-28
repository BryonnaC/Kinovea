using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using Analysistem.Utils;
using static Analysistem.Synchronizer;
using static Analysistem.FileHandler;

namespace Analysistem
{
    /**
     * This class is the UI's main point of entry.
     * It handles all callback functions and maps to the proper functionality 
     */
    public class Controller
    {
        public delegate void MyFunction();

        public void RunDemo(MyFunction recordingFunction)
        {
            //FakeUser.PressKey('a');

            FakeUser.PressCaptureScreenHotkey();

            // if it doesn't work (but doesn't throw an error), try increasing timeToOpenBurger or timeToOpenFileExplorer in FileHandler.cs
            //EventInfo info = ExportSparkvue();   // TODO: this line is throwing an exception still
            //Console.WriteLine(data.fileName);

            #region CsvFile DEMO
            //CsvFile testFile = new CsvFile("C:\\Users\\col_b\\Downloads\\test_x.csv", "C:\\Users\\col_b\\Downloads\\test_y.csv", new string[] { });

            //Console.WriteLine($"\nCsvFile:\n{testFile}");

            //IEnumerable<string> serial = testFile.Serialize();
            //Console.WriteLine($"Serialized:\n{string.Join("\n", serial)}\n");
            //testFile.Load(null, serial);

            //Console.WriteLine($"Deserialized:\n{testFile}");
            #endregion

            #region Synchronize:Record DEMO
            //bool isStart = true; // you set this depending on if you want to start/stop recording
            //EventInfo dataROn = Record(isStart); // Record() for syncing via Kinovea.Record() and clicking SparkVue <-- the function we will likely actually use
            //EventInfo dataROff = Record(!isStart); // "
            //EventInfo dataRTOn = RecordThreads(isStart); // " <-- same as above but with threads; needs thorough comparison/further optimization if possible
            //EventInfo dataRTOff = RecordThreads(!isStart); // "

            //Console.WriteLine("ROn: {0}us -- [ {1}% ]", dataROn.delay, dataROn.target.confidence);
            //Console.WriteLine("ROff: {0}us -- [ {1}% ]", dataROff.delay, dataROff.target.confidence);
            //Console.WriteLine("RTOn: {0}us -- [ {1}% ]", dataRTOn.delay, dataRTOn.target.confidence);
            //Console.WriteLine("RTOff: {0}us -- [ {1}% ]", dataRTOff.delay, dataRTOff.target.confidence);

            //(Target[] targets, double delay) = Record(true, Units.Nanoseconds);
            //(Point location, double value, bool detected, double confidence) = targets[0];
            //Console.WriteLine("Full Deconstruction: {4}ns -- [ {0} -- {1} -- {3}% -- {2} ]", location, value, detected, confidence, delay);
            #endregion
        }

        // play with the Data elsewhere
        //public EventInfo StartSimultaneousRecording()
        //{
        //    return Record(true);
        //}
        // OR
        //public void StartSimultaneousRecording()
        //{
        //    (Target sparkvueInfo, double delay) = Record(true);
        //    // play with the Data here!
        //}
        // same goes for stopping => Record(false)
    }
}
