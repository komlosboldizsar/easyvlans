namespace BToolbox.XmlDeserializer.Relations;

public interface ICompositeRelationBuilder<TEnvironment> : IRelationBuilder<TEnvironment>
{
    void Add(IRelationBuilder<TEnvironment> builder);
}
