using B.XmlDeserializer.Context;
using B.XmlDeserializer.Exceptions;
using B.XmlDeserializer.Relations;
using System.Xml;

namespace B.XmlDeserializer;

public abstract class HeterogenousCollectionDeserializerBase<TCollection, TElementBase, TEnvironment> :
    CollectionDeserializerBase<TCollection, TElementBase, TEnvironment>
{

    public override string ElementName { get; }

    public HeterogenousCollectionDeserializerBase(string elementName)
        => ElementName = elementName;

    protected override TElementBase parseChildNode(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent)
    {
        if (!registrations.TryGetValue(xmlNode.LocalName, out IDeserializer<TElementBase, TEnvironment> deserializer))
            throw new UnexpectedElementNameException(xmlNode, getExpectedElementNames());
        return deserializer.Parse(xmlNode, context, out relationBuilder, parent);
    }

    private readonly Dictionary<string, IDeserializer<TElementBase, TEnvironment>> registrations = new();

    public void Register(IDeserializer<TElementBase, TEnvironment> deserializer)
        => registrations.Add(deserializer.ElementName, deserializer);

    private string[] getExpectedElementNames() => registrations.Select(r => r.Value.ElementName).ToArray();

}
