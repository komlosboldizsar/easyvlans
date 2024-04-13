using BToolbox.XmlDeserializer.Exceptions;
using System.Xml;

namespace BToolbox.XmlDeserializer.Context;

internal class ExceptionDeserializationReport : IDeserializationReport
{

    public DeserializationReportSeverity Severity => DeserializationReportSeverity.Error;
    public XmlNode XmlNode => Exception.XmlNode;
    public string Message => Exception.Message;

    public DeserializationException Exception { get; }

    public ExceptionDeserializationReport(DeserializationException exception)
        => Exception = exception;

}
