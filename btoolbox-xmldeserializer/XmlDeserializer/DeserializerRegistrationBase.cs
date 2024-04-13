using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace BToolbox.XmlDeserializer;

public abstract class DeserializerRegistrationBase<TResult, TElement, TEnvironment> : IDeserializerRegistration<TResult, TEnvironment>, IDeserializerRegistrationForElement<TElement>
{

    private readonly IDeserializer<TElement, TEnvironment> deserializer;

    public DeserializerRegistrationBase(IDeserializer<TElement, TEnvironment> deserializer)
        => this.deserializer = deserializer;

    public string ElementName => deserializer.ElementName;

    public void Parse(XmlNode node, TResult result, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent)
    {
        TElement element = deserializer.Parse(node, context, out relationBuilder, parent);
        handleResult(result, element);
    }

    protected abstract void handleResult(TResult result, TElement element);

}
