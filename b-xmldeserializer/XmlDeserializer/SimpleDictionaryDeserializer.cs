using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer;

public class SimpleDictionaryDeserializer<TKey, TElement, TEnvironment> : SimpleCollectionDeserializerBase<Dictionary<TKey, TElement>, TElement, TEnvironment>
{

    private readonly Func<TElement, TKey> getKey;

    public SimpleDictionaryDeserializer(string elementName, IDeserializer<TElement, TEnvironment> elementDeserializer, Func<TElement, TKey> getKey)
        : base(elementName, elementDeserializer)
        => this.getKey = getKey;

    protected override Dictionary<TKey, TElement> createCollection(XmlNode xmlNode, DeserializationContext context, object parent)
        => new();

    protected override void addElementToCollection(Dictionary<TKey, TElement> collection, TElement element)
        => collection.Add(getKey(element), element);

}
