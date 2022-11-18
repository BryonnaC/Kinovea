using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
// nuget => emgu.cv.bitmap
// if you're still getting errors, go on a rampage uninstalling and reinstalling in different ways til it works (somehow not really a joke)

public enum Units:int // delay units
{
    Milliseconds = 1,
    Microseconds = 1_000,
    Nanoseconds = 1_000_000,
}

// structs for returning information
// these are intended to be expanded if we want to return more stuff

#region TARGET
public struct Target // using TM_SQDIFF
{
    public Point location; // minLoc, adjusted per screen as minLoc has a default origin of (0,0)
    public double value; // minVal for client debugging
    public bool detected; // minVal < THRESHOLD
    public double confidence; // (1 - minVal/THRESHOLD) * 100 for perc representation of detection confidence cuz why not
    public double threshold; // THRESHOLD for this Target
    public int screenId; // screen id this Target was found on

    public Target(Point minLoc, double minVal, double threshold, int screenId)
    {
        Point[] screenOrigins = (from screen in (IEnumerable<Screen>)Screen.AllScreens select new Point(screen.Bounds.X, screen.Bounds.Y)).ToArray();
        this.location = new Point(screenOrigins[screenId].X + minLoc.X, screenOrigins[screenId].Y + minLoc.Y);
        this.value = minVal;
        this.threshold = threshold;
        this.screenId = screenId;
        this.confidence = Math.Round(minVal > threshold ? 0 : ((1 - minVal / threshold) * 100), 3);
        this.detected = minVal < threshold;
    }

    // ui props sorta deconstruct
    public void Deconstruct(out Point location, out double value,
                            out bool detected, out double confidence)
    {
        location = this.location;
        value = this.value;
        detected = this.detected;
        confidence = this.confidence;
    }
    // debug deconstruct
    public void Deconstruct(out double threshold, out int screenId)
    {
        threshold = this.threshold;
        screenId = this.screenId;
    }
}
#endregion

#region DATA
public struct Data
{
    public Target target; // SparkVue target information
    public double delay; // approx. delay between programmatically starting Kinovea and clicking record in SparkVue

    public Data(Target target, double delay)
    {
        this.target = target;
        this.delay = delay;
    }

    public void Deconstruct(out Target sparkvueInfo, out double delay)
    {
        sparkvueInfo = target;
        delay = this.delay;
    }
}
#endregion

public static class Synchronizer
{
    #region HELPER FUNCTIONS
    private static double ToUnits(this TimeSpan elapsed, Units unit)
    {
        return elapsed.Ticks / (double)TimeSpan.TicksPerMillisecond * (int)unit;
    }

    // converts Bitmap (RGBA) to Mat (BGR)
    private static Mat ToMatBgr(this Bitmap bitmap)
    {
        Mat matBgr = new Mat();
        CvInvoke.CvtColor(bitmap.ToMat(), matBgr, ColorConversion.Rgba2Bgr);

        return matBgr;
    }
    #endregion

    private static class SynchronizeRecording
    {
        #region STATIC VARS
        // if minVal from `DetectTarget()` is below this value, the detection is *likely* to be accurate
        // "likely" because these values tend to be inconsistent across computers and especially between different
        //      templates (e.g., SparkVue's THRESHOLD is approx. double that of Kinovea's (from my own testing!))
        private const double THRESHOLD = 9_000_000.0;
        private static readonly Screen[] screens = Screen.AllScreens;
        #endregion

        #region MOUSE STUFF
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x02;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x04;
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);
        
