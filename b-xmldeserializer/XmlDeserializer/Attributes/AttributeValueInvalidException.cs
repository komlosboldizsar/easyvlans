using B.XmlDeserializer.Exceptions;
using System.Xml;

namespace B.XmlDeserializer.Attributes;

public class AttributeValueInvalidException : DeserializationException
{

    public AttributeValueInvalidException(string message, XmlNode xmlNode)
        : base($"Attribute value invalid: [{message}]", xmlNode) { }

    public AttributeValueInvalidException(string message)
        : base($"Attribute value invalid: [{message}]", (XmlNode)null) { }

}
