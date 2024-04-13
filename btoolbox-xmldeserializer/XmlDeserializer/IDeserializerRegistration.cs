using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace BToolbox.XmlDeserializer;

public interface IDeserializerRegistration
{
    string ElementName { get; }
}

public interface IDeserializerRegistrationForElement<TElement> : IDeserializerRegistration { }

public interface IDeserializerRegistration<TResult, TEnvironment> : IDeserializerRegistration
{
    void Parse(XmlNode xmlNode, TResult result, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent);
}

