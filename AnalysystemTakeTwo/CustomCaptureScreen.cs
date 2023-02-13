using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kinovea.Pipeline;
using Kinovea.Camera;
using Kinovea.Services;
using Kinovea.ScreenManager;

namespace AnalysystemTakeTwo
{
    class CustomCaptureScreen : CaptureScreen //- throws an exception bc of dll
    {
        private bool cameraLoaded;
        private bool cameraConnected;
        private bool prepareFailed;
        private ImageDescriptor prepareFailedImageDescriptor;
        ImageDescriptor imageDescriptor;
        CameraSummary cameraSummary;
        CameraManager cameraManager;
        ICaptureSource cameraGrabber;

        //private Metadata metadata;

/*        public ImageRotation ImageRotation
        {
            get { return cameraSummary == null ? ImageRotation.Rotate0 : cameraSummary.Rotation; }
            set { ChangeRotation(value); }
        }

        private void ChangeRotation(ImageRotation rotation)
        {
            metadata.ImageRotation = rotation;

            if (rotation == cameraSummary.Rotation)
                return;

            cameraSummary.UpdateRotation(rotation);
            cameraSummary.UpdateDisplayRectangle(Rectangle.Empty);

            Disconnect();
            Connect();
        }*/

        public CustomCaptureScreen()
        {

        }

        new public void LoadCamera(CameraSummary _cameraSummary, ScreenDescriptionCapture screenDescription)
        {
            base.LoadCamera(_cameraSummary, screenDescription);
            /*if (cameraLoaded)
                UnloadCamera();*//*

            cameraSummary = _cameraSummary;

            if (cameraSummary.Manager != null)
            {
                cameraManager = cameraSummary.Manager;
                cameraGrabber = cameraManager.CreateCaptureSource(cameraSummary);
                base.cameraGrabber = this.cameraGrabber;
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
            *//*log.DebugFormat("Restoring camera: {0}", cameraSummary.Alias);

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

        protected void AssociateCamera(bool connect)
        {
            /*            if (cameraGrabber == null)
                            return;

                        //UpdateTitle();
                        base.cameraLoaded = true;

                        OnActivated(EventArgs.Empty);

                        if (connect)
                            Connect();*/
            base.AssociateCamera(connect);
        }

        new private void Connect()
        {
            base.Connect();
            /*if (!cameraLoaded || cameraGrabber == null)
                return;

            *//*if (cameraConnected)
                Disconnect();*//*

            // First we try to prepare the grabber by using the preferences and checking if it succeeded.
            // If the configuration cannot be known in advance by an API, we try to read a single frame and check its configuration.
            imageDescriptor = ImageDescriptor.Invalid;
            if (prepareFailed && prepareFailedImageDescriptor != ImageDescriptor.Invalid)
            {
                imageDescriptor = cameraGrabber.GetPrepareFailedImageDescriptor(prepareFailedImageDescriptor);
            }
            else
            {
                imageDescriptor = cameraGrabber.Prepare();

                if (imageDescriptor == null || imageDescriptor.Format == Kinovea.Services.ImageFormat.None || imageDescriptor.Width <= 0 || imageDescriptor.Height <= 0)
                {
                    cameraGrabber.Close();

                    imageDescriptor = ImageDescriptor.Invalid;
                    prepareFailed = true;
                    Console.WriteLine("The camera does not support configuration and we could not preallocate buffers.");

                    // Attempt to retrieve an image and look up its format on the fly.
                    // This is asynchronous. We'll come back here after the image has been captured or a timeout expired.
                    //cameraManager.CameraThumbnailProduced += cameraManager_CameraThumbnailProduced;
                    //cameraManager.StartThumbnail(cameraSummary);
                }
            }

            if (imageDescriptor == ImageDescriptor.Invalid)
            {
                //UpdateTitle();
                return;
            }

            // At this point we have a proper image descriptor, but it is possible that we are on a Thumbnailer thread.

*//*            if (dummy.InvokeRequired)
                dummy.BeginInvoke((Action)delegate { Connect2(); });
            else*//*
            base.Connect2();*/
        }

