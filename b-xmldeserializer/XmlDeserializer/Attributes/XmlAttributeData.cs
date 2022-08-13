using System.Xml;

namespace B.XmlDeserializer.Attributes;

public record XmlAttributeData<TValue>(XmlAttribute Attribute, TValue Value);
