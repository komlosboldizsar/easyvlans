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
            Config config = (new ConfigParser()).LoadConfig();
            Application.Run(new GUI.MainForm(config));
        }
    }
}
