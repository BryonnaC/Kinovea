using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Analysistem.Utils;
using static Analysistem.Utils.FakeUser;

namespace Analysistem
{
    // wanted to put CsvFile here but it's definitely large enough to warrant its own file

    static class FileHandler
    {
        public static EventInfo ExportSparkvue()
        {
            // cache original mouse position
            GetCursorPos(out Point originalPos);

            const int timeToOpenBurger = 250; // milliseconds
            Target hamburgerTarget = DetectTarget(encodedTemplates[Template.HamburgerButton]);
            MoveToAndClick(hamburgerTarget.location);
            
            Thread.Sleep(timeToOpenBurger);

            const int timeToOpenFileExplorer = 1000; // milliseconds
            Target exportTarget = DetectTarget(encodedTemplates[Template.ExportData]);
            MoveToAndClick(exportTarget.location);

            // return to original mouse position
            SetCursorPos(originalPos.X, originalPos.Y);

            Thread.Sleep(timeToOpenFileExplorer);

            // exported name format: 'force yyyy-MM-dd HH:mm:ss:ffff.csv'
            string fileName = $"force {DateTime.Now.GetTimestamp()}";
            
            foreach (char c in fileName) PressKey(c); // type out the file name
            PressEnter(); // save the file

            return new EventInfo(new Target[] { hamburgerTarget, exportTarget }, 0, fileName);
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
