namespace easyvlans.Logger
{
    public class FileLogger
    {

        private const string LOG_FOLDER_PATH = @".\log";
        private readonly string filePath = null;

        public FileLogger()
        {
            string startTimestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            filePath = $"{LOG_FOLDER_PATH}{Path.DirectorySeparatorChar}easyvlans-{startTimestamp}.log";
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            LogDispatcher.NewLogMessage += newLogMessageHandler;
        }

        private void newLogMessageHandler(DateTime timestamp, LogMessageSeverity severity, string message)
            => writeToFile($"[{timestamp:HH:mm:ss}][{convertTypeToString(severity)}] {message}");

        private void writeToFile(string str)
        {
            using StreamWriter sw = new(filePath, true);
            sw.WriteLine(str);
        }

        private static string convertTypeToString(LogMessageSeverity severity)
            => severity switch
            {
                LogMessageSeverity.Verbose => "VERBOSE",
                LogMessageSeverity.Info => "INFO",
                LogMessageSeverity.Warning => "WARN",
                LogMessageSeverity.Error => "ERROR",
                _ => "?",
            };

    }
}
