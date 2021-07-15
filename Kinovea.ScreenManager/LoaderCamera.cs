﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kinovea.Camera;
using Kinovea.Services;

namespace Kinovea.ScreenManager
{
    /// <summary>
    /// Finds the best place to load the camera into, creating a new screen if necessary, and loads the camera into the chosen screen.
    /// </summary>
    public static class LoaderCamera
    {
        public static void LoadCameraInScreen(ScreenManagerKernel manager, CameraSummary summary, int targetScreen, ScreenDescriptionCapture screenDescription = null)
        {
            CameraTypeManager.CancelThumbnails();
            CameraTypeManager.StopDiscoveringCameras();

            if (targetScreen < 0)
                LoadUnspecified(manager, summary, screenDescription);
            else
                LoadInSpecificTarget(manager, targetScreen, summary, screenDescription);
        }

        private static void LoadUnspecified(ScreenManagerKernel manager, CameraSummary summary, ScreenDescriptionCapture screenDescription)
        {
            if (manager.ScreenCount == 0)
            {
                manager.AddCaptureScreen();
                LoadInSpecificTarget(manager, 0, summary, screenDescription);
            }
            else if (manager.ScreenCount == 1)
            {
                LoadInSpecificTarget(manager, 0, summary, screenDescription);
            }
            else if (manager.ScreenCount == 2)
            {
                int target = manager.FindTargetScreen(typeof(CaptureScreen));
                if (target != -1)
                    LoadInSpecificTarget(manager, target, summary, screenDescription);
            }
        }

        private static void LoadInSpecificTarget(ScreenManagerKernel manager, int targetScreen, CameraSummary summary, ScreenDescriptionCapture screenDescription)
        {
            AbstractScreen screen = manager.GetScreenAt(targetScreen);

            if (screen is CaptureScreen)
            {
                CaptureScreen captureScreen = screen as CaptureScreen;
                captureScreen.LoadCamera(summary, screenDescription);

                manager.OrganizeScreens();
                manager.OrganizeCommonControls();
                manager.OrganizeMenus();
            }
            else if (screen is PlayerScreen)
            {
                // Loading a camera onto a video should never close the video.
                // We only load the camera if there is room to create a new capture screen, otherwise we do nothing.
                if (manager.ScreenCount == 1)
                {
                    manager.AddCaptureScreen();
                    LoadInSpecificTarget(manager, 1, summary, screenDescription);
                }
            }
        }

    }
}
