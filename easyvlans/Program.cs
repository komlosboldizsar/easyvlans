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
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FileLogger fileLogger = new FileLogger();
            Config config = null;
            string parsingError = null;
            try
            {
                config = (new ConfigParser()).LoadConfig();
            }
            catch (ConfigParsingException e)
            {
                parsingError = e.Message;
                LogDispatcher.E("XML configuration parsing error: " + e.Message);
                if (e.InnerException != null)
                    LogDispatcher.E("Inner exception: " + e.InnerException.Message);
            }
            Application.Run(new GUI.MainForm(config, parsingError));
        }
    }
}
