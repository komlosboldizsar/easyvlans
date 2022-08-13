using B.XmlDeserializer.Context;
using B.XmlDeserializer.Relations;
using System.Xml;

namespace B.XmlDeserializer;

public interface IDeserializer<out TElement, TEnvironment>
{
    string ElementName { get; }
    TElement Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent = null);
}
