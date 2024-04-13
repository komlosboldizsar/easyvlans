using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace BToolbox.XmlDeserializer;

public class StringValueDeserializer<TEnvironment> : ElementDeserializer<string, TEnvironment>
{

    public StringValueDeserializer(string _elementName)
        => elementName = _elementName;

    private string elementName;
    public override string ElementName => elementName;

    protected override string createElement(XmlNode elementNode, DeserializationContext context, object parent)
        => elementNode.InnerText;

}
