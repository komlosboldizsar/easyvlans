using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Logger
{
    public class LogDispatcher
    {

        public delegate void NewLogMessageDelegate(DateTime Timestamp, LogMessageSeverity severity, string message);
        public static event NewLogMessageDelegate NewLogMessage;

        public static List<LogMessage> Messages { get; } = new List<LogMessage>();

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



    }
}
