using System.Xml;

namespace BToolbox.XmlDeserializer.Attributes;

public record XmlAttributeData<TValue>(XmlAttribute Attribute, TValue Value);
