using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace BToolbox.XmlDeserializer.Relations;

public interface ISlaveRelationBuilder<TElement, TEnvironment>
{
    void BuildRelations(XmlNode xmlNode, TElement element, TEnvironment environment, DeserializationContext context);
}
