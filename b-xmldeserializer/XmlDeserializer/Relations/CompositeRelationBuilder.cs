using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer.Relations;

internal class CompositeRelationBuilder<TElement, TEnvironment> : RelationBuilder<TElement, TEnvironment>, ICompositeRelationBuilder<TEnvironment>
{

    public CompositeRelationBuilder(XmlNode xmlNode, TElement readyElement)
        : base(xmlNode, readyElement) { }

    private readonly List<IRelationBuilder<TEnvironment>> builders = new();

    public void Add(IRelationBuilder<TEnvironment> builder)
    {
        if (builder != null)
            builders.Add(builder);
    }

    public override void BuildRelations(TEnvironment environment, DeserializationContext context)
        => builders.ForEach(b => b.BuildRelations(environment, context));

}
