using Analysistem.Utils;
using static Analysistem.Utils.FakeUser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using static System.Math;
using static System.Convert;

namespace Analysistem
{
    public static class Synchronizer
    {
        #region WHERE THE MAGIC HAPPENS
        // start/stop recording: Synchronous
        public static EventInfo Record(bool isStart, Unit delayUnits = Unit.Milliseconds)
        {
            Target sparkvueTarget = DetectTarget(isStart ? Template.SparkvueStart : Template.SparkvueStop);

            Stopwatch stopwatch = new Stopwatch();

            // cache original mouse position
            GetCursorPos(out Point originalPos);

            // pass in stopwatch to start after mouse_down but before mouse_up 
            //MoveToAndClick(sparkvueTarget.location, null, stopwatch); // might be more accurate to start stopwatch below the click?
            SetCursorPos(sparkvueTarget.location.X, sparkvueTarget.location.Y);

            stopwatch.Start();
            PressCaptureScreenHotkey(); // begin recording in Kinovea
            Click(sparkvueTarget.location);
            stopwatch.Stop();

            // return to original mouse position
            // SetCursorPos(originalPos.X, originalPos.Y);

            return new EventInfo(new Target[] { sparkvueTarget }, stopwatch.Elapsed.ToUnits(delayUnits));
        }

        // start/stop recording: Asynchronous
        public static EventInfo RecordThreads(bool isStart, Unit delayUnits = Unit.Milliseconds)
        {
            Target sparkvueTarget = DetectTarget(isStart ? Template.SparkvueStart : Template.SparkvueStop);

            Stopwatch stopwatch = new Stopwatch();
            Barrier barrier = new Barrier(2, (b) =>
            {
                if (b.CurrentPhaseNumber == 0) stopwatch.Start();
                else stopwatch.Stop();
            });

#pragma warning disable IDE0039 //? Use local function
            Action recordKinovea = () =>
            {
                barrier.SignalAndWait(); // wait to start stopwatch

                PressCaptureScreenHotkey(); // begin recording in Kinovea

                barrier.SignalAndWait(); // wait to stop stopwatch
            };

            Action recordSparkvue = () =>
            {
                // cache original mouse position
                GetCursorPos(out Point originalPos);

                // pass in barrier to wait to start stopwatch
                MoveToAndClick(sparkvueTarget.location, barrier);

                barrier.SignalAndWait(); // wait to stop stopwatch

                // return to original mouse position
                SetCursorPos(originalPos.X, originalPos.Y);
            };

            Parallel.Invoke(recordKinovea, recordSparkvue);

            barrier.Dispose();

            return new EventInfo(new Target[] { sparkvueTarget }, stopwatch.Elapsed.ToUnits(delayUnits));
        }

        public static CsvFile MakeCoincident(CsvFile kinovea, CsvFile sparkvue)
        {
            // TODO:  synchronize csv
            //return new CsvFile();

            // identify lower and higer time resolution file - we're just assuming it will always be the same
            // create a new csv
            CsvFile upSampledKinovea = CsvFile.Empty;
            // create time, x, and y headers and columns
            upSampledKinovea.headers.AddRange(new List<string>() { "Time (ms)", "X Trajectory", "Y Trajectory" });
            upSampledKinovea.columns.AddRange(new List<List<string>>() { new List<string>(), new List<string>(), new List<string>() });
            // find time delta (ms) of sparkvue data
            /* IF FORCE PLATE IS RECORDING AT 1000HZ, THE RESOLUTION IS 1MS THIS MEANS THAT THEY WOULD ALWAYS BE ON THE SAME TIME SCALE, SO NO ROUNDING ON OUR END */
            double targetResolutionMs = (ToDouble(sparkvue.columns[1][1]) - ToDouble(sparkvue.columns[1][0])) * 1000;
            // go through each cell in kinovea time column
            for (int i = 0; i < kinovea.columns[0].Count; i++)
            {
                kinovea.columns[0][i] = (Round(ToInt32(kinovea.columns[0][i]) / targetResolutionMs) * targetResolutionMs).ToString();
                for (int j = 0; j < kinovea.columns[i].Count; j++) // TODO: Make this do wtf it's suppsoed to 
                {
                    // put all the motion data where it's supposed to go

                }
            }

            // find the flat spot in the force data      - "airtime"
            // find the parabola spot in the motion data - "airtime"
            // make sure their lengths are the same enough based on the new time scale
            // possibly fix the position of the beginning or end

            return upSampledKinovea;
        }
        #endregion
    }
}
