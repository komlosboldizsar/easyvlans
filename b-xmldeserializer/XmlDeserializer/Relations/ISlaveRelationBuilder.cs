using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer.Relations;

public interface ISlaveRelationBuilder<TElement, TEnvironment>
{
    void BuildRelations(XmlNode xmlNode, TElement element, TEnvironment environment, DeserializationContext context);
}
