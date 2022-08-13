using B.XmlDeserializer.Exceptions;
using B.XmlDeserializer.Relations;
using B.XmlDeserializer.Context;
using System.Xml;

namespace B.XmlDeserializer;

public abstract class ElementDeserializer<TElement, TEnvironment> : IDeserializer<TElement, TEnvironment>
{

    public abstract string ElementName { get; }

    public virtual TElement Parse(XmlNode elementNode, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent = null)
    {
        checkElementName(elementNode);
        TElement element = createElement(elementNode, context, parent);
        relationBuilder = createRelationBuilder(elementNode, element);
        return element;
    }

    protected void checkElementName(XmlNode elementNode)
    {
        if (elementNode.LocalName != ElementName)
            throw new UnexpectedElementNameException(elementNode, ElementName);
    }

    protected virtual TElement createElement(XmlNode xmlNode, DeserializationContext context, object parent) => default;

    protected virtual IRelationBuilder<TEnvironment> createRelationBuilder(XmlNode xmlNode, TElement element)
    {
        ISlaveRelationBuilder<TElement, TEnvironment> slave = createSlaveRelationBuilder();
        if (slave == null)
            return null;
        return new MasterRelationBuilder<TElement, TEnvironment>(xmlNode, element, slave);
    }

    protected virtual ISlaveRelationBuilder<TElement, TEnvironment> createSlaveRelationBuilder() => null;

}
