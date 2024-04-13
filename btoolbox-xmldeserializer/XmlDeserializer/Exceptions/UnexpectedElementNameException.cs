using System.Xml;

namespace BToolbox.XmlDeserializer.Exceptions;

public class UnexpectedElementNameException : DeserializationException
{

    public string[] ExpectedElementNames { get; }

    public UnexpectedElementNameException(XmlNode invalidNode, params string[] expectedElementNames)
        : base($"Unexpected element with name [{invalidNode.LocalName}], expected: [{string.Join(';', expectedElementNames)}]", invalidNode)
        => ExpectedElementNames = expectedElementNames;

}
