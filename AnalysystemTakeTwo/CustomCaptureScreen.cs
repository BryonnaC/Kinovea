using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinovea.Pipeline;
using Kinovea.Camera;
using Kinovea.Services;

namespace AnalysystemTakeTwo
{
    class CustomCaptureScreen
    {
        bool cameraLoaded;
        bool cameraConnected;

        ImageDescriptor imageDescriptor;
        CameraSummary cameraSummary;
        CameraManager cameraManager;
        ICaptureSource cameraGrabber;
        

        public void LoadCamera(CameraSummary _cameraSummary, ScreenDescriptionCapture screenDescription)
        {
            /*if (cameraLoaded)
                UnloadCamera();*/

            cameraSummary = _cameraSummary;

            if (cameraSummary.Manager != null)
            {
                cameraManager = cameraSummary.Manager;
                cameraGrabber = cameraManager.CreateCaptureSource(cameraSummary);
                AssociateCamera(true);
                return;
            }

            // No camera manager in the camera summary: special case for when we want to load a camera from launch settings.

            if (string.IsNullOrEmpty(cameraSummary.Alias))
            {
                // Loading an empty screen through launch settings. Our job is done here.
                return;
            }

            // Loading a camera through launch settings.
            // At this point we don't know if the camera has been discovered yet or not.
            /*log.DebugFormat("Restoring camera: {0}", cameraSummary.Alias);

            CameraSummary summary2 = CameraTypeManager.GetCameraSummary(cameraSummary.Alias);
            if (summary2 != null)
            {
                log.DebugFormat("Camera is already known.");

                if (CameraDiscoveryComplete != null)
                    CameraDiscoveryComplete(this, new EventArgs<string>(cameraSummary.Alias));

                // Finish loading the screen.
                cameraSummary = summary2;
                cameraManager = cameraSummary.Manager;
                cameraGrabber = cameraManager.CreateCaptureSource(cameraSummary);

                bool connect = screenDescription != null ? screenDescription.Autostream : true;
                AssociateCamera(connect);

                if (screenDescription != null && cameraLoaded && cameraConnected)
                {
                    view.ForceDelaySeconds(screenDescription.Delay);
                    delayedDisplay = screenDescription.DelayedDisplay;
                }
            }
            else
            {
                // We don't know about this camera yet. Go through normal discovery.
                this.screenDescription = screenDescription;
                stopwatchDiscovery.Start();
                CameraTypeManager.CamerasDiscovered += CameraTypeManager_CamerasDiscovered;
                CameraTypeManager.StartDiscoveringCameras();
            }*/
        }

        private void AssociateCamera(bool connect)
        {

        }

        private void Connect()
        {

        }
        
        private void Connect2()
        {

        }
    }
}
