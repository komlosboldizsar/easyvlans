using System.Xml;

namespace BToolbox.XmlDeserializer.Context;

public interface IDeserializationReport
{
    DeserializationReportSeverity Severity { get; }
    XmlNode XmlNode { get; }
    string Message { get; }
}
