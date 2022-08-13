using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer.Relations;

public abstract class RelationBuilder<TElement, TEnvironment> : IRelationBuilder<TEnvironment>
{

    public RelationBuilder(XmlNode xmlNode, TElement readyElement)
    {
        XmlNode = xmlNode;
        ReadyElement = readyElement;
    }

    public XmlNode XmlNode { get; set; }

    public TElement ReadyElement { get; set; }

    public virtual void BuildRelations(TEnvironment environment, DeserializationContext context) { }

}
