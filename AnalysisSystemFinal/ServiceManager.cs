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
        private List<string> csv_Horiz;
        private List<string> csv_Vert;


        public ServiceManager()
        {
            math = new WuAlgorithm();
            oldmath = new DeprecatedAlgorithm();
            hasForceData = false;
            hasPositionData = false;
            csv_Horiz = new List<string>();
            csv_Vert = new List<string>();

            //oldmath.ImitateMATLAB();
            //math.TestCSVFiles();
            //math.GraphAdjusted();

            CaptureScreenView.RecordingStarted += CaptureScrView_RecordingStarted;
            ForceSelector.ImportForce += FileSelect_ImportForce;
            PositionDataSelection.ImportPositionData += FileSelect_ImportPosition;
            FormMultiTrajectoryAnalysis.UseLivePosition += Kinematics_UseLivePosition;
        }

        private void Kinematics_UseLivePosition(object sender, GraphToCsvToMathEventArgs e)
        {
            csv_Horiz = e.csv_StringHoriz;
            csv_Vert = e.csv_StringVert;

            if (math.calibrationComplete)
            {
                hasPositionData = true;
            }
            else
            {
                math.CalibratePlane(csv_Horiz, csv_Vert);
            }

            horizPath = null;
            vertPath = null;
            CheckToCalculate();
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
                if(horizPath == null)
                {
                    math.TakeInPositionValues(csv_Horiz, csv_Vert, forcePath);
                }
                else
                {
                    math.CaclulateFromImportedCSV(horizPath, vertPath, forcePath);
                }

                hasPositionData = false;
                hasForceData = false;
            }
        }

        private void CaptureScrView_RecordingStarted()
        {
            Synchronizer.Record(true);
        }

    }
}
