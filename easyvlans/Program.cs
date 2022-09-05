using B.XmlDeserializer.Exceptions;
using easyvlans.Logger;
using easyvlans.Model;
using easyvlans.Model.Deserializers;
using easyvlans.Model.Remote;
using easyvlans.Modules;
using System;
using System.Windows.Forms;

namespace easyvlans
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _ = new FileLogger();
            LogDispatcher.I("Program started.");
            ModuleLoader.LoadAndInitModules();
            LogDispatcher.I($"Loaded and initialized {ModuleLoader.InitializedModuleCount} modules.");
            Config config = null;
            string parsingError = null;
            try
            {
                LogDispatcher.I("Loading configuration...");
                config = (new ConfigDeserializer()).LoadConfig();
                if (config.Remotes != null)
                {
                    foreach (IRemoteMethod remoteMethod in config.Remotes)
                        remoteMethod.MeetConfig(config);
                    foreach (IRemoteMethod remoteMethod in config.Remotes)
                        remoteMethod.Start();
                }
            }
            catch (DeserializationException e)
            {
                parsingError = e.Message;
                LogDispatcher.E("XML configuration parsing error: " + e.Message);
                if (e.InnerException != null)
                    LogDispatcher.E("Inner exception: " + e.InnerException.Message);
            }
            LogDispatcher.I("Starting GUI...");
            Application.Run(new GUI.MainForm(config, parsingError));
        }

    }
}
