using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer.Attributes;

public abstract class XmlAttributeParser<TValue, TData>
    where TData : XmlAttributeParserData<TValue>, new()
{

    protected readonly XmlNode node;
    protected readonly string attributeName;
    protected readonly XmlAttribute attribute;
    protected readonly TData data = new();
    protected readonly DeserializationContext context;

    public XmlAttributeParser(XmlNode node, string attributeName, TData data, DeserializationContext context)
    {
        this.node = node;
        this.attributeName = attributeName;
        attribute = node.Attributes[attributeName];
        this.data = data;
        this.context = context;
    }

    public XmlAttributeData<TValue> Get()
    {
        if (attribute == null)
        {
            if (data.mandatory)
                throw new MandatoryAttributeNotFoundException(node, attributeName);
            return new(attribute, data.defaultValue);
        }
        return new(attribute, getFromString(attribute.Value));
    }

    protected abstract TValue getFromString(string stringValue);

    protected void throwArgumentInvalidException(string message)
        => throw new AttributeValueInvalidException(message, attribute);

}
