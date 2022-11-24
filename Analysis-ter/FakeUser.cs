using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using Emgu.CV.CvEnum;
using Emgu.CV;
// nuget => emgu.cv.bitmap
// if you're still getting errors, go on a rampage uninstalling and reinstalling in different ways til it works (somehow not really a joke)

namespace Analysistem.Utils
{
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
            location = new Point(screenOrigins[screenId].X + minLoc.X, screenOrigins[screenId].Y + minLoc.Y);
            value = minVal;
            this.threshold = threshold;
            this.screenId = screenId;
            confidence = Math.Round(minVal > threshold ? 0 : ((1 - minVal / threshold) * 100), 3);
            detected = minVal < threshold;
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
    public struct EventInfo
    {
        public Target[] targets; // SparkVue target information
        public double delay; // approx. delay between programmatically starting Kinovea and clicking record in SparkVue
        public string fileName;

        public EventInfo(Target[] targets, double delay, string fileName = "")
        {
            this.targets = targets;
            this.delay = delay;
            this.fileName = fileName;
        }

        public void Deconstruct(out Target[] targets, out double delay)
        {
            targets = this.targets;
            delay = this.delay;
        }
    }
    #endregion

    static class FakeUser
    {
        #region STATIC VARS
        // if minVal from `DetectTarget()` is below this value, the detection is *likely* to be accurate
        // "likely" because these values tend to be inconsistent across computers and especially between different
        //      templates (e.g., SparkVue's THRESHOLD is approx. double that of Kinovea's (from my own testing!))
        public const double Threshold = 9_000_000.0;
        private static readonly Screen[] screens = Screen.AllScreens;
        #endregion

        #region MOUSE STUFF
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        public static void Click(Point location, Barrier barrier = null, Stopwatch stopwatch = null)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)location.X, (uint)location.Y, 0, 0);
            if (stopwatch != null) stopwatch.Start();
            else if (barrier != null) barrier.SignalAndWait(); // maybe the delay would be more accurate with these two lines below LEFTUP ???
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)location.X, (uint)location.Y, 0, 0);
        }

        // convenience function
        public static void MoveToAndClick(Point location, Barrier barrier = null, Stopwatch stopwatch = null)
        {
            SetCursorPos(location.X, location.Y);
            Click(location, barrier, stopwatch);
        }
        #endregion

        #region KEYBOARD STUFF
        private const uint KEYEVENTF_KEYDOWN = 0x00;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x01;
        private const uint KEYEVENTF_KEYUP = 0x02;
        private const uint VK_SHIFT = 0x10;
        private const uint VK_RETURN = 0x0D; 
        public const char CH_RETURN = ';'; // arbitrary char representation; just need a free char that's invalid for file names and doesn't use Shift
        [DllImport("user32.dll")] 
        private static extern short VkKeyScan(char ch);
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [StructLayout(LayoutKind.Explicit)]
        private struct Helper
        {
            [FieldOffset(0)] public short Value;
            [FieldOffset(0)] public byte Low;
            [FieldOffset(1)] public byte High;
        }

        private static void HoldKey(byte virtualKeyCode)
        {
            keybd_event(virtualKeyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0);
        }

        private static void ReleaseKey(byte virtualKeyCode)
        {
            keybd_event(virtualKeyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public static void PressKey(char key)
        {
            Helper helper = new Helper { Value = VkKeyScan(key) };

            byte virtualKeyCode = key == CH_RETURN ? (byte)VK_RETURN : helper.Low;
            byte shiftState = helper.High;
            bool holdingShift = (shiftState & 1) != 0;

            if (holdingShift) HoldKey((byte)VK_SHIFT);

            HoldKey(virtualKeyCode);
            ReleaseKey(virtualKeyCode);

            if (holdingShift) ReleaseKey((byte)VK_SHIFT);

            Console.WriteLine("Don't know if it worked, but at least it didn't crash...");
        }

        // for convenience/explicitness
        public static void PressEnter()
        {
            PressKey(CH_RETURN);
        }
        #endregion

        #region HELPER FUNCTIONS
        // converts Bitmap (RGBA) to Mat (BGR)
        private static Mat ToMatBgr(this Bitmap bitmap)
        {
            Mat matBgr = new Mat();
            CvInvoke.CvtColor(bitmap.ToMat(), matBgr, ColorConversion.Rgba2Bgr);

            return matBgr;
        }
        #endregion

        #region TEMPLATES
        public static Mat LoadMat(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String); // base64 string to u8 bytes

            Bitmap bitmap;
            using (MemoryStream ms = new MemoryStream(bytes)) // stream across the bytes
            {
                bitmap = (Bitmap)Image.FromStream(ms); // convert bytes to Bitmap
            }

            return bitmap.ToMatBgr(); // convert RGBA Bitmap to BGR Mat and return
        }
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

        public static Target DetectTargetOnScreen(Mat template, int screen, double threshold)
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

            return new Target(minLoc, minVal, threshold, screen);
        }

        // detect target on all screens
        public static Target DetectTarget(Mat template, double threshold = Threshold) // using TM_SQDIFF
        {
            Target bestGuess = DetectTargetOnScreen(template, 0, threshold);

            for (int idx = 1; idx < screens.Length; idx++)
            {
                Target currGuess = DetectTargetOnScreen(template, idx, threshold);

                if (currGuess.value < bestGuess.value) bestGuess = currGuess;
            }

            return bestGuess;
        }
        #endregion
    }
}
