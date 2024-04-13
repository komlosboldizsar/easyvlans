using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Exceptions;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace BToolbox.XmlDeserializer;

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
