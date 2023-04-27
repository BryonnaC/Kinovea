using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinovea.ScreenManager;
using AnalysisSystemFinal.UserInterface;

namespace AnalysisSystemFinal
{
    class ServiceManager
    {
        WuAlgorithm math;
        DeprecatedAlgorithm oldmath;
        private bool hasPositionData;
        private bool hasForceData;
        private string horizPath;
        private string vertPath;
        private string forcePath;

        public ServiceManager()
        {
            math = new WuAlgorithm();
            oldmath = new DeprecatedAlgorithm();
            hasForceData = false;
            hasPositionData = false;
            //oldmath.ImitateMATLAB();
            //math.TestCSVFiles();
            //math.GraphAdjusted();
            CaptureScreenView.RecordingStarted += CaptureScrView_RecordingStarted;
            ForceSelector.ImportForce += FileSelect_ImportForce;
            PositionDataSelection.ImportPositionData += FileSelect_ImportPosition;
            //FormMultiTrajectoryAnalysis.DoCustomMath += Kinematics_DoCustomMath;
        }

        private void FileSelect_ImportPosition(object sender, PositionFileEventArgs e)
        {
            hasPositionData = true;
            horizPath = e.horizPath;
            vertPath = e.vertPath;
            CheckToCalculate();
        }

        private void FileSelect_ImportForce(object sender, ForceFileEventArgs e)
        {
            Console.WriteLine(e.filePath);
            forcePath = e.filePath;
            hasForceData = true;
            CheckToCalculate();
        }

        private void CheckToCalculate()
        {
            if(hasPositionData && hasForceData)
            {
                math.CaclulateFromImportedCSV(horizPath, vertPath, forcePath);
            }
        }

        private void Kinematics_DoCustomMath(object sender, GraphToCsvToMathEventArgs e)
        {
            //DoMath();   //this is placeholder, we're gonna need the *numbers* bby

            math.TakeInPositionValues(e.csv_StringHoriz, e.csv_StringVert);
        }

        private void CaptureScrView_RecordingStarted()
        {
            Synchronizer.Record(true);
        }

        public static void DoMath()
        {
            //MatrixMath math = new MatrixMath();

            //math.ImitateMATLAB();
        }
    }
}
