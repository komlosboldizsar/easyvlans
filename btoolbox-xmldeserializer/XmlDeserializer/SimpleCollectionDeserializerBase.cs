using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace BToolbox.XmlDeserializer;

public abstract class SimpleCollectionDeserializerBase<TCollection, TElement, TEnvironment> :
    CollectionDeserializerBase<TCollection, TElement, TEnvironment>
{

    public override string ElementName { get; }

    private readonly IDeserializer<TElement, TEnvironment> elementDeserializer;

    public SimpleCollectionDeserializerBase(string elementName, IDeserializer<TElement, TEnvironment> elementDeserializer)
    {
        ElementName = elementName;
        this.elementDeserializer = elementDeserializer;
    }

    protected override TElement parseChildNode(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent)
        => elementDeserializer.Parse(xmlNode, context, out relationBuilder, parent);

}
