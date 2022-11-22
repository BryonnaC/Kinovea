using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using Analysistem.Utils;
using static Analysistem.Synchronizer;
using static Analysistem.FileHandler;
using static Analysistem.Utils.FakeUser; // ONLY FOR TESTING, unless we find a scenario where having a utils class 
                                         // as a dependency is absolutely necessary, we should avoid it on principle

namespace Analysistem
{
    /**
     * This class is the UI's main point of entry.
     * It handles all callback functions and maps to the proper functionality 
     */
    public class Controller
    {
        public void RunDemo()
        {
            //PressKey('a');

            // if it doesn't work (but doesn't throw an error), try increasing timeToOpenBurger in FileHandler.cs
            Data data = ExportSparkvue();   //TODO this line is throwing an exception still
            Console.WriteLine(data.fileName);

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
            //Data dataROn = Record(isStart); // Record() for syncing via Kinovea.Record() and clicking SparkVue <-- the function we will likely actually use
            //Data dataROff = Record(!isStart); // "
            //Data dataRTOn = RecordThreads(isStart); // " <-- same as above but with threads; needs thorough comparison/further optimization if possible
            //Data dataRTOff = RecordThreads(!isStart); // "

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
        //public Data StartSimultaneousRecording()
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
