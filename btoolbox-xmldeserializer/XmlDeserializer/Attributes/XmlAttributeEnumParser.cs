using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace BToolbox.XmlDeserializer.Attributes;

public sealed class XmlAttributeEnumParser<TEnum> : XmlAttributeParser<TEnum, XmlAttributeEnumParser<TEnum>.Data>
    where TEnum : Enum
{

    public XmlAttributeEnumParser(XmlNode node, string attributeName, Data data, DeserializationContext context)
        : base(node, attributeName, data, context) { }

    protected override TEnum getFromString(string stringValue)
    {
        if (!data.enumValues.TryGetValue(stringValue, out TEnum enumValue))
        {
            if (data.defaultOnUnknown)
                return data.defaultValue;
            throw new Exception();
        }
        return enumValue;
    }

    public class Builder : XmlAttributeParserBuilder<Builder, TEnum, Data>
    {

        public Builder(XmlNode node, string attributeName, DeserializationContext context)
            : base(node, attributeName, context) { }

        public override XmlAttributeEnumParser<TEnum> Build()
            => new(node, attributeName, data, context);

        public Builder Translation(string stringValue, TEnum enumValue)
        {
            data.enumValues.Add(stringValue, enumValue);
            return this;
        }

        public Builder DefaultOnUnknown(bool defaultOnUnknown)
        {
            data.defaultOnUnknown = defaultOnUnknown;
            return this;
        }

    }

    public class Data : XmlAttributeParserData<TEnum>
    {
        public Dictionary<string, TEnum> enumValues = new();
        public bool defaultOnUnknown = false;
    }

}

public static class XmlAttributeEnumParserHelpers
{
    public static XmlAttributeEnumParser<TEnum>.Builder AttributeAsEnum<TEnum>(this XmlNode node, string attributeName, DeserializationContext context)
        where TEnum : Enum
       => new(node, attributeName, context);
}

