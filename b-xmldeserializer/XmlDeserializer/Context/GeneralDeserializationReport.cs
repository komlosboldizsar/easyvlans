using System.Xml;

namespace B.XmlDeserializer.Context;

internal class GeneralDeserializationReport : DeserializationReportBase
{
    public GeneralDeserializationReport(DeserializationReportSeverity severity, XmlNode xmlNode, string message)
        : base(severity, xmlNode, message) { }
}
