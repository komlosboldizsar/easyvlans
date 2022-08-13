using B.XmlDeserializer.Exceptions;
using B.XmlDeserializer.Helpers;
using System.Xml;

namespace B.XmlDeserializer.Context;

public class NotDeserializedItemReport : DeserializationReportBase
{

    public Type TargetType { get; }

    public NotDeserializedItemReport(DeserializationException exception, Type targetType = null)
        : base(DeserializationReportSeverity.Error, exception.XmlNode, formatMessage(exception.Message, targetType))
        => TargetType = targetType;

    public NotDeserializedItemReport(XmlNode xmlNode, string reason, Type targetType = null)
        : base(DeserializationReportSeverity.Error, xmlNode, formatMessage(reason, targetType))
        => TargetType = targetType;

    private static string formatMessage(string reason, Type targetType)
        => $"Element not deserialized{formatTargetType(targetType)}, reason: [{reason}]";

    private static string formatTargetType(Type targetType)
        => targetType != null ? $" as [{targetType.Reference()}]" : "";

}

public static class NotDeserializedItemReportExtensions
{

    public static void ReportNotDeserializedItem(this DeserializationContext context, DeserializationException exception, Type targetType = null)
        => context.Report(new NotDeserializedItemReport(exception, targetType));

    public static void ReportNotDeserializedItem(this DeserializationContext context, XmlNode xmlNode, string reason, Type targetType = null)
        => context.Report(new NotDeserializedItemReport(xmlNode, reason, targetType));

}

