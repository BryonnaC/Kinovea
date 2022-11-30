using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
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
        public bool detected; // minVal < threshold
        public double confidence; // (1 - minVal/threshold) * 100 for perc representation of detection confidence cuz why not
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

    #region EVENT_INFO
    public struct EventInfo
    {
        public Target?[] targets; // SparkVue target information
        public double delay; // approx. delay between programmatically starting Kinovea and clicking record in SparkVue
        public string fileName;

        public EventInfo(Target?[] targets, double delay, string fileName = "")
        {
            this.targets = targets;
            this.delay = delay;
            this.fileName = fileName;
        }

        // non-nullable constructor
        public EventInfo(Target[] targets, double delay, string fileName = "")
        {
            this.targets = (from target in targets select (Target?)target).ToArray();
            this.delay = delay;
            this.fileName = fileName;
        }

        public void Deconstruct(out Target?[] targets, out double delay)
        {
            targets = this.targets;
            delay = this.delay;
        }
    }
    #endregion

    public static class FakeUser
    {
        #region STATIC VARS
        private static readonly Screen[] screens = Screen.AllScreens;
        public static readonly TemplateDictionary encodedTemplates = new TemplateDictionary()
            {
                { Template.SparkvueStart, new TemplateData("iVBORw0KGgoAAAANSUhEUgAAAEwAAAAlCAYAAADobA+5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAP5SURBVGhD7ZlNT1NBFIbv36BUJXUDCTFYY0wAsQkbmjRu2ahsEERbqlAEAlRQoAViMNSaiHyoC03AKImLxujKRKKJ+pfGeYc717nTU3qh7YVJunh6pzNzznvO2+kHF6th5A2r4x2r4QEfeKRxZJM1T2ZZeHaMdSzcZt3Lvazn6XUWW42KK55jHuvYh/1UHpPxZNi5sTxrS0+wa9lb3Jwez2A/4hBP5TWRsoa1zqRZJHuTNMQriEceKr9pWAH+QNGUesEuzyV5w9EiA45HVORDXkrPFKzAfT7QCI2tsfb5QaLpykFe5Kd0TYAb9poP/tOUytfMLAnyQ0fXNgErkOQDhYO3Id1oNYGOrm0CLsNap9K8mWp9ZpUjKvRUfRNwDGsazVf8bXhUoAddtaDTjhUY5gNO28wE2VStga6swQS4YdssmHzFIhl/T5cEutBHHSYgDGsez5DN+AX09cLAyqd99vbrX4etL39YX75A7vULYVg4nSIb8Qvo64XBrPXCbxZb3itak6Tff6+6iYnNb+LFwZVatwKJbdYx30824hfQRx2S2NKeMAuGqPM6LsOI9eOQ2LAN41dqXRjWvdRLNuIX0FeLggEwYuXjvmteBWvk25WvScPlWu7zL3Z1bteJlevIIQ3CnsyHH06MRDfOCsS37Vs0dDN+AH3UoSINcZrV1kH6nXLC7DmYgRhc8Vyaj73qHhgGYJKcB64TpsxLhGH+/VgtRZQsTj1FasMSyjAdmA0DxWm156Rh1ItR1rBG/hA94RMGfdRBIZuTp61rdtdZcwx7XnDFqGA/4tRYmRMm6vtVw/Q1IAw7DZ9hemE6MAeNqE2WMgzPMY/9kuoZdm+LdT452W9J6KOOw+ia3VFOyo6Ycxlm75MNCzNKxMWyimF2nMRlmLYGhGHhmVGyEb+Avl6YjlfDYALMgCml4io2rCW1SDbiF9BXi+rLFdizvZ+s69FBg4BqhJpzTOQ51D3CMDvfYYYhDvHIo68BYdiZ+DqLLJ7Q35JcF/p6YbJwNAtUE1TQtLoOU2COjMO6NE2evMMMAzBLxqsvBrAa7/IB5+LUONlQrYGurMEErCB/AKFkjkUyN8imagX0oCtrMAErOMQHNhcmpnkj/t1xhZ6qbwIuw8CVdIJorvpAR9c2gSLDQokca58bIJusFsgPHV3bBKzgHT7QOD+8WjPTkBf5KV0T4IZt8kExoXjOfntW7z/fyIe8lJ4plDRMcmF8mv9WquzbE/HIQ+U3jbKGgVB8jYUnHx75xy32Iw7xVF4TsYKDfOCRs0MvWcvIArs0Nco6H/eLuwwHt4ai4ornmMc69mE/lcdkuGEbfFDHK1ZwgA/qeKZu2JHYYP8AO4gg4ZYduUsAAAAASUVORK5CYII=") },
                { Template.SparkvueStop, new TemplateData("iVBORw0KGgoAAAANSUhEUgAAAEgAAAAgCAIAAAA+KKknAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAARHSURBVFhH3Vd9bBRFFN/ZlmuhvaOf0IS7Hldp0RD6HT0qBCxtTMSIPUwFNTGEfzTRGE+TiqQpgcSkJhwGDIEA0X+IQKClYDSmVDSxVfpFC5Kq5evahkh7LUpbe3cte/5mZ27duxJQurs1/PLLZd6bN7Pz5r03M0dsNScEQSAhKSk4YQkEiBCCOHNIRBycm+CPNXHZcBDb9joiSc6hvi09rYlTAa7WAjfnWnYUl/8RN4/LxgKO1VuCE7tazuQPD3CddvDkrfliSSEXjIUI30xSaP6kXxSI5jRPBjH/rBCOoapCIYGQUEhzalSwDwMaMRIicJLoAfqJiI00jCLbVOqXWq0R6Q/mnw3SVHwkITsGF3WCfjM/CCJyRatLeTrkhKTzG0/UGG2oC0Nbqr7FWbI4/bz7We/29YyXt65z5dqibGZOo2usxJG220Wv7E2ft9hrToPLPvqqrpu+DVx51ssfPnf2rWdkw5lCPhUZ9YAyeZjOxWkpCabvegdbrvmiuijvNeThGCtPZhwWmuNNMaI1OfoB6S59/M1VS9CVvcDs3fFC7+Bo2afnoKcR3lCYYYlnZkc7+qoaulibDWn1jgzc/mtjUSY0Y4Gp6i8vsvgjYvLDQy/Ik6vYcLH/9zv+lVlpNOVUek9TT1V9F1YGl+zVDWV7v4XSlWs9+MqTmGXTZ81QHu3wwoEjr6/go2hohGUZliJbMnphg+E7n8/FKPQaXWPIwHdPdsA3Gpmd62tfzOcd98KGApspVjzW6aV5KwhVp7rgdr412ZVnYwaAbyxAd0GeGZawf8qRClEk3HN9gDfoNP541ef8+Js9534N3pU2FtnPvl3K9AoUS+TtyHjw/DWfounsH0mMi3U6UpkIDI76ld4bvvHglGRLmof2rL08PE2/5NScQQQQuiObS7hWhZKsNHP8HC78d8g1pmPIMPc/hTSd+77/DYWxwBzPNaohLVeHRv2TihgmM5CVCsK9jtQEpCLTzFrE/g2QZrgbnFnpXBaEwswUbMRP12nJTQdSF78dfSP4Dd9jOoFNrmJtRUHjO2sV8aXCTOzx1z/fRPvWnxNYNNIPRzzrPdHZh5qpLLIzDcYib7sHbtdf6GcGwNOPpUOPdkW+bd3yRd7hcU9jD0Ri31qf6h/b13oq586QvBYtsT97xYGlTi6EUesqeLnYztrwpPp0N12oDHfZE2+szsZthtor/6QJGlSap7JYuceOtXur6i6wNjNuuzGMTIbD0OCwdR9vZ0cosX8gO9amm2M50Y5pBXe57Nj14VcPN3OVCux1D8hx1RoRz1Ktqaw5Ss9o9FtRYzJEKWX+r0/F+2N3Y8/SbQ2vHfqBy5HQ+VQEVLtoJI0+7g0jUhH/n6UY9UWuHURBiviagaQRG42J602kL2LNcWl+RuTnjCNxvH8SK0icDJTeurLQP8YWpAkuJWW0p1inxBguGwvieI869qhBEP4GgyulNRPb7m0AAAAASUVORK5CYII=") },
                { Template.HamburgerButton, new TemplateData("iVBORw0KGgoAAAANSUhEUgAAAJIAAAAeCAYAAADHCUctAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAScSURBVHhe7Zo9SFtRFMf/7RwQDUpSwUXEJUMVWozURerQFl3UTQcX4S1OgtCluAgVJ5dAFwe7tV0qrUPFRYmiUBeXoC5Cm2BJSsG9ved+vHffR/Jic0loc38Q8vJyfe++c//n3HNOvHd5XfwNi6VB7st3i6UhrJAsRrBCshjBCsliBCskixGskCxGiC7/K3msrBVwID+GSeHNxjNk5Ke2IdYugOMsYL5ffmgjYoT0/wrm/P0WFo/vuPA17VLGl82PeHVNx43a7QLby4fI/UP2t1ubMZKYWFrAznSCHZewuJln0mofGoxIynOAsekpvM4mxWnF1S5GcyVg5AnyMwPypN/b0kfvMPnhVn5X5TqScmAsER1RqtyjbxA7S934LOfspw7vr8suXmQKz82zlwuf0yiToUTZLIjPhnexRXNoMCINYH7jCRx2dPBhH18q4qyAGY0MQobSDKBTZMZYxzjyGwv89WaErvMRK0dBX6bF2eKGI2Op8eT9udwWRt9fyHFhfPfgC0ZzFvcivOuZ2kKSGH5EUQnInenzIhFdYkjOXc0f1wVM6vPvf8a+EzYV4pbjAyLS7abG53LvAmvQPAxsbWxhnBR7v8WrtV2ci5MsByHPS2B1TvM2HyXsMWPo0Sczo0R54l6HKB/tcw+naKV7XDI7KwRxfIjtK3HOT/gezSDZI4SE40vtOUjAfrEmsw+FYHzj4qHn9j/TAJ7zLfUWe4XWbKgxQmJ7/TLzeN/LE4sL8yI3NyDvYuGZEtmx6XFMdIkhYVKYDy3wAIZ4pCjhzBVGGV9PKYQn8HQwLIjMEImYeeOnqJwk6h5NoLMDY/KwXYgRkhZa3Vf0FpDMjmO1jx2w6CD2+L9bxHSv8ObzGyWLCkq8EkogHSVKtWjXv1DkJyytwGDVxqqWFyI6EI5jKOeo/Kgd9ru6DeU2Bvn5S/Sa+jqQ5icUItfzontU0l8nlJRrO0Uw8W42BoUkk2tJ9FYTT/GbMEimR0azOKHECa0FlG/koj7o9ldjy1TNsbzxpYruKqm+C1KMqhqWO4VILVqHISHRw8nkmhmJJ8DBaqQuyih+p/cEUp38BKMLKdoyWSJZjKpIqnp/q/ByutVxVWkx+3wiJyP7zNbIG+vg6kQ0PQPtgFZjREiqqnIcYaTMzJSbL0VXU1WoFLBHRurrxbBrbFVOR1ck52ciCjovqlWHtfFyMTN4FaZeaMTkeVUJO4+KdmO9jajRPI0LqZLHOu3PzEO80pzlS3ODPAmu3tugilD/jnntW2r2hVsGKpGnHpMuTOqnUHXov3d9qGrv4LTA7mwCseXwXIXNJ1ieq2p02+2RqShO6FUqocaHnSc52Mvt6muRsG1T5UjB5yEb8Twq2Gmn5irPr8LrQz8f8b+5w47ylz/ayi5qZ+1Or9d91b/Xu86PUXR/nyKir6Oov5ur36NG0u/rIseMJWLtUusaJBz9WeU2B++awa6++j2Qo29lvnkz5HfeeG8ers2CHXT3WcLbrXsd/Z4xtOCf/+tcZMs/hcGqzdLOWCFZjGCFZDFCC3Iky/+IjUgWI1ghWYxghWQxghWSxQDAH9NBcSUbvOwfAAAAAElFTkSuQmCC") },
                { Template.ExportData, new TemplateData("iVBORw0KGgoAAAANSUhEUgAAACsAAAArCAYAAADhXXHAAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAGPSURBVFhH7ZjLSgNBEEWrOwmZiI+V4CaPhYroJ4ggguCvxLWP+AiCC38kbtWIor8g4kL0A4IbdyKCaBLzsDrUwnEqs0t3V8iBy/Sd1Z1LMVOJyp7cdLvQAZ9RoEHrJGjfgxpMxnaniZG7PSdCGi9ikNYsc9dTyWpW4VWKRDWrssdX5hhhc3kWSmsL5OxReXiBg9tncmGwWczaT05gcpCG5D3rEi4PSlSzKnd0aXJHmB5Pw8xEQM4eb19NeP34Jhemb1gfkbYbyEHlyvwYpJMagmSCnD0a7Q7Uf9rkwmDYKhu2uDIHpfVFcvao3Nfg8PqJXJj4mXUFlwUVu3W5gsti5N8XjMtCEjUGKr9/YY4RJoMUTGVS5Ozx2WjBO37FOFR+jw/rI6Of4oOSypdiZnbMwczWY2a2sHvOhi2uzsPOxhI5e5ze1aBcfSQXZohm1gVcDpK0ZjEyp97juOBfjj9She0zPEUx+2w6ZX+fbbZi9tnCFh/WR0Z/zA1K+DZg7noqac3KQVizWkC5CYyZCeAXnK6h/6KX5CIAAAAASUVORK5CYII=") },
            };
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
        private const uint VK_CTRL = 0x11;
        private const uint VK_RETURN = 0x0D; 
        public const char CH_RETURN = ';'; // arbitrary char representation; just need a free char that's invalid for file names and doesn't use Shift
        [DllImport("user32.dll")] 
        private static extern short VkKeyScan(char ch);
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [StructLayout(LayoutKind.Explicit)]
        private struct VK_Helper
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
            VK_Helper helper = new VK_Helper { Value = VkKeyScan(key) };

            byte virtualKeyCode = key == CH_RETURN ? (byte)VK_RETURN : helper.Low;
            byte shiftState = helper.High;
            bool holdingShift = (shiftState & 1) != 0;

            if (holdingShift) HoldKey((byte)VK_SHIFT);

            HoldKey(virtualKeyCode);
            ReleaseKey(virtualKeyCode);

            if (holdingShift) ReleaseKey((byte)VK_SHIFT);
        }

        // for convenience/explicitness
        public static void PressEnter()
        {
            PressKey(CH_RETURN);
        }

        public static void PressCaptureScreenHotkey()
        {
            HoldKey((byte)VK_CTRL);
            PressEnter();
            ReleaseKey((byte)VK_CTRL);
        }
        #endregion

        public static void CalibrateTemplate(Template template)
        {
            // this should need to be run ONCE PER COMPUTER.
            // basically, if values aren't found wherever persistent storage is dealt with, run through this.
            // there should also be a way for user to call this function again manually (some button somewhere).

            // Todo: save to persistent storage -> { [templId]: { detected: [minValDet], notDetected: [minValNotDet] } }
            // Prompt user:
            //      Is [template] on screen?
            //          - DetectTarget([template]) -> store { detected: minVal }
            //      Is [template] not on screen?
            //          - DetectTarget([template]) -> store { notDetected: minVal }
        }

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
        public static Target DetectTarget(Template template, bool onScreen = true) // using TM_SQDIFF
        {
            Mat _template = encodedTemplates[template];
            int threshold = encodedTemplates[template, onScreen];

            Target bestGuess = DetectTargetOnScreen(_template, 0, threshold);

            for (int idx = 1; idx < screens.Length; idx++)
            {
                Target currGuess = DetectTargetOnScreen(_template, idx, threshold);

                if (currGuess.value < bestGuess.value) bestGuess = currGuess;
            }

            return bestGuess;
        }
        #endregion
    }
}
