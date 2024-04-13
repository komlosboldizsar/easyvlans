using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Exceptions;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace BToolbox.XmlDeserializer;

public abstract class CollectionDeserializerBase<TCollection, TElement, TEnvironment> : IDeserializer<TCollection, TEnvironment>
{

    public abstract string ElementName { get; }

    protected TCollection parse(XmlNode collectionNode, DeserializationContext context, out IRelationBuilder<TEnvironment> collectionRelationBuilder, object parent, bool forwardParent)
    {
        if (collectionNode.LocalName != ElementName)
            throw new UnexpectedElementNameException(collectionNode, ElementName);
        TCollection collection = createCollection(collectionNode, context, parent);
        ICompositeRelationBuilder<TEnvironment> compositeRelationBuilder = createRelationBuilder(collectionNode, collection);
        object parentOfElements = forwardParent ? parent : collection;
        foreach (XmlNode elementNode in collectionNode.ChildNodes)
        {
            try
            {
                TElement element = parseChildNode(elementNode, context, out IRelationBuilder<TEnvironment> elementRelationBuilder, parentOfElements);
                addElementToCollection(collection, element);
                if (elementRelationBuilder != null)
                    compositeRelationBuilder.Add(elementRelationBuilder);
            }
            catch (DeserializationException ex)
            {
                context.ReportNotDeserializedItem(ex, typeof(TElement));
            }
        }
        collectionRelationBuilder = compositeRelationBuilder;
        return collection;
    }

    public TCollection Parse(XmlNode collectionNode, DeserializationContext context, out IRelationBuilder<TEnvironment> collectionRelationBuilder, object parent = null)
        => parse(collectionNode, context, out collectionRelationBuilder, parent, false);

    public TCollection ParseWithGivenParent(XmlNode collectionNode, DeserializationContext context, out IRelationBuilder<TEnvironment> collectionRelationBuilder, object parent = null)
        => parse(collectionNode, context, out collectionRelationBuilder, parent, true);

    protected abstract TCollection createCollection(XmlNode xmlNode, DeserializationContext context, object parent);

    protected virtual ICompositeRelationBuilder<TEnvironment> createRelationBuilder(XmlNode xmlNode, TCollection readyCollection)
        => new MasterCompositeRelationBuilder<TCollection, TEnvironment>(xmlNode, readyCollection, createSlaveRelationBuilder());

    protected virtual ISlaveRelationBuilder<TCollection, TEnvironment> createSlaveRelationBuilder()
        => null;

    protected abstract TElement parseChildNode(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent);

    protected abstract void addElementToCollection(TCollection collection, TElement element);

}
