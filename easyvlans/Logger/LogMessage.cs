using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Logger
{
    public record LogMessage(DateTime Timestamp, LogMessageSeverity Severity, string Message);
}
