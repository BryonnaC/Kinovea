using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using Kinovea.Camera;
using Kinovea.Video;
using Kinovea.Services;
using System.IO;

namespace AnalysystemTakeTwo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Assembly assembly = Assembly.GetExecutingAssembly();

            /*Console.WriteLine(Application.StartupPath);
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(folder, "AnalysisApp");
            Directory.CreateDirectory(specificFolder);
            string camDirectory = Path.Combine(specificFolder, "Plugins", "Camera");
            Directory.CreateDirectory(camDirectory);*/

            Console.WriteLine(Directory.GetCurrentDirectory());

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

            // Do camera shit
            Console.WriteLine("Loading camera managers plugins.");
            CameraTypeManager.LoadCameraManagersPlugins(SoftwareManager.camDirectory);
            
            Console.WriteLine(Directory.GetCurrentDirectory());

            ScreenManager screenManager = new ScreenManager();
            screenManager.ShowInitialScreen();

            Application.Run();
        }
    }
}
