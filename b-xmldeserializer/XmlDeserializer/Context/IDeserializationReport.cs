using System.Xml;

namespace B.XmlDeserializer.Context;

public interface IDeserializationReport
{
    DeserializationReportSeverity Severity { get; }
    XmlNode XmlNode { get; }
    string Message { get; }
}
