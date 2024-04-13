using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace BToolbox.XmlDeserializer;

public interface IDeserializer<out TElement, TEnvironment>
{
    string ElementName { get; }
    TElement Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent = null);
}
