using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer;

public class HeterogenousListDeserializer<TElementBase, TEnvironment> :
    HeterogenousCollectionDeserializerBase<List<TElementBase>, TElementBase, TEnvironment>
{

    public HeterogenousListDeserializer(string elementName)
        : base(elementName) { }

    protected override List<TElementBase> createCollection(XmlNode xmlNode, DeserializationContext context, object parent)
        => new();

    protected override void addElementToCollection(List<TElementBase> collection, TElementBase element)
        => collection.Add(element);

}
