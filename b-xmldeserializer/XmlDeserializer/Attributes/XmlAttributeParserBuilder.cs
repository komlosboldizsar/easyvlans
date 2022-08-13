using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer.Attributes;

public abstract class XmlAttributeParserBuilder<TBuilder, TValue, TData>
    where TBuilder : XmlAttributeParserBuilder<TBuilder, TValue, TData>
    where TData : XmlAttributeParserData<TValue>, new()
{

    protected readonly XmlNode node;
    protected readonly string attributeName;
    protected readonly TData data = new();
    protected readonly DeserializationContext context;

    public XmlAttributeParserBuilder(XmlNode node, string attributeName, DeserializationContext context)
    {
        this.node = node;
        this.attributeName = attributeName;
        this.context = context;
    }

    public abstract XmlAttributeParser<TValue, TData> Build();

    public XmlAttributeData<TValue> Get()
        => Build().Get();

    public TBuilder Mandatory()
    {
        data.mandatory = true;
        return (TBuilder)this;
    }

    public TBuilder Default(TValue defaultValue)
    {
        data.defaultValue = defaultValue;
        return (TBuilder)this;
    }

}
