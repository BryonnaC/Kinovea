using Analysistem.Utils;
using static Analysistem.Utils.FakeUser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static Analysistem.Math.Math;
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

        public static CsvFile _MakeCoincident(CsvFile kinovea, CsvFile sparkvue)
        {
            const int kinoveaSampleRate = 33;
            const int sparkvueSampleRate = 5;
            const int timeColumnIndex = 1;

            double[] kinoveaTimeVector = CalculateTimeVector(kinovea.headers.Count, kinoveaSampleRate);
            double[] sparkvueTimeVector = CalculateTimeVector(sparkvue.headers.Count, sparkvueSampleRate);

            double[] kinoveaSignal = kinovea.columns[timeColumnIndex].Select(cell => double.Parse(cell)).ToArray();
            double[] sparkvueSignal = sparkvue.columns[timeColumnIndex].Select(cell => double.Parse(cell)).ToArray();

            /** In the words of ChatGTP:
             * It is worth noting that this approach may not always be reliable, as the cross-correlation 
             *  may not always have a unique maximum value. In such cases, it may be necessary to use a 
             *  different method to determine the time offset that aligns the two signals.
             */
            double[] crossCorrelation = CalculateCrossCorrelation(kinoveaSignal, sparkvueSignal);
            int maxCrossCorrelationIndex = Array.IndexOf(crossCorrelation, crossCorrelation.Max());

            double timeOffset = kinoveaTimeVector[maxCrossCorrelationIndex] - sparkvueTimeVector[maxCrossCorrelationIndex];

            List<string> adjustedKinoveaTimeVector = kinoveaTimeVector.Select(time => (time + timeOffset).ToString()).ToList();
            kinovea.columns[timeColumnIndex] = adjustedKinoveaTimeVector;

            return kinovea;
        }

        public static CsvFile MakeCoincident(CsvFile kinovea, CsvFile sparkvue)
        {
            // identify lower and higer time resolution file - we're just assuming it will always be the same
            // create a new csv
            CsvFile upSampledKinovea = CsvFile.Empty;
            // create time, x, and y headers and columns
            upSampledKinovea.headers.AddRange(new List<string>() { "Time (ms)", "Y Trajectory" });
            upSampledKinovea.columns.AddRange(new List<List<string>>() { new List<string>(), new List<string>() });
            // find time delta (ms) of sparkvue data (target resolution)
            /* IF FORCE PLATE IS RECORDING AT 1000HZ, THE RESOLUTION IS 1MS THIS MEANS THAT THEY WOULD ALWAYS BE ON THE SAME TIME SCALE, SO NO ROUNDING ON OUR END */
            double targetResolutionMs = (ToDouble(sparkvue.columns[1][1]) - ToDouble(sparkvue.columns[1][0])) * 1000;
            // go through each cell in kinovea time column
            for (int i = 0; i < kinovea.columns[0].Count; i++)
            {
                // round the time cells to the target resolution and place them into the new csv
                upSampledKinovea.columns[0].Add((Round(ToInt32(kinovea.columns[0][i]) / targetResolutionMs) * targetResolutionMs).ToString());
            }
            for (int i = 1; i < kinovea.columns.Count; i++)
            {
                for (int j = 0; j < kinovea.columns[i].Count; j++)
                {
                    // put all the motion data where it's supposed to go
                    upSampledKinovea.columns[i].Add(kinovea.columns[i][j]);
                }
            }

            // define size of force "look at" window
            int forceWindowLength = 10;
            // define "flat" std dev threshold
            double flatThreshold = 20.0;
            // declare left and right side of force "airtime", and width (range of these idxs)
            int forceAirIdxL = 0, forceAirIdxR = 0;
            int forceAirWidth = 0;

            // define size of motion "look at" window
            int motionWindowLength = 2;
            // define "steep" std dev threshold
            double steepThreshold = 10;
            // declare left and right side of motion "airtime", and width (range of these idxs)
            int motionAirIdxL = 0, motionAirIdxR = 0;
            int motionAirWidth = 0;

            // find the flat spot in the force data      - "airtime"
            // create queue to look at a bit of the data at a time
            Queue<double> forceWindow = new Queue<double>();
            // populate with the first `forceWindowLength` data points
            for (int i = 0; i < forceWindowLength; i++)
            {
                forceWindow.Enqueue(ToDouble(sparkvue.columns[6][i]));
            }
            // loop through the rest of the data
            for (int i = forceWindowLength; i < sparkvue.columns[6].Count; i++)
            {
                List<double> windowList = forceWindow.ToList();
                double stdDev = getStandardDeviation(windowList);
                if (forceAirIdxL == 0 && stdDev < flatThreshold)
                {
                    forceAirIdxL = i - forceWindowLength;
                }
                if (forceAirIdxR == 0 && forceAirIdxL != 0 && stdDev > flatThreshold)
                {
                    forceAirIdxR = i - 3;
                    forceAirWidth = ToInt32((ToDouble(sparkvue.columns[1][forceAirIdxR]) - ToDouble(sparkvue.columns[1][forceAirIdxL])) * 1000);
                    Console.WriteLine("forceAirWidth: " + forceAirWidth);
                    //Console.WriteLine("----------" + sparkvue.columns[1][forceAirIdxL] + " " + sparkvue.columns[1][forceAirIdxR] + " " + stdDev + "----------");
                    break;
                }

                forceWindow.Enqueue(ToDouble(sparkvue.columns[6][i]));
                forceWindow.Dequeue();
            }

            // find the parabola spot in the motion data - "airtime"
            // create queue to look at a bit of the data at a time
            Queue<double> motionWindow = new Queue<double>();
            // populate with the first `windowLength` data points
            for (int i = 0; i < motionWindowLength; i++)
            {
                motionWindow.Enqueue(ToDouble(upSampledKinovea.columns[1][i]));
            }
            // loop through the rest of the data
            for (int i = motionWindowLength; i < upSampledKinovea.columns[1].Count; i++)
            {
                List<double> windowList = motionWindow.ToList();
                double range = Abs(windowList[1] - windowList[0]);
                if (motionAirIdxL == 0 && range > steepThreshold)
                {
                    motionAirIdxL = i - motionWindowLength;
                }
                if (motionAirIdxR == 0 && motionAirIdxL != 0 && range > flatThreshold)
                {
                    motionAirIdxR = i - 4;
                    motionAirWidth = ToInt32(upSampledKinovea.columns[0][motionAirIdxR]) - ToInt32(upSampledKinovea.columns[0][motionAirIdxL]);
                    //Console.WriteLine("----------" + upSampledKinovea.columns[1][motionAirIdxL] + " " + upSampledKinovea.columns[1][motionAirIdxR] + " " + range + "----------");
                    break;
                }
                //Console.Write(range);
                //Console.WriteLine(" " + upSampledKinovea.columns[0][i - motionWindowLength]);

                motionWindow.Enqueue(ToDouble(upSampledKinovea.columns[1][i]));
                motionWindow.Dequeue();
            }

            // make sure their lengths are the same enough based on the new time scale
            if (forceAirWidth == motionAirWidth) return upSampledKinovea;
            // possibly fix the position of the beginning or end
            //if (motionAirWidth < forceAirWidth)
            //{
            while (motionAirWidth < forceAirWidth)
            {
                upSampledKinovea.columns[0][motionAirIdxR] = (ToInt32(upSampledKinovea.columns[0][motionAirIdxR]) + targetResolutionMs).ToString();
                motionAirWidth = ToInt32(upSampledKinovea.columns[0][motionAirIdxR]) - ToInt32(upSampledKinovea.columns[0][motionAirIdxL]);
            }
            //}
            //if (motionAirWidth > forceAirWidth)
            //{
            while (motionAirWidth > forceAirWidth)
            {
                upSampledKinovea.columns[0][motionAirIdxR] = (ToInt32(upSampledKinovea.columns[0][motionAirIdxR]) - targetResolutionMs).ToString(); ;
                motionAirWidth = ToInt32(upSampledKinovea.columns[0][motionAirIdxR]) - ToInt32(upSampledKinovea.columns[0][motionAirIdxL]);
            }
            //}
            Console.WriteLine("motion width: " + motionAirWidth);

            return upSampledKinovea;
        }

        private static double getStandardDeviation(List<double> doubleList) // TODO: PUT THIS SOMEWHERE REASONABLE?
        {
            double average = doubleList.Average();
            double sumOfDerivation = 0;
            foreach (double value in doubleList)
            {
                sumOfDerivation += (value) * (value);
            }
            double sumOfDerivationAverage = sumOfDerivation / (doubleList.Count - 1);
            return Sqrt(sumOfDerivationAverage - (average * average));
        }
        #endregion
    }
}
