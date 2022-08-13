using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer.Attributes;

public sealed class XmlAttributeStringParser : XmlAttributeParser<string, XmlAttributeStringParser.Data>
{

    public XmlAttributeStringParser(XmlNode node, string attributeName, Data data, DeserializationContext context)
        : base(node, attributeName, data, context) { }

    protected override string getFromString(string stringValue)
    {
        if (data.notEmpty && string.IsNullOrWhiteSpace(stringValue))
            throwArgumentInvalidException("Value must be not empty");
        return stringValue;
    }

    public class Builder : XmlAttributeParserBuilder<Builder, string, Data>
    {

        public Builder(XmlNode node, string attributeName, DeserializationContext context)
            : base(node, attributeName, context) { }

        public override XmlAttributeStringParser Build()
            => new(node, attributeName, data, context);

        public Builder NotEmpty()
        {
            data.notEmpty = true;
            return this;
        }

    }

    public class Data : XmlAttributeParserData<string>
    {
        public bool notEmpty;
    }

}

public static class XmlAttributeStringParserHelpers
{
    public static XmlAttributeStringParser.Builder AttributeAsString(this XmlNode node, string attributeName, DeserializationContext context)
       => new(node, attributeName, context);
}

