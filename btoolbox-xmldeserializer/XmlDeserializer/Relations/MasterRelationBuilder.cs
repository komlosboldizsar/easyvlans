using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace BToolbox.XmlDeserializer.Relations;

internal class MasterRelationBuilder<TElement, TEnvironment> : RelationBuilder<TElement, TEnvironment>
{

    public MasterRelationBuilder(XmlNode xmlNode, TElement readyElement, ISlaveRelationBuilder<TElement, TEnvironment> slave)
        : base(xmlNode, readyElement)
        => this.slave = slave;

    private readonly ISlaveRelationBuilder<TElement, TEnvironment> slave;

    public override void BuildRelations(TEnvironment environment, DeserializationContext context)
        => slave.BuildRelations(XmlNode, ReadyElement, environment, context);

}
