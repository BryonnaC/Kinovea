using Analysistem.Utils;
using static Analysistem.Utils.FakeUser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;

namespace Analysistem
{
    public static class Synchronizer
    {
        private static readonly Dictionary<bool, Mat> encodedTemplates = new Dictionary<bool, Mat>()
        {
            { true, LoadMat("iVBORw0KGgoAAAANSUhEUgAAAEwAAAAlCAYAAADobA+5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAP5SURBVGhD7ZlNT1NBFIbv36BUJXUDCTFYY0wAsQkbmjRu2ahsEERbqlAEAlRQoAViMNSaiHyoC03AKImLxujKRKKJ+pfGeYc717nTU3qh7YVJunh6pzNzznvO2+kHF6th5A2r4x2r4QEfeKRxZJM1T2ZZeHaMdSzcZt3Lvazn6XUWW42KK55jHuvYh/1UHpPxZNi5sTxrS0+wa9lb3Jwez2A/4hBP5TWRsoa1zqRZJHuTNMQriEceKr9pWAH+QNGUesEuzyV5w9EiA45HVORDXkrPFKzAfT7QCI2tsfb5QaLpykFe5Kd0TYAb9poP/tOUytfMLAnyQ0fXNgErkOQDhYO3Id1oNYGOrm0CLsNap9K8mWp9ZpUjKvRUfRNwDGsazVf8bXhUoAddtaDTjhUY5gNO28wE2VStga6swQS4YdssmHzFIhl/T5cEutBHHSYgDGsez5DN+AX09cLAyqd99vbrX4etL39YX75A7vULYVg4nSIb8Qvo64XBrPXCbxZb3itak6Tff6+6iYnNb+LFwZVatwKJbdYx30824hfQRx2S2NKeMAuGqPM6LsOI9eOQ2LAN41dqXRjWvdRLNuIX0FeLggEwYuXjvmteBWvk25WvScPlWu7zL3Z1bteJlevIIQ3CnsyHH06MRDfOCsS37Vs0dDN+AH3UoSINcZrV1kH6nXLC7DmYgRhc8Vyaj73qHhgGYJKcB64TpsxLhGH+/VgtRZQsTj1FasMSyjAdmA0DxWm156Rh1ItR1rBG/hA94RMGfdRBIZuTp61rdtdZcwx7XnDFqGA/4tRYmRMm6vtVw/Q1IAw7DZ9hemE6MAeNqE2WMgzPMY/9kuoZdm+LdT452W9J6KOOw+ia3VFOyo6Ycxlm75MNCzNKxMWyimF2nMRlmLYGhGHhmVGyEb+Avl6YjlfDYALMgCml4io2rCW1SDbiF9BXi+rLFdizvZ+s69FBg4BqhJpzTOQ51D3CMDvfYYYhDvHIo68BYdiZ+DqLLJ7Q35JcF/p6YbJwNAtUE1TQtLoOU2COjMO6NE2evMMMAzBLxqsvBrAa7/IB5+LUONlQrYGurMEErCB/AKFkjkUyN8imagX0oCtrMAErOMQHNhcmpnkj/t1xhZ6qbwIuw8CVdIJorvpAR9c2gSLDQokca58bIJusFsgPHV3bBKzgHT7QOD+8WjPTkBf5KV0T4IZt8kExoXjOfntW7z/fyIe8lJ4plDRMcmF8mv9WquzbE/HIQ+U3jbKGgVB8jYUnHx75xy32Iw7xVF4TsYKDfOCRs0MvWcvIArs0Nco6H/eLuwwHt4ai4ornmMc69mE/lcdkuGEbfFDHK1ZwgA/qeKZu2JHYYP8AO4gg4ZYduUsAAAAASUVORK5CYII=") },
            { false, LoadMat("iVBORw0KGgoAAAANSUhEUgAAAEgAAAAgCAIAAAA+KKknAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAARHSURBVFhH3Vd9bBRFFN/ZlmuhvaOf0IS7Hldp0RD6HT0qBCxtTMSIPUwFNTGEfzTRGE+TiqQpgcSkJhwGDIEA0X+IQKClYDSmVDSxVfpFC5Kq5evahkh7LUpbe3cte/5mZ27duxJQurs1/PLLZd6bN7Pz5r03M0dsNScEQSAhKSk4YQkEiBCCOHNIRBycm+CPNXHZcBDb9joiSc6hvi09rYlTAa7WAjfnWnYUl/8RN4/LxgKO1VuCE7tazuQPD3CddvDkrfliSSEXjIUI30xSaP6kXxSI5jRPBjH/rBCOoapCIYGQUEhzalSwDwMaMRIicJLoAfqJiI00jCLbVOqXWq0R6Q/mnw3SVHwkITsGF3WCfjM/CCJyRatLeTrkhKTzG0/UGG2oC0Nbqr7FWbI4/bz7We/29YyXt65z5dqibGZOo2usxJG220Wv7E2ft9hrToPLPvqqrpu+DVx51ssfPnf2rWdkw5lCPhUZ9YAyeZjOxWkpCabvegdbrvmiuijvNeThGCtPZhwWmuNNMaI1OfoB6S59/M1VS9CVvcDs3fFC7+Bo2afnoKcR3lCYYYlnZkc7+qoaulibDWn1jgzc/mtjUSY0Y4Gp6i8vsvgjYvLDQy/Ik6vYcLH/9zv+lVlpNOVUek9TT1V9F1YGl+zVDWV7v4XSlWs9+MqTmGXTZ81QHu3wwoEjr6/go2hohGUZliJbMnphg+E7n8/FKPQaXWPIwHdPdsA3Gpmd62tfzOcd98KGApspVjzW6aV5KwhVp7rgdr412ZVnYwaAbyxAd0GeGZawf8qRClEk3HN9gDfoNP541ef8+Js9534N3pU2FtnPvl3K9AoUS+TtyHjw/DWfounsH0mMi3U6UpkIDI76ld4bvvHglGRLmof2rL08PE2/5NScQQQQuiObS7hWhZKsNHP8HC78d8g1pmPIMPc/hTSd+77/DYWxwBzPNaohLVeHRv2TihgmM5CVCsK9jtQEpCLTzFrE/g2QZrgbnFnpXBaEwswUbMRP12nJTQdSF78dfSP4Dd9jOoFNrmJtRUHjO2sV8aXCTOzx1z/fRPvWnxNYNNIPRzzrPdHZh5qpLLIzDcYib7sHbtdf6GcGwNOPpUOPdkW+bd3yRd7hcU9jD0Ri31qf6h/b13oq586QvBYtsT97xYGlTi6EUesqeLnYztrwpPp0N12oDHfZE2+szsZthtor/6QJGlSap7JYuceOtXur6i6wNjNuuzGMTIbD0OCwdR9vZ0cosX8gO9amm2M50Y5pBXe57Nj14VcPN3OVCux1D8hx1RoRz1Ktqaw5Ss9o9FtRYzJEKWX+r0/F+2N3Y8/SbQ2vHfqBy5HQ+VQEVLtoJI0+7g0jUhH/n6UY9UWuHURBiviagaQRG42J602kL2LNcWl+RuTnjCNxvH8SK0icDJTeurLQP8YWpAkuJWW0p1inxBguGwvieI869qhBEP4GgyulNRPb7m0AAAAASUVORK5CYII=") }
        };

        #region WHERE THE MAGIC HAPPENS
        // start/stop recording: Synchronous
        public static Data Record(bool isStart, Units delayUnits = Units.Milliseconds)
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

            return new Data(new Target[] { sparkvueTarget }, stopwatch.Elapsed.ToUnits(delayUnits));
        }

        // start/stop recording: Asynchronous
        public static Data RecordThreads(bool isStart, Units delayUnits = Units.Milliseconds)
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

            return new Data(new Target[] { sparkvueTarget }, stopwatch.Elapsed.ToUnits(delayUnits));
        }

        public static CsvFile MakeCoincident(CsvFile kinovea, CsvFile sparkvue)
        {
            // TODO:  synchronize csv
            return new CsvFile();
        }
        #endregion
    }
}
