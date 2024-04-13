using System.Xml;

namespace BToolbox.XmlDeserializer.Context;

internal class GeneralDeserializationReport : DeserializationReportBase
{
    public GeneralDeserializationReport(DeserializationReportSeverity severity, XmlNode xmlNode, string message)
        : base(severity, xmlNode, message) { }
}
