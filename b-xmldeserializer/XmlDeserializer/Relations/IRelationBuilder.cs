using B.XmlDeserializer.Context;

namespace B.XmlDeserializer.Relations;

public interface IRelationBuilder<TEnvironment>
{
    void BuildRelations(TEnvironment environment, DeserializationContext context);
}
