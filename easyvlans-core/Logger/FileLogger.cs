namespace easyvlans.Logger
{
    public class FileLogger : IDisposable
    {

        private const string LOG_FOLDER_PATH = @".\log";
        private readonly string filePath = null;
        private readonly LogMessageSeverity? maxSeverity;
        private StreamWriter streamWriter;

        public FileLogger(LogMessageSeverity? maxSeverity = null)
        {
            this.maxSeverity = maxSeverity;
            string startTimestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            filePath = $"{LOG_FOLDER_PATH}{Path.DirectorySeparatorChar}easyvlans-{startTimestamp}.log";
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            FileStreamOptions streamWriterOptions = new();
            streamWriterOptions.Access = FileAccess.Write;
            streamWriterOptions.Mode = FileMode.Create;
            streamWriterOptions.Share = FileShare.Read;
            streamWriter = new(filePath, streamWriterOptions)
            {
                AutoFlush = true
            };
            LogDispatcher.NewLogMessage += newLogMessageHandler;
        }

        public void Dispose()
        {
            streamWriter?.Dispose();
            streamWriter = null;
        }

        private void newLogMessageHandler(DateTime timestamp, LogMessageSeverity severity, string message)
        {
            if ((maxSeverity != null) && (severity > maxSeverity))
                return;
            lock (streamWriter)
            {
                streamWriter.WriteLine($"[{timestamp:HH:mm:ss}][{convertTypeToString(severity)}] {message}");
            }
        }

        private static string convertTypeToString(LogMessageSeverity severity)
            => severity switch
            {
                LogMessageSeverity.Error => "ERROR",
                LogMessageSeverity.Warning => "WARN",
                LogMessageSeverity.Info => "INFO",
                LogMessageSeverity.Verbose => "VERBOSE",
                LogMessageSeverity.VerbosePlus => "VERBOSE+",
                _ => "?",
            };

    }
}