        private static void Click(Point location, Barrier barrier = null, Stopwatch stopwatch = null)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)location.X, (uint)location.Y, 0, 0);
            if (stopwatch != null) stopwatch.Start();
            else if (barrier != null) barrier.SignalAndWait(); // maybe the delay would be more accurate with these two lines below LEFTUP ???
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)location.X, (uint)location.Y, 0, 0);
        }

        // convenience function
        private static void MoveToAndClick(Point location, Barrier barrier = null, Stopwatch stopwatch = null)
        {
            SetCursorPos(location.X, location.Y);
            Click(location, barrier, stopwatch);
        }
        #endregion

        #region TEMPLATES
        private static Mat LoadMat(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String); // base64 string to u8 bytes

            Bitmap bitmap;
            using (MemoryStream ms = new MemoryStream(bytes)) // stream across the bytes
            {
                bitmap = (Bitmap)Bitmap.FromStream(ms); // convert bytes to Bitmap
            }

            return bitmap.ToMatBgr(); // convert RGBA Bitmap to BGR Mat and return
        }

        private static readonly Dictionary<bool, Mat> encodedTemplates = new Dictionary<bool, Mat>()
        {
            { true, LoadMat("iVBORw0KGgoAAAANSUhEUgAAAEwAAAAlCAYAAADobA+5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAP5SURBVGhD7ZlNT1NBFIbv36BUJXUDCTFYY0wAsQkbmjRu2ahsEERbqlAEAlRQoAViMNSaiHyoC03AKImLxujKRKKJ+pfGeYc717nTU3qh7YVJunh6pzNzznvO2+kHF6th5A2r4x2r4QEfeKRxZJM1T2ZZeHaMdSzcZt3Lvazn6XUWW42KK55jHuvYh/1UHpPxZNi5sTxrS0+wa9lb3Jwez2A/4hBP5TWRsoa1zqRZJHuTNMQriEceKr9pWAH+QNGUesEuzyV5w9EiA45HVORDXkrPFKzAfT7QCI2tsfb5QaLpykFe5Kd0TYAb9poP/tOUytfMLAnyQ0fXNgErkOQDhYO3Id1oNYGOrm0CLsNap9K8mWp9ZpUjKvRUfRNwDGsazVf8bXhUoAddtaDTjhUY5gNO28wE2VStga6swQS4YdssmHzFIhl/T5cEutBHHSYgDGsez5DN+AX09cLAyqd99vbrX4etL39YX75A7vULYVg4nSIb8Qvo64XBrPXCbxZb3itak6Tff6+6iYnNb+LFwZVatwKJbdYx30824hfQRx2S2NKeMAuGqPM6LsOI9eOQ2LAN41dqXRjWvdRLNuIX0FeLggEwYuXjvmteBWvk25WvScPlWu7zL3Z1bteJlevIIQ3CnsyHH06MRDfOCsS37Vs0dDN+AH3UoSINcZrV1kH6nXLC7DmYgRhc8Vyaj73qHhgGYJKcB64TpsxLhGH+/VgtRZQsTj1FasMSyjAdmA0DxWm156Rh1ItR1rBG/hA94RMGfdRBIZuTp61rdtdZcwx7XnDFqGA/4tRYmRMm6vtVw/Q1IAw7DZ9hemE6MAeNqE2WMgzPMY/9kuoZdm+LdT452W9J6KOOw+ia3VFOyo6Ycxlm75MNCzNKxMWyimF2nMRlmLYGhGHhmVGyEb+Avl6YjlfDYALMgCml4io2rCW1SDbiF9BXi+rLFdizvZ+s69FBg4BqhJpzTOQ51D3CMDvfYYYhDvHIo68BYdiZ+DqLLJ7Q35JcF/p6YbJwNAtUE1TQtLoOU2COjMO6NE2evMMMAzBLxqsvBrAa7/IB5+LUONlQrYGurMEErCB/AKFkjkUyN8imagX0oCtrMAErOMQHNhcmpnkj/t1xhZ6qbwIuw8CVdIJorvpAR9c2gSLDQokca58bIJusFsgPHV3bBKzgHT7QOD+8WjPTkBf5KV0T4IZt8kExoXjOfntW7z/fyIe8lJ4plDRMcmF8mv9WquzbE/HIQ+U3jbKGgVB8jYUnHx75xy32Iw7xVF4TsYKDfOCRs0MvWcvIArs0Nco6H/eLuwwHt4ai4ornmMc69mE/lcdkuGEbfFDHK1ZwgA/qeKZu2JHYYP8AO4gg4ZYduUsAAAAASUVORK5CYII=") },
            { false, LoadMat("iVBORw0KGgoAAAANSUhEUgAAAEgAAAAgCAIAAAA+KKknAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAARHSURBVFhH3Vd9bBRFFN/ZlmuhvaOf0IS7Hldp0RD6HT0qBCxtTMSIPUwFNTGEfzTRGE+TiqQpgcSkJhwGDIEA0X+IQKClYDSmVDSxVfpFC5Kq5evahkh7LUpbe3cte/5mZ27duxJQurs1/PLLZd6bN7Pz5r03M0dsNScEQSAhKSk4YQkEiBCCOHNIRBycm+CPNXHZcBDb9joiSc6hvi09rYlTAa7WAjfnWnYUl/8RN4/LxgKO1VuCE7tazuQPD3CddvDkrfliSSEXjIUI30xSaP6kXxSI5jRPBjH/rBCOoapCIYGQUEhzalSwDwMaMRIicJLoAfqJiI00jCLbVOqXWq0R6Q/mnw3SVHwkITsGF3WCfjM/CCJyRatLeTrkhKTzG0/UGG2oC0Nbqr7FWbI4/bz7We/29YyXt65z5dqibGZOo2usxJG220Wv7E2ft9hrToPLPvqqrpu+DVx51ssfPnf2rWdkw5lCPhUZ9YAyeZjOxWkpCabvegdbrvmiuijvNeThGCtPZhwWmuNNMaI1OfoB6S59/M1VS9CVvcDs3fFC7+Bo2afnoKcR3lCYYYlnZkc7+qoaulibDWn1jgzc/mtjUSY0Y4Gp6i8vsvgjYvLDQy/Ik6vYcLH/9zv+lVlpNOVUek9TT1V9F1YGl+zVDWV7v4XSlWs9+MqTmGXTZ81QHu3wwoEjr6/go2hohGUZliJbMnphg+E7n8/FKPQaXWPIwHdPdsA3Gpmd62tfzOcd98KGApspVjzW6aV5KwhVp7rgdr412ZVnYwaAbyxAd0GeGZawf8qRClEk3HN9gDfoNP541ef8+Js9534N3pU2FtnPvl3K9AoUS+TtyHjw/DWfounsH0mMi3U6UpkIDI76ld4bvvHglGRLmof2rL08PE2/5NScQQQQuiObS7hWhZKsNHP8HC78d8g1pmPIMPc/hTSd+77/DYWxwBzPNaohLVeHRv2TihgmM5CVCsK9jtQEpCLTzFrE/g2QZrgbnFnpXBaEwswUbMRP12nJTQdSF78dfSP4Dd9jOoFNrmJtRUHjO2sV8aXCTOzx1z/fRPvWnxNYNNIPRzzrPdHZh5qpLLIzDcYib7sHbtdf6GcGwNOPpUOPdkW+bd3yRd7hcU9jD0Ri31qf6h/b13oq586QvBYtsT97xYGlTi6EUesqeLnYztrwpPp0N12oDHfZE2+szsZthtor/6QJGlSap7JYuceOtXur6i6wNjNuuzGMTIbD0OCwdR9vZ0cosX8gO9amm2M50Y5pBXe57Nj14VcPN3OVCux1D8hx1RoRz1Ktqaw5Ss9o9FtRYzJEKWX+r0/F+2N3Y8/SbQ2vHfqBy5HQ+VQEVLtoJI0+7g0jUhH/n6UY9UWuHURBiviagaQRG42J602kL2LNcWl+RuTnjCNxvH8SK0icDJTeurLQP8YWpAkuJWW0p1inxBguGwvieI869qhBEP4GgyulNRPb7m0AAAAASUVORK5CYII=") }
        };
        #endregion

        #region TARGET DETECTION
        private static Mat CaptureScreen(int index)
        {
            Rectangle bounds = screens[index].Bounds;

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) // new Bitmap
            {
                using (Graphics g = Graphics.FromImage(bitmap)) // take the screenshot
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }

                return bitmap.ToMatBgr(); // convert RGBA Bitmap to BGR Mat and return
            }
        }

        private static Target DetectTargetOnScreen(Mat template, int screen)
        {
            Mat screenshot = CaptureScreen(screen);
            Mat result = new Mat(); // ref to hold MatchTemplate() result
            CvInvoke.MatchTemplate(screenshot, template, result, TemplateMatchingType.Sqdiff); // run dumb algo to find best match of template in screenshot

            // exempt from naming conventions (every person in every language will name these as they are)
            double minVal = 0; // essentially, 'arbitrary' confidence value for detection accuracy
            double maxVal = 0; // used for other TemplateMatchingTypes
            Point minLoc = new Point(); // 'best guess' top-left corner location of target
            Point maxLoc = new Point(); // used for other TemplateMatchingTypes
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

            return new Target(minLoc, minVal, THRESHOLD, screen);
        }

        // detect target on all screens
        private static Target DetectTarget(Mat template) // using TM_SQDIFF
        {
            Target bestGuess = DetectTargetOnScreen(template, 0);

            for (int idx = 1; idx < screens.Length; idx++)
            {
                Target currGuess = DetectTargetOnScreen(template, idx);

                if (currGuess.value < bestGuess.value) bestGuess = currGuess;
            }

            return bestGuess;
        }
        #endregion

        #region WHERE THE MAGIC HAPPENS
        // start/stop recording: Synchronous
        public static Data Record(bool isStart, Units delayUnits)
        {
            Target sparkvueTarget = DetectTarget(encodedTemplates[isStart]);

            Stopwatch stopwatch = new Stopwatch();

            // cache original mouse position
            GetCursorPos(out Point originalPos);

            // pass in stopwatch to start after mouse_down but before mouse_up 
            MoveToAndClick(sparkvueTarget.location, null, stopwatch); // might be more accurate to start stopwatch below the click?

            // isStart to determine start/stop
            //==============================================================================================================================================================================================================================================
            //============================================================================== TODO: start/stop recording kinovea here =======================================================================================================================
            //==============================================================================================================================================================================================================================================
            //CaptureScreen captureScreen = new CaptureScreen();
            //captureScreen.TriggerCapture();                     //this capture screen will likely get changed as GUI is put together

            stopwatch.Stop();

            // return to original mouse position
            SetCursorPos(originalPos.X, originalPos.Y);

            return new Data(sparkvueTarget, ToUnits(stopwatch.Elapsed, delayUnits));
        }

        // start/stop recording: Asynchronous
        public static Data RecordThreads(bool isStart, Units delayUnits)
        {
            Target sparkvueTarget = DetectTarget(encodedTemplates[isStart]);

            Stopwatch stopwatch = new Stopwatch();
            Barrier barrier = new Barrier(2, (b) =>
            {
                if (b.CurrentPhaseNumber == 0) stopwatch.Start();
                else stopwatch.Stop();
            });

            #pragma warning disable IDE0039 //? Use local function
            Action startKinovea = () =>
            {
                barrier.SignalAndWait(); // wait to start stopwatch

                // isStart to determine start/stop
                //==========================================================================================================================================================================================================================================
                //========================================================================== TODO: start/stop recording kinovea here =======================================================================================================================
                //==========================================================================================================================================================================================================================================
                //CaptureScreen captureScreen = new CaptureScreen();  //this will change as GUI is built
                //captureScreen.TriggerCapture();

                barrier.SignalAndWait(); // wait to stop stopwatch
            };

            Action startSparkvue = () =>
            {
                // cache original mouse position
                GetCursorPos(out Point originalPos);

                // pass in barrier to wait to start stopwatch
                MoveToAndClick(sparkvueTarget.location, barrier); 

                barrier.SignalAndWait(); // wait to stop stopwatch

                // return to original mouse position
                SetCursorPos(originalPos.X, originalPos.Y);
            };

            Parallel.Invoke(startKinovea, startSparkvue);

            barrier.Dispose();

            return new Data(sparkvueTarget, ToUnits(stopwatch.Elapsed, delayUnits));
        }
        #endregion
    }

    private static class SynchronizeCsv
    {

        private static void combineCSV(string fileNameOne, string fileNameTwo, string fileNameDest)
        {
            var xPositions = File.ReadAllLines(fileNameOne);
            var yPositions = File.ReadAllLines(fileNameTwo);
            var result = xPositions.Zip(yPositions, (f, s) => string.Join(",", f, s));
            File.WriteAllLines(fileNameDest, result);
        }
    }

    #region FACADE XD
    public static Data Record(bool isStart, Units delayUnits = Units.Microseconds)
    {
        //this is throwing an exception with CV and Emgu
        return SynchronizeRecording.Record(isStart, delayUnits);
    }
    public static Data RecordThreads(bool isStart, Units delayUnits = Units.Microseconds)
    {
        return SynchronizeRecording.RecordThreads(isStart, delayUnits);
    }
    #endregion
}
