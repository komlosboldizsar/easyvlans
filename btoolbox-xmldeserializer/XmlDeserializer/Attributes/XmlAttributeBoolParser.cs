using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace BToolbox.XmlDeserializer.Attributes;

public sealed class XmlAttributeBoolParser : XmlAttributeParser<bool, XmlAttributeBoolParser.Data>
{

    public XmlAttributeBoolParser(XmlNode node, string attributeName, Data data, DeserializationContext context)
        : base(node, attributeName, data, context) { }

    protected override bool getFromString(string stringValue)
        => stringValue == STR_TRUE;

    private const string STR_TRUE = "true";

    public class Builder : XmlAttributeParserBuilder<Builder, bool, Data>
    {

        public Builder(XmlNode node, string attributeName, DeserializationContext context)
            : base(node, attributeName, context) { }

        public override XmlAttributeBoolParser Build()
            => new(node, attributeName, data, context);

    }

    public class Data : XmlAttributeParserData<bool> { }

}

public static class XmlAttributeBoolParserHelpers
{
    public static XmlAttributeBoolParser.Builder AttributeAsBool(this XmlNode node, string attributeName, DeserializationContext context)
       => new(node, attributeName, context);
}

