using BToolbox.XmlDeserializer.Context;

namespace BToolbox.XmlDeserializer.Relations;

public interface IRelationBuilder<TEnvironment>
{
    void BuildRelations(TEnvironment environment, DeserializationContext context);
}
