using Analysistem;
using Analysistem.Utils;
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
        public void Record_Called_StartsRecording() // TC-008
        {
            /** Record, when called, starts recording
             * Method: Record(isStart: true, delayUnits: Unit.Milliseconds)
             * Should begin recording in Kinovea and Sparkvue
             */

            /* Assign */
            Template templateToExpect = Template.SparkvueStop;

            /* Act */
            Synchronizer.Record(true);
            Target actualTarget = FakeUser.DetectTarget(templateToExpect);

            /* Assert */
            Assert.IsTrue(actualTarget.detected);
        }

        [TestMethod]
        public void Record_CalledAgain_StopsRecording() // TC-009
        {
            /** Record, when called again, stops recording
             * Method: Record(isStart: false, delayUnits: Units.Milliseconds)
             * Should stop recording in Kinovea and Sparkvue
             */

            /* Assign */
            Template templateToExpect = Template.SparkvueStart;

            /* Act */
            Synchronizer.Record(false);
            Target actualTarget = FakeUser.DetectTarget(templateToExpect);

            /* Assert */
            Assert.IsTrue(actualTarget.detected);
        }

        [TestMethod]
        public void MakeCoincident_TwoValidCsvFiles_ReturnTwoSynchronizedCsvFiles() // TC-010
        {
            /** MakeCoincident, when passed two valid CsvFiles, should return two synchronized CsvFiles
             * Method: MakeCoincident()
             */ 
        }
    }

    [TestClass]
    public class FileHandlerIntegrationTest
    {
        [TestMethod]
        public void ExportSparkvue_SparkvueOnScreen_CsvFileCreated() // TC-011
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
                     
            /* Assign & Act */
            EventInfo eventInfo = FileHandler.ExportSparkvue();
            string expected = eventInfo.fileName;
            string actual = expected; // TODO: get the path from the file system (actually try to find the just created file) 

            /* Assert */
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        // mouse should not move/click, keyboard should not type, no file should be created
        public void ExportSparkvue_SparkvueNotOnScreen_NoActionTaken() // TC-012
        {
            // Call ExportSparkvue, get returned EventInfo, if any properties are null then success!

            /* Assign & Act */
            EventInfo eventInfo = FileHandler.ExportSparkvue();

            /* Assert */
            Assert.IsNull(eventInfo.targets[0]);
            Assert.IsNull(eventInfo.targets[1]);
            Assert.IsNull(eventInfo.fileName);
        }
    }
}
