using BToolbox.OneInstance;
using BToolbox.XmlDeserializer.Exceptions;
using BToolbox.Logger;
using CommandLine;
using easyvlans.GUI;
using easyvlans.Model;
using easyvlans.Model.Deserializers;
using easyvlans.Model.Polling;
using easyvlans.Model.Remote;
using easyvlans.Modules;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyvlans
{
    static class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Arguments parsedArguments = Parser.Default.ParseArguments<Arguments>(args).Value;
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _ = new FileLogger()
            {
                MaxSeverity = parsedArguments.VeryVerbose ? null : LogMessageSeverity.Verbose
            };
            LogDispatcher.I("Program started.");
            ModuleLoader.LoadAndInitModules();
            LogDispatcher.I($"Loaded and initialized {ModuleLoader.InitializedModuleCount} modules.");
            Config config = null;
            string parsingError = null;
            bool start = true;
            OneInstanceData oneInstanceData = null;
            try
            {
                LogDispatcher.I("Loading configuration...");
                ConfigDeserializer.Deserializer.Register(new OneInstanceDataDeserializer(), (config, oneInstDta) =>
                {
                    oneInstanceData = oneInstDta;
                    OneInstanceGuard.Set("easyvlans", oneInstanceData.ID);
                });
                config = (new ConfigDeserializer()).LoadConfig(parsedArguments.ConfigFile ?? DEFAULT_CONFIG_FILE);
                start = OneInstanceGuard.InitAnyway();
                if (start && (config.Remotes != null))
                    foreach (IRemoteMethod remoteMethod in config.Remotes)
                        remoteMethod.Start();
            }
            catch (DeserializationException e)
            {
                parsingError = e.Message;
                LogDispatcher.E("XML configuration parsing error: " + e.Message);
                Exception innerEx = e.InnerException;
                while (innerEx != null)
                {
                    parsingError += $"\r\n--------\r\n{innerEx.Message}";
                    LogDispatcher.E("Inner exception: " + innerEx.Message);
                    innerEx = innerEx.InnerException;
                }
            }
            if (start)
            {
                if (config != null)
                    Task.Run(() => loadAsync(config));
                PollingDispatcher.Start();
                LogDispatcher.I("Starting GUI...");
                bool hideOnStartup = (oneInstanceData?.StartVisible == false);
                if (parsedArguments.Hidden)
                    hideOnStartup = true;
                if (parsedArguments.NotHidden)
                    hideOnStartup = false;
                MainForm mainForm = new(config, parsingError, (oneInstanceData != null), hideOnStartup);
                Application.Run(mainForm);
            }
        }

        private const string DEFAULT_CONFIG_FILE = "config.xml";

        private async static Task loadAsync(Config config)
        {
            Task[] allReadTask = new Task[config.Switches.Count];
            int i = 0;
            foreach (Switch @switch in config.Switches.Values)
                allReadTask[i++] = Task.Run(async () => await loadSwitchAsync(@switch));
            await Task.WhenAll(allReadTask);
        }

        private async static Task loadSwitchAsync(Switch @switch)
        {
            await @switch.ReadInterfaceStatusAsync();
            await @switch.ReadVlanMembershipAsync();
        }

    }
}
