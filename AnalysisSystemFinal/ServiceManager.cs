using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTranslation;
using Kinovea.ScreenManager;

namespace AnalysisSystemFinal
{
    class ServiceManager
    {
        MatrixMath math;

        public ServiceManager()
        {
            math = new MatrixMath();
            
            //CaptureScreenView.RecordingStarted += CaptureScrView_RecordingStarted;
            FormMultiTrajectoryAnalysis.DoCustomMath += Kinematics_DoCustomMath;
        }

        private void Kinematics_DoCustomMath(object sender, Kinovea.ScreenManager.GraphToCsvToMathEventArgs e)
        {
            DoMath();   //this is placeholder, we're gonna need the *numbers* bby

            if(e.csv_StringHoriz.Count < 12)
            {
                math.Calibrate(e.csv_StringHoriz, e.csv_StringVert);
            }
        }

        private void CaptureScrView_RecordingStarted()
        {
            //Synchronizer.Record(true);
        }

        public static void DoMath()
        {
            //MatrixMath math = new MatrixMath();

            //math.ImitateMATLAB();
        }
    }
}
