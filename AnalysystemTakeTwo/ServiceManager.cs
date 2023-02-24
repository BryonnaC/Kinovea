using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTranslation;
using Kinovea.ScreenManager;

namespace AnalysystemTakeTwo
{
    class ServiceManager
    {
        public ServiceManager()
        {
            CaptureScreenView.RecordingStarted += CaptureScrView_RecordingStarted;
        }

        private void CaptureScrView_RecordingStarted()
        {
            //Synchronizer.Record(true);
        }

        public static void DoMath()
        {
            MatrixMath math = new MatrixMath();

            math.ImitateMATLAB();
        }
    }
}
