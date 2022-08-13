using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer;

public class RootDeserializer<TEnvironment>
{

    private readonly IDeserializer<TEnvironment, TEnvironment> deserializer;
    private readonly Action<DeserializationContext> contextInitializer;

    public RootDeserializer(IDeserializer<TEnvironment, TEnvironment> deserializer, Action<DeserializationContext> contextInitializer = null)
    {
        this.deserializer = deserializer;
        this.contextInitializer = contextInitializer;
    }

    public TEnvironment Deserialize(XmlNode rootNode, out DeserializationContext context, DeserializationContext.ReportHandler reportHandler = null)
    {
        context = new();
        contextInitializer?.Invoke(context);
        context.ReportHandlers += reportHandler;
        TEnvironment result = deserializer.Parse(rootNode, context, out var relationData);
        relationData?.BuildRelations(result, context);
        return result;
    }

    public TEnvironment Deserialize(XmlDocument document, out DeserializationContext context, DeserializationContext.ReportHandler reportHandler = null)
        => Deserialize(document.DocumentElement, out context, reportHandler);

    public TEnvironment Deserialize(string filePath, out DeserializationContext context, DeserializationContext.ReportHandler reportHandler = null)
    {
        XmlDocument doc = new();
        doc.Load(filePath);
        return Deserialize(doc, out context, reportHandler);
    }

}
