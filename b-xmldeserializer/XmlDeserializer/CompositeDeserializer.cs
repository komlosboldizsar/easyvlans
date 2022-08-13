using B.XmlDeserializer.Context;
using B.XmlDeserializer.Exceptions;
using B.XmlDeserializer.Relations;
using System.Xml;

namespace B.XmlDeserializer;

public abstract class CompositeDeserializer<TResult, TEnvironment> : IDeserializer<TResult, TEnvironment>
{

    public string ElementName { get; }

    protected CompositeDeserializer(string elementName)
        => ElementName = elementName;

    private readonly Dictionary<string, IDeserializerRegistration<TResult, TEnvironment>> registrations = new();

    protected void addRegistration(string tag, IDeserializerRegistration<TResult, TEnvironment> registration)
        => registrations.Add(tag, registration);

    public TResult Parse(XmlNode parentNode, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent)
    {
        if (parentNode.LocalName != ElementName)
            throw new UnexpectedElementNameException(parentNode, ElementName);
        TResult result = createResult();
        CompositeRelationBuilder<TResult, TEnvironment> compositeRelationBuilder = new(parentNode, result);
        foreach (XmlNode childNode in parentNode.ChildNodes)
        {
            try
            {
                if (!registrations.TryGetValue(childNode.LocalName, out IDeserializerRegistration<TResult, TEnvironment> registration))
                    throw new UnexpectedElementNameException(childNode, getExpectedElementNames());
                registration.Parse(childNode, result, context, out IRelationBuilder<TEnvironment> partRelationBuilder, parent);
                compositeRelationBuilder.Add(partRelationBuilder);
            }
            catch (UnexpectedElementNameException ex)
            {
                context.ReportNotDeserializedItem(ex);
            }
        }
        relationBuilder = compositeRelationBuilder;
        return result;
    }

    protected abstract TResult createResult();

    private string[] getExpectedElementNames() => registrations.Select(r => r.Value.ElementName).ToArray();

}
