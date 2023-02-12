using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AnalysystemTakeTwo
{
    public static class SoftwareManager
    {
        public static string camDirectory;

        public static void StartUp()
        {
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(folder, "AnalysisApp");
            Directory.CreateDirectory(specificFolder);
            camDirectory = Path.Combine(specificFolder, "Plugins", "Camera");
            Directory.CreateDirectory(camDirectory);
        }
    }
}
