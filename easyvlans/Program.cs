using easyvlans.Logger;
using easyvlans.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Config config = null;
            string parsingError = null;
            try
            {
                LogDispatcher.I("Loading configuration...");
                config = (new ConfigParser()).LoadConfig();
            }
            catch (ConfigParsingException e)
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
