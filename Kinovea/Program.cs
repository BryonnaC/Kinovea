/*
Copyright � Joan Charmant 2008.
jcharmant@gmail.com 
 
This file is part of Kinovea.

Kinovea is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License version 2 
as published by the Free Software Foundation.

Kinovea is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Kinovea. If not, see http://www.gnu.org/licenses/.

 */

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using Kinovea.Services;
using System.Diagnostics;

namespace Kinovea.Root
{
    internal static class Program
    {
        const int windowView = 2; //used for testing purposes to launch window
                                  //will be changed later
        private static bool FirstInstance
        {
            get
            {
                bool gotMutex;
                mutex = new Mutex(false, "Local\\" + appGuid, out gotMutex);
                return gotMutex;
            }
        }
        private static Mutex mutex;
        private static string appGuid = "b049b83e-90f3-4e84-9289-52ee6ea2a9ea";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;
            
            Thread.CurrentThread.Name = "Main";
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            Software.Initialize(assembly.GetName().Version);
            Software.LogInfo();

            Software.SanityCheckDirectories();
            PreferencesManager.Initialize();
            bool firstInstance = Program.FirstInstance;
            if (!firstInstance && !PreferencesManager.GeneralPreferences.AllowMultipleInstances)
                return;

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                CommandLineArgumentManager.Instance.ParseArguments(args);

            Software.ConfigureInstance();
            if (!string.IsNullOrEmpty(Software.InstanceName) && PreferencesManager.GeneralPreferences.InstancesOwnPreferences)
                PreferencesManager.Initialize();

            log.Debug("Application level initialisations.");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            log.Debug("Showing SplashScreen.");
            FormSplashScreen splashForm = new FormSplashScreen();
            splashForm.Show();
            splashForm.Update();

            //if statement for testing purposes -- will be changed TBD
            
            if(windowView == 1)
            {
                RootKernel kernel = new RootKernel(windowView);
                kernel.Prepare();
            
                log.Debug("Closing splash screen.");
                splashForm.Close();

                log.Debug("Launching.");
                kernel.Launch();
            }
            else if(windowView == 2)
            {
                Console.WriteLine("hey i'm here");
                UIAdapter kernel = new UIAdapter(windowView);
                kernel.Prepare();

                log.Debug("Closing splash screen.");
                splashForm.Close();

                log.Debug("Launching.");
                kernel.Launch();
            }

        }
        
        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            
            string message = string.Format("Message: {0}", ex.Message);
            string source = string.Format("Source: {0}", ex.Source);
            string target = string.Format("Target site: {0}", ex.TargetSite);
            string inner = string.Format("InnerException: {0}", ex.InnerException);
            string trace = string.Format("Stack: {0}", ex.StackTrace);
            
            string dumpFile = string.Format("Unhandled Crash - {0}.txt", Guid.NewGuid());
            using (StreamWriter sw = File.AppendText(Path.Combine(Software.SettingsDirectory, dumpFile)))
            {
                sw.WriteLine(message);
                sw.WriteLine(source);
                sw.WriteLine(target);
                sw.WriteLine(inner);
                sw.WriteLine(trace);
                sw.Close();
            }
            
            // Dump again in the log.
            log.Error("----------------- Unhandled Crash -------------------------");
            log.Error(message);
            log.Error(source);
            log.Error(target);
            log.Error(inner);
            log.Error(trace);
        }
    }

    /*public static class KinoveaSystem
    {

        private static bool FirstInstance
        {
            get
            {
                bool gotMutex;
                mutex = new Mutex(false, "Local\\" + appGuid, out gotMutex);
                return gotMutex;
            }
        }
        private static Mutex mutex;
        private static string appGuid = "b049b83e-90f3-4e84-9289-52ee6ea2a9ea";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        [STAThread]
        public static void MainReplacement()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;

            Thread.CurrentThread.Name = "Main";

            Assembly assembly = Assembly.GetExecutingAssembly();
            Software.Initialize(assembly.GetName().Version);
            Software.LogInfo();

            Software.SanityCheckDirectories();
            PreferencesManager.Initialize();
            bool firstInstance = KinoveaSystem.FirstInstance;
            if (!firstInstance && !PreferencesManager.GeneralPreferences.AllowMultipleInstances)
                return;

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                CommandLineArgumentManager.Instance.ParseArguments(args);

            Software.ConfigureInstance();
            if (!string.IsNullOrEmpty(Software.InstanceName) && PreferencesManager.GeneralPreferences.InstancesOwnPreferences)
                PreferencesManager.Initialize();

            log.Debug("Application level initialisations.");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            log.Debug("Showing SplashScreen.");
            FormSplashScreen splashForm = new FormSplashScreen();
            splashForm.Show();
            splashForm.Update();

            RootKernel kernel = new RootKernel();
            kernel.Prepare();

            log.Debug("Closing splash screen.");
            splashForm.Close();

            log.Debug("Launching.");
            kernel.Launch();
        }

        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;

            string message = string.Format("Message: {0}", ex.Message);
            string source = string.Format("Source: {0}", ex.Source);
            string target = string.Format("Target site: {0}", ex.TargetSite);
            string inner = string.Format("InnerException: {0}", ex.InnerException);
            string trace = string.Format("Stack: {0}", ex.StackTrace);

            string dumpFile = string.Format("Unhandled Crash - {0}.txt", Guid.NewGuid());
            using (StreamWriter sw = File.AppendText(Path.Combine(Software.SettingsDirectory, dumpFile)))
            {
                sw.WriteLine(message);
                sw.WriteLine(source);
                sw.WriteLine(target);
                sw.WriteLine(inner);
                sw.WriteLine(trace);
                sw.Close();
            }

            // Dump again in the log.
            log.Error("----------------- Unhandled Crash -------------------------");
            log.Error(message);
            log.Error(source);
            log.Error(target);
            log.Error(inner);
            log.Error(trace);
        }
    }*/
}