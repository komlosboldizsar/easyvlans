using B.XmlDeserializer.Exceptions;
using easyvlans.GUI;
using easyvlans.Logger;
using easyvlans.Model;
using easyvlans.Model.Deserializers;
using easyvlans.Model.Remote;
using easyvlans.Modules;
using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
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
            bool dontStart = false;
            try
            {
                LogDispatcher.I("Loading configuration...");
                ConfigDeserializer.Deserializer.Register(new OneInstanceDataDeserializer(), (config, oneInstanceData) => oneInstanceDataHandler(oneInstanceData));
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
                if (e.InnerException is OneInstanceAlreadyRunningException)
                {
                    dontStart = true;
                    OneInstancePipe.SignalOneInstanceToShow(_oneInstanceData.ID);
                }
                else
                {
                    parsingError = e.Message;
                    LogDispatcher.E("XML configuration parsing error: " + e.Message);
                    if (e.InnerException != null)
                        LogDispatcher.E("Inner exception: " + e.InnerException.Message);
                }
            }
            if (!dontStart)
            {
                Task.Run(() => loadAsync(config));
                LogDispatcher.I("Starting GUI...");
                MainForm mainForm = new(config, parsingError, (_oneInstanceData != null), (_oneInstanceData?.StartVisible == false));
                Application.Run(mainForm);
            }
        }

        private async static Task loadAsync(Config config)
        {
            Task[] allReadVlansTask = new Task[config.Switches.Count];
            int i = 0;
            foreach (Switch @switch in config.Switches.Values)
                allReadVlansTask[i++] = Task.Run(@switch.ReadConfigAsync);
            await Task.WhenAll(allReadVlansTask);
        }

        private static void oneInstanceDataHandler(OneInstanceData oneInstanceData)
        {
            _oneInstanceData = oneInstanceData;
            string mutexId = ONE_INSTANCE_MUTEX_PREFIX;
            if (oneInstanceData.ID != null)
                mutexId += $"_{oneInstanceData.ID}";
            _oneInstanceMutex = new(true, mutexId, out bool mutexResult);
            if (!mutexResult)
                throw new OneInstanceAlreadyRunningException();
            OneInstancePipe.StartOneInstanceServer(oneInstanceData.ID);
        }

        private static OneInstanceData _oneInstanceData;
#pragma warning disable IDE0052
        private static Mutex _oneInstanceMutex;
#pragma warning restore IDE0052
        private const string ONE_INSTANCE_MUTEX_PREFIX = "_easyvlans_oneinstance";

    }
}
