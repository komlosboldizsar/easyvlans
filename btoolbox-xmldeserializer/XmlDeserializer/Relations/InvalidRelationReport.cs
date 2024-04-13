using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Exceptions;
using BToolbox.XmlDeserializer.Helpers;
using System.Xml;

namespace BToolbox.XmlDeserializer.Relations;

public class InvalidRelationReport : DeserializationReportBase
{

    public object Element { get; }

    public InvalidRelationReport(XmlNode xmlNode, object element, string reason)
        : base(DeserializationReportSeverity.Warning, xmlNode, $"Invalid relation for element [{element.GetType().Reference()}], reason: [{reason}]")
        => Element = element;

}

public static class InvalidRelationReportExtensions
{

    public static void ReportInvalidRelation(this DeserializationContext context, DeserializationException ex, object element)
        => context.Report(new InvalidRelationReport(ex.XmlNode, element, ex.Message));

    public static void ReportInvalidRelation(this DeserializationContext context, DeserializationException ex, XmlNode xmlNode, object element)
        => context.Report(new InvalidRelationReport(xmlNode, element, ex.Message));

    public static void ReportInvalidRelation(this DeserializationContext context, XmlNode xmlNode, object element, string reason)
        => context.Report(new InvalidRelationReport(xmlNode, element, reason));

}

