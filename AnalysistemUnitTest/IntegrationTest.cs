using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysistemUnitTest
{
    [TestClass]
    // This class may not actually be an integration test?
    // It's only testing the integration of a few *effectively coupled* methods from
    // the *same* class (FakeUser)
    // So, probably a unit test idk
    public class SynchronizerIntegrationTest
    {
        [TestMethod]
        public void Record_Called_StartsRecording()
        {
            /** Record, when called, starts recording
             * Method: Record(isStart: true, delayUnits: Unit.Milliseconds)
             * Should begin recording in Kinovea and Sparkvue
             */
        }

        [TestMethod]
        public void Record_CalledAgain_StopsRecording()
        {
            /** Record, when called again, stops recording
             * Method: Record(isStart: false, delayUnits: Units.Milliseconds)
             * Should stop recording in Kinovea and Sparkvue
             */
        }

        [TestMethod]
        public void MakeCoincident_TwoValidCsvFiles_ReturnTwoSynchronizedCsvFiles()
        {
            /** MakeCoincident, when passed two valid CsvFiles, should return two synchronized CsvFiles
             * 
             */ 
        }
    }

    [TestClass]
    public class FileHandlerIntegrationTest
    {
        [TestMethod]
        public void ExportSparkvue_SparkvueOnScreen_CsvFileCreated()
        {
            /** Calling ExportSparkvue, when Sparkvue is on the screen, should export the recorded force data as a .csv using a time generated name
             * Method: ExportSparkvue()
             * Method Relies on:
             *    FakeUser => Mouse: MoveToAndClick(), GetCursorPos(), SetCursorPos()
             *                Keyboard: PressKey()
             *                Computer Vision: DetectTarget()
             *    Time => GetTimeStamp()
             * Test Additionally Relies on:
             *    CsvFile struct
             *    
             * Click hamburger button to open menu
             * Click Export Data
             * Generate file name
             * Type out file name
             * Hit enter to save the file
             * Attempt to load file using CsvFile (predicate: file should exist)
             */

        }

        [TestMethod]
        // mouse should not move/click, keyboard should not type, no file should be created
        public void ExportSparkvue_SparkvueNotOnScreen_NoActionTaken()
        {
            // Call ExportSparkvue, get returned EventInfo, if any properties are null then success!
        }
    }
}
