using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using Analysistem.Utils;
using System.IO;
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
        public static bool isStart = true;

        protected void RunDemo(MyFunction recordingFunction)
        {
            List<Marker> frontMarkers = new List<Marker>() {
                new Marker(256.98, -201.43),
                new Marker(98.80, -631.48),
                new Marker(422.57, -574.63),
                new Marker(254.40, -315.12),
            };
            List<Marker> sideMarkers = new List<Marker>() {
                new Marker(-516.62, -201.43),
                new Marker(-588.29, -572.16),
                new Marker(-373.27, -648.78),
                new Marker(-499.32, -305.24),
            };

            Markers markers = new Markers(frontMarkers, sideMarkers);
            markers.NormalizeAndCentralizeCoords();

            Console.WriteLine(markers.frontMarkers[0].x);
            Console.WriteLine(markers.frontMarkers[0].y);
            Console.WriteLine(markers.frontMarkers[1].x);
            Console.WriteLine(markers.frontMarkers[1].y);

            //FakeUser.PressKey('a');

            //FakeUser.PressCaptureScreenHotkey();
            // THESE SHOULD GO BACK //Record(isStart);
            // THESE SHOULD GO BACK //isStart = !isStart;

            // if it doesn't work (but doesn't throw an error), try increasing timeToOpenBurger or timeToOpenFileExplorer in FileHandler.cs
            //EventInfo info = ExportSparkvue();   // TODO: this line is throwing an exception still
            //Console.WriteLine(data.fileName);

            #region CsvFile DEMO
            //CsvFile kinovea = new CsvFile("C:\\Users\\col_b\\OneDrive\\Documents\\_PSU\\cmspc484\\test_data_demo\\demo_motion_y.csv");
            //CsvFile sparkvue = new CsvFile("C:\\Users\\col_b\\OneDrive\\Documents\\_PSU\\cmspc484\\test_data_demo\\force_demo_112922.csv");
            //CsvFile syncKinovea = Synchronizer.MakeCoincident(kinovea, sparkvue);
            ////Console.WriteLine(syncKinovea.ToString());
            //File.WriteAllText("C:\\Users\\col_b\\OneDrive\\Documents\\_PSU\\cmspc484\\test_data_demo\\syncKinovea.csv", syncKinovea.Serialize());

            /*CsvFile testFile = new CsvFile("C:\\Users\\col_b\\Downloads\\test_x.csv", "C:\\Users\\col_b\\Downloads\\test_y.csv", new string[] { });

            Console.WriteLine($"\nCsvFile:\n{testFile}");

            IEnumerable<string> serial = testFile.Serialize();
            Console.WriteLine($"Serialized:\n{string.Join("\n", serial)}\n");
            testFile.Load(null, serial);

            Console.WriteLine($"Deserialized:\n{testFile}");*/
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

        protected UserControl1 GetUserInformationForm()
        {
            UserControl1 infoForm = new UserControl1();

            return infoForm;
        }

        protected void InfoWindow()
        {
            Form1 infoDemo = new Form1();
            infoDemo.Show();
            Console.WriteLine("Happy demo-ing!");
        }

        protected void CalibrateWindow()
        {
            CalibrationWindow calWin = new CalibrationWindow();
            calWin.Show();
            Console.WriteLine("Happy cal-demo-ing!");
        }
    }
}
