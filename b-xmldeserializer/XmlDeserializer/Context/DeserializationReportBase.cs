using System.Xml;

namespace B.XmlDeserializer.Context;

public abstract class DeserializationReportBase : IDeserializationReport
{

    public DeserializationReportSeverity Severity { get; }
    public XmlNode XmlNode { get; }
    public string Message { get; }

    public DeserializationReportBase(DeserializationReportSeverity severity, XmlNode xmlNode, string message)
    {
        Severity = severity;
        XmlNode = xmlNode;
        Message = message;
    }

}
