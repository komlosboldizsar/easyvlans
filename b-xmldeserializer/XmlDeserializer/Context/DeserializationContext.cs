using B.XmlDeserializer.Exceptions;
using System.Xml;

namespace B.XmlDeserializer.Context;

public class DeserializationContext
{

    private readonly List<IDeserializationReport> _reports = new();
    public IReadOnlyList<IDeserializationReport> Reports => _reports.AsReadOnly();

    public delegate void ReportHandler(DeserializationContext context, IDeserializationReport report);
    public event ReportHandler ReportHandlers;

    public virtual void Report(IDeserializationReport report)
    {
        _reports.Add(report);
        ReportHandlers?.Invoke(this, report);
    }

    public virtual void Report(DeserializationReportSeverity severity, XmlNode xmlNode, string message)
        => Report(new GeneralDeserializationReport(severity, xmlNode, message));

    public virtual void Report(DeserializationException exception)
        => Report(new ExceptionDeserializationReport(exception));

    public string TranslateReportMessage(IDeserializationReport report)
    {
        string message = report.Message;
        message = _typenameTranslator.TranslateTypeNames(message);
        return message;
    }

    private readonly TypenameTranslator _typenameTranslator = new();

    public void RegisterTypeName<T>(string name)
        => _typenameTranslator.RegisterTypeName<T>(name);

    public void RegisterTypeName(Type type, string name)
        => _typenameTranslator.RegisterTypeName(type, name);

}
