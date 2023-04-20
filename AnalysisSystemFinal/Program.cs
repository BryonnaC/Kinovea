using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using Kinovea.Camera;
using Kinovea.Video;
using Kinovea.Services;
using Kinovea.FileBrowser;
using Kinovea.ScreenManager;
using System.IO;

namespace AnalysisSystemFinal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
/*            Console.WriteLine("Hello World");
            return;*/

            // C# winforms default
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set up essential to mimicking Kinovea
            Assembly assembly = Assembly.GetExecutingAssembly();
            Software.Initialize(assembly.GetName().Version);
            Console.WriteLine("Loading video readers.");
            List<Type> videoReaders = new List<Type>();
            videoReaders.Add(typeof(Kinovea.Video.Bitmap.VideoReaderBitmap));
            videoReaders.Add(typeof(Kinovea.Video.FFMpeg.VideoReaderFFMpeg));
            videoReaders.Add(typeof(Kinovea.Video.GIF.VideoReaderGIF));
            videoReaders.Add(typeof(Kinovea.Video.SVG.VideoReaderSVG));
            videoReaders.Add(typeof(Kinovea.Video.Synthetic.VideoReaderSynthetic));
            VideoTypeManager.LoadVideoReaders(videoReaders);

            SoftwareManager.StartUp();

            Console.WriteLine("Loading built-in camera managers.");
            CameraTypeManager.LoadCameraManager(typeof(Kinovea.Camera.DirectShow.CameraManagerDirectShow));
            CameraTypeManager.LoadCameraManager(typeof(Kinovea.Camera.HTTP.CameraManagerHTTP));
            CameraTypeManager.LoadCameraManager(typeof(Kinovea.Camera.FrameGenerator.CameraManagerFrameGenerator));

            // Load camera plugins to OUR library, not Kinovea library
            Console.WriteLine("Loading camera managers plugins.");
            //CameraTypeManager.LoadCameraManagersPlugins(SoftwareManager.camDirectory);
            CameraTypeManager.LoadCameraManagersPlugins();

            ToolManager.LoadTools();

            // Set up actually just happens in the constructor
            ScreenManager screenManager = new ScreenManager();
            ServiceManager serviceManager = new ServiceManager();
            FileBrowserKernel fileBrowser = new FileBrowserKernel();

            //function used to test formulas against matlab
            //ServiceManager.DoMath();

            screenManager.LoadSplashScreen();

            //async loading here to allow us to move to dashboard without click
            Task.Run(async delegate
            {
                await ScreenManager.DelaySplashScreen();
            }).Wait();

            screenManager.FirstSwitchToDashboard();

            Application.Run();
            Application.Exit();
        }
    
        // NO LONGER USED STUFF
        /*Console.WriteLine(Application.StartupPath);
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string specificFolder = Path.Combine(folder, "AnalysisApp");
        Directory.CreateDirectory(specificFolder);
        string camDirectory = Path.Combine(specificFolder, "Plugins", "Camera");
        Directory.CreateDirectory(camDirectory);*/
    }
}