        /*private void Connect2()
        {
            Console.WriteLine("wow you made it all the way here");
            // Second part of Connect function. 
            // The function is split because the first part might need to be run repeatedly and from non UI thread, 
            // while this part must run on the UI thread.
            AllocateDelayer();
            bool sideways = ImageRotation == ImageRotation.Rotate90 || ImageRotation == ImageRotation.Rotate270;
            Size referenceSize = sideways ? new Size(imageDescriptor.Height, imageDescriptor.Width) : new Size(imageDescriptor.Width, imageDescriptor.Height);
            SanityCheckDisplayRectangle(cameraSummary, referenceSize);

            metadata.ImageSize = referenceSize;
            metadata.ImageAspect = Convert(cameraSummary.AspectRatio);
            metadata.ImageRotation = cameraSummary.Rotation;
            metadata.Mirrored = cameraSummary.Mirror;
            metadata.PostSetupCapture();

            // Make sure the viewport will not use the bitmap allocated by the consumerDisplay as it is about to be disposed.
            viewportController.ForgetBitmap();
            viewportController.InitializeDisplayRectangle(cameraSummary.DisplayRectangle, referenceSize);

            // The behavior of how we pull frames from the pipeline, push them to the delayer, record them to disk and display them is dependent 
            // on the recording mode (even while not recording). The recoring mode does not change for the camera connection session. 
            recordingMode = PreferencesManager.CapturePreferences.RecordingMode;

            if (recordingMode == CaptureRecordingMode.Camera)
            {
                // Start consumer thread for recording mode "camera".
                // This is used to pull frames from the pipeline and push them directly to disk.
                // It will be dormant until recording is started but it has the same lifetime as the pipeline.
                consumerRealtime = new ConsumerRealtime(shortId);
                recorderThread = new Thread(consumerRealtime.Run) { IsBackground = true };
                recorderThread.Name = consumerRealtime.GetType().Name + "-" + shortId;
                recorderThread.Start();

                pipelineManager.Connect(imageDescriptor, cameraGrabber, consumerDisplay, consumerRealtime);
            }
            else if (recordingMode == CaptureRecordingMode.Delay || recordingMode == CaptureRecordingMode.Scheduled)
            {
                // Start consumer thread for recording mode "delay".
                // This is used to pull frames from the pipeline and push them in the delayer, 
                // and then pull frames from the delayer and write them to disk.
                consumerDelayer = new ConsumerDelayer(shortId);
                recorderThread = new Thread(consumerDelayer.Run) { IsBackground = true };
                recorderThread.Name = consumerDelayer.GetType().Name + "-" + shortId;
                recorderThread.Start();

                pipelineManager.Connect(imageDescriptor, cameraGrabber, consumerDisplay, consumerDelayer);

                // The delayer life is synched with the grabbing, which is connect/disconnect.
                // So we can activate the consumer right away.
                consumerDelayer.PrepareDelay(delayer);
                consumerDelayer.Activate();
            }

            nonGrabbingInteractionTimer.Enabled = false;

            // Keep ts per frame in sync.
            // This means that if we have imported keyframes, their time should be kept the same.
            if (cameraGrabber.Framerate != 0)
                metadata.AverageTimeStampsPerFrame = (long)(metadata.AverageTimeStampsPerSecond / cameraGrabber.Framerate);

            // Start the low frequency / low precision timer.
            // This timer is used for display and to feed the delay buffer when using recording mode "Camera".
            // No point displaying images faster than what the camera produces, or that the monitor can show, but floor at 1 fps.
            double displayFramerate = PreferencesManager.CapturePreferences.DisplaySynchronizationFramerate;
            double monitorFramerate = GetMonitorFramerate();

            double slowFramerate = Math.Min(displayFramerate, monitorFramerate);
            if (cameraGrabber.Framerate != 0)
                slowFramerate = Math.Min(slowFramerate, cameraGrabber.Framerate);

            slowFramerate = Math.Max(slowFramerate, 1);

            displayTimer.Interval = (int)(1000.0 / slowFramerate);
            displayTimer.Enabled = true;
            cameraGrabber.GrabbingStatusChanged += Grabber_GrabbingStatusChanged;
            cameraGrabber.Start();

            //UpdateTitle();
            cameraConnected = true;

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Connected to camera.");
            Console.WriteLine("Image: {0}, {1}x{2}px, top-down: {3}.", imageDescriptor.Format, imageDescriptor.Width, imageDescriptor.Height, imageDescriptor.TopDown);
            Console.WriteLine("Nominal camera framerate: {0:0.###} fps, Monitor framerate: {1:0.###} fps, Custom display framerate: {2:0.###} fps, Final display framerate: {3:0.###} fps.",
                cameraGrabber.Framerate, monitorFramerate, displayFramerate, slowFramerate);
            Console.WriteLine("Recording mode: {0}, Compositor mode: {1}.", recordingMode, PreferencesManager.CapturePreferences.DelayCompositeConfiguration.CompositeType);
            Console.WriteLine("--------------------------------------------------");
        }*/
    }
}
