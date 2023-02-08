using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer.Attributes;

public sealed class XmlAttributeConverterParser<TOutput> : XmlAttributeParser<TOutput, XmlAttributeConverterParser<TOutput>.Data>
{

    public XmlAttributeConverterParser(XmlNode node, string attributeName, Data data, DeserializationContext context, IAttributeConverter<TOutput> converter)
        : base(node, attributeName, data, context)
        => this.converter = converter;

    private readonly IAttributeConverter<TOutput> converter;

    protected override TOutput getFromString(string stringValue)
    {
        try
        {
            return converter.Convert(stringValue);
        }
        catch (ArgumentException ex)
        {
            throw new AttributeValueInvalidException(ex.Message, attribute);
        }
    }

    public class Builder : XmlAttributeParserBuilder<Builder, TOutput, Data>
    {

        public Builder(XmlNode node, string attributeName, DeserializationContext context, IAttributeConverter<TOutput> converter)
            : base(node, attributeName, context)
            => this.converter = converter;

        private readonly IAttributeConverter<TOutput> converter;

        public override XmlAttributeConverterParser<TOutput> Build()
            => new(node, attributeName, data, context, converter);

    }

    public class Data : XmlAttributeParserData<TOutput>
    { }

}

public static class XmlAttributeConverterParserHelpers
{
    public static XmlAttributeConverterParser<TOutput>.Builder AttributeAs<TOutput>(this XmlNode node, string attributeName, DeserializationContext context, IAttributeConverter<TOutput> converter)
       => new(node, attributeName, context, converter);
}