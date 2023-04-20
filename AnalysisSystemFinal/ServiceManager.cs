using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinovea.ScreenManager;

namespace AnalysisSystemFinal
{
    class ServiceManager
    {
        WuAlgorithm math;

        public ServiceManager()
        {
            //math = new WuAlgorithm();
            //math.TestCSVFiles();
            //math.GraphAdjusted();
            CaptureScreenView.RecordingStarted += CaptureScrView_RecordingStarted;
            //FormMultiTrajectoryAnalysis.DoCustomMath += Kinematics_DoCustomMath;
        }

        private void Kinematics_DoCustomMath(object sender, Kinovea.ScreenManager.GraphToCsvToMathEventArgs e)
        {
            DoMath();   //this is placeholder, we're gonna need the *numbers* bby

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
