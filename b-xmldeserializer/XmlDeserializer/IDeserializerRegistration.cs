using B.XmlDeserializer.Context;
using B.XmlDeserializer.Relations;
using System.Xml;

namespace B.XmlDeserializer;

public interface IDeserializerRegistration
{
    string ElementName { get; }
}

public interface IDeserializerRegistrationForElement<TElement> : IDeserializerRegistration { }

public interface IDeserializerRegistration<TResult, TEnvironment> : IDeserializerRegistration
{
    void Parse(XmlNode xmlNode, TResult result, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent);
}

