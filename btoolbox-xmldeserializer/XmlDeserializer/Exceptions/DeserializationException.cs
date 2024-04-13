using System.Xml;

namespace BToolbox.XmlDeserializer.Exceptions;

public class DeserializationException : Exception
{

    public XmlNode XmlNode { get; init; }

    public DeserializationException() { }

    public DeserializationException(string message)
        : base(message) { }

    public DeserializationException(string message, Exception innerException)
        : base(message, innerException) { }

    public DeserializationException(string message, XmlNode xmlNode)
        : base(message) => XmlNode = xmlNode;

    public DeserializationException(string message, Exception innerException, XmlNode xmlNode)
        : base(message, innerException) => XmlNode = xmlNode;

}
