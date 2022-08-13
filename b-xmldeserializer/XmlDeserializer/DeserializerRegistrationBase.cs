using B.XmlDeserializer.Context;
using B.XmlDeserializer.Relations;
using System.Xml;

namespace B.XmlDeserializer;

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
