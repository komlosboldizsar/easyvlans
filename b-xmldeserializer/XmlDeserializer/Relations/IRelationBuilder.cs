using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer.Relations;

public interface IRelationBuilder<TEnvironment>
{
    void BuildRelations(TEnvironment environment, DeserializationContext context);
}
