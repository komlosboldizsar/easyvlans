using B.XmlDeserializer.Attributes;
using System.Xml;

namespace B.XmlDeserializer.Exceptions;

public class RelatedObjectNotFoundException : DeserializationException
{

    public Type RelatedObjectType { get; }

    public RelatedObjectNotFoundException(XmlNode ownerNode, string relatedObjectId, Type relatedObjectType)
        : base(formatMessage(relatedObjectId, relatedObjectType), ownerNode)
        => RelatedObjectType = relatedObjectType;

    public RelatedObjectNotFoundException(string relatedObjectId, Type relatedObjectType)
        : base(formatMessage(relatedObjectId, relatedObjectType), (XmlNode)null)
        => RelatedObjectType = relatedObjectType;

    private static string formatMessage(string relatedObjectId, Type relatedObjectType)
        => $"Related element [TYPE:{relatedObjectType}] with ID [{relatedObjectId}] not found.";

    public static RelatedObjectNotFoundException Throw<TAttributeValue>(XmlAttributeData<TAttributeValue> attributeData, Type relatedObjectType)
        => throw new RelatedObjectNotFoundException(attributeData.Attribute, attributeData.Value?.ToString(), relatedObjectType);

}
