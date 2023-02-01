namespace easyvlans.Logger
{
    public class LogDispatcher
    {

        public delegate void NewLogMessageDelegate(DateTime Timestamp, LogMessageSeverity severity, string message);
        public static event NewLogMessageDelegate NewLogMessage;

        public static readonly List<LogMessage> Messages = new();

        public static void Log(LogMessageSeverity severity, string message)
        {
            DateTime timestamp = DateTime.Now;
            Messages.Add(new LogMessage(timestamp, severity, message));
            NewLogMessage?.Invoke(timestamp, severity, message);
        }

        public static void E(string message) => Log(LogMessageSeverity.Error, message);
        public static void W(string message) => Log(LogMessageSeverity.Warning, message);
        public static void I(string message) => Log(LogMessageSeverity.Info, message);
        public static void V(string message) => Log(LogMessageSeverity.Verbose, message);
        public static void VV(string message) => Log(LogMessageSeverity.VerbosePlus, message);

    }
}
