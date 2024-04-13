using B.XmlDeserializer.Context;
using B.XmlDeserializer.Exceptions;
using B.XmlDeserializer.Relations;
using System.Xml;

namespace B.XmlDeserializer;

public class StringValueDeserializer<TEnvironment> : ElementDeserializer<string, TEnvironment>
{

    public StringValueDeserializer(string _elementName)
        => elementName = _elementName;

    private string elementName;
    public override string ElementName => elementName;

    protected override string createElement(XmlNode elementNode, DeserializationContext context, object parent)
        => elementNode.InnerText;

}
