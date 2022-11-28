using Analysistem.Utils;
using static Analysistem.Utils.FakeUser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Analysistem
{
    public static class Synchronizer
    {
        //public delegate void MyFunction();

        public static void Test(Controller.MyFunction fu)
        {
            // putit here bryoppknasdfia
            PressCaptureScreenHotkey();
        }

        #region WHERE THE MAGIC HAPPENS
        // start/stop recording: Synchronous
        //public static EventInfo Record(bool isStart, Controller.MyFunction kinoveaRecord, Unit delayUnits = Unit.Milliseconds)
        //{
        //    Target sparkvueTarget = DetectTarget(isStart ? Template.SparkvueStart : Template.SparkvueStop);

        //    Stopwatch stopwatch = new Stopwatch();

        //    // cache original mouse position
        //    GetCursorPos(out Point originalPos);

        //    // pass in stopwatch to start after mouse_down but before mouse_up 
        //    MoveToAndClick(sparkvueTarget.location, null, stopwatch); // might be more accurate to start stopwatch below the click?

        //    // isStart to determine start/stop
        //    //==============================================================================================================================================================================================================================================
        //    //============================================================================== TODO: start/stop recording kinovea here =======================================================================================================================
        //    //==============================================================================================================================================================================================================================================
        //    kinoveaRecord();

        //    stopwatch.Stop();

        //    // return to original mouse position
        //    SetCursorPos(originalPos.X, originalPos.Y);

        //    return new EventInfo(new Target[] { sparkvueTarget }, stopwatch.Elapsed.ToUnits(delayUnits));
        //}

        //// start/stop recording: Asynchronous
        //public static EventInfo RecordThreads(bool isStart, Controller.MyFunction kinoveaRecord, Unit delayUnits = Unit.Milliseconds)
        //{
        //    Target sparkvueTarget = DetectTarget(isStart ? Template.SparkvueStart : Template.SparkvueStop);

        //    Stopwatch stopwatch = new Stopwatch();
        //    Barrier barrier = new Barrier(2, (b) =>
        //    {
        //        if (b.CurrentPhaseNumber == 0) stopwatch.Start();
        //        else stopwatch.Stop();
        //    });

        //    #pragma warning disable IDE0039 //? Use local function
        //    Action recordKinovea = () =>
        //    {
        //        barrier.SignalAndWait(); // wait to start stopwatch

        //        // isStart to determine start/stop
        //        //==========================================================================================================================================================================================================================================
        //        //========================================================================== TODO: start/stop recording kinovea here =======================================================================================================================
        //        //==========================================================================================================================================================================================================================================
        //        kinoveaRecord();

        //        barrier.SignalAndWait(); // wait to stop stopwatch
        //    };

        //    Action recordSparkvue = () =>
        //    {
        //        // cache original mouse position
        //        GetCursorPos(out Point originalPos);

        //        // pass in barrier to wait to start stopwatch
        //        MoveToAndClick(sparkvueTarget.location, barrier);

        //        barrier.SignalAndWait(); // wait to stop stopwatch

        //        // return to original mouse position
        //        SetCursorPos(originalPos.X, originalPos.Y);
        //    };

        //    Parallel.Invoke(recordKinovea, recordSparkvue);

        //    barrier.Dispose();

        //    return new EventInfo(new Target[] { sparkvueTarget }, stopwatch.Elapsed.ToUnits(delayUnits));
        //}

        //public static CsvFile MakeCoincident(CsvFile kinovea, CsvFile sparkvue)
        //{
        //    // TODO:  synchronize csv
        //    return new CsvFile();
        //}
        #endregion
    }
}
