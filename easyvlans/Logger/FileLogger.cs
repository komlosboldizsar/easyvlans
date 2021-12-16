using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Logger
{

    public class FileLogger
    {

        private const string LOG_FOLDER_PATH = ".\\log";
        private string filePath = null;

        public FileLogger()
        {
            string startTimestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            filePath = $"{LOG_FOLDER_PATH}{Path.DirectorySeparatorChar}easyvlans-{startTimestamp}.log";
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            LogDispatcher.NewLogMessage += NewLogMessageHandler;
        }

        private void NewLogMessageHandler(DateTime timestamp, LogMessageSeverity severity, string message)
        {
            string textToAdd = $"[{timestamp.ToString("HH:mm:ss")}][{convertTypeToString(severity)}] {message}";
            writeToFile(textToAdd);
        }

        private void writeToFile(string str)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(str);
            }
        }

        private string convertTypeToString(LogMessageSeverity severity)
        {
            switch (severity)
            {
                case LogMessageSeverity.Verbose:
                    return "VERBOSE";
                case LogMessageSeverity.Info:
                    return "INFO";
                case LogMessageSeverity.Warning:
                    return "WARN";
                case LogMessageSeverity.Error:
                    return "ERROR";
            }
            return "?";
        }

    }

}
