using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Analysistem.Utils;
using static Analysistem.Utils.FakeUser;

namespace Analysistem
{
    // wanted to put CsvFile here but it's definitely large enough to warrant its own file

    public static class FileHandler
    {
        public static EventInfo ExportSparkvue()
        {
            Target? hamburgerTarget = DetectTarget(Template.HamburgerButton);
            Target? exportTarget = null;
            string fileName = null;

            if (hamburgerTarget is Target _hamburgerTarget)
            {
                const int timeToOpenBurger = 250; // milliseconds

                GetCursorPos(out Point originalPos); // cache original mouse position
                MoveToAndClick(_hamburgerTarget.location);
                SetCursorPos(originalPos.X, originalPos.Y); // return to original mouse position

                Thread.Sleep(timeToOpenBurger);

                exportTarget = DetectTarget(Template.ExportData);
                if (exportTarget is Target _exportTarget)
                {
                    // exported name format: 'force yyyy-MM-dd HH:mm:ss:ffff.csv'
                    fileName = $"force {DateTime.Now.GetTimestamp()}";
                    const int timeToOpenFileExplorer = 1000; // milliseconds

                    MoveToAndClick(_exportTarget.location);
                    SetCursorPos(originalPos.X, originalPos.Y); // return to original mouse position

                    Thread.Sleep(timeToOpenFileExplorer);

                    foreach (char c in fileName) PressKey(c); // type out the file name
                    PressEnter(); // save the file
                }
            }

            return new EventInfo(new Target?[] { hamburgerTarget, exportTarget }, 0, fileName);
        }

        public static void ExportKinovea()
        {
            // TODO: auto export the Kinovea stuff
        }

        public static string[] GetPaths()
        {
            // TODO: dynamically acquire paths for the CSVs (might not need this?)
            return new string[0];
        }

        /* just chuck anything that has to do with paths or the filesystem here */
    }
}
