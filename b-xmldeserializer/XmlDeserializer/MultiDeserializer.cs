using B.XmlDeserializer.Context;
using B.XmlDeserializer.Exceptions;
using B.XmlDeserializer.Relations;
using System.Xml;

namespace B.XmlDeserializer;

public class MultiDeserializer<TEnvironment> : IDeserializer<ResultCollection, TEnvironment>
{

    public class Registration<TElement> : DeserializerRegistrationBase<ResultCollection, TElement, TEnvironment>
    {
        public Registration(IDeserializer<TElement, TEnvironment> deserializer) : base(deserializer) { }
        protected override void handleResult(ResultCollection resultCollection, TElement element) => resultCollection.AddForRegistration(this, element);
    }

    private readonly List<IDeserializerRegistration<ResultCollection, TEnvironment>> registrations = new();

    public Registration<TElement> Register<TElement>(IDeserializer<TElement, TEnvironment> deserializer)
    {
        if (ElementName == null)
            ElementName = deserializer.ElementName;
        else if (ElementName != deserializer.ElementName)
            throw new DeserializerBuildingException($"Invalid registration of sub-deserializer for multi-deserializer. Sub-deserializers must accept the same element name, currently [{ElementName}].");
        Registration<TElement> registration = new(deserializer);
        registrations.Add(registration);
        return registration;
    }

    public string ElementName { get; private set; }

    public ResultCollection Parse(XmlNode parentNode, DeserializationContext context, out IRelationBuilder<TEnvironment> relationBuilder, object parent)
    {
        if (parentNode.LocalName != ElementName)
            throw new UnexpectedElementNameException(parentNode, ElementName);
        InnerContext innerContext = new(context);
        ResultCollection resultCollection = new();
        CompositeRelationBuilder<ResultCollection, TEnvironment> compositeRelationBuilder = new(parentNode, resultCollection);
        foreach (IDeserializerRegistration<ResultCollection, TEnvironment> registration in registrations)
        {
            registration.Parse(parentNode, resultCollection, innerContext, out IRelationBuilder<TEnvironment> partRelationBuilder, parent);
            compositeRelationBuilder.Add(partRelationBuilder);
        }
        relationBuilder = compositeRelationBuilder;
        innerContext.ReportUnexpectedNames(parentNode.ChildNodes, registrations.Count);
        return resultCollection;
    }

    private class InnerContext : DeserializationContext
    {

        private readonly DeserializationContext outerContext;

        public InnerContext(DeserializationContext outerContext) => this.outerContext = outerContext;

        private readonly Dictionary<XmlNode, int> notDeserializedReportCounts = new();

        public override void Report(IDeserializationReport report)
        {
            if (report is NotDeserializedItemReport notDeserializedItemReport)
            {
                if (!notDeserializedReportCounts.TryGetValue(notDeserializedItemReport.XmlNode, out int notDeserializedReportCount))
                    notDeserializedReportCount = 0;
                notDeserializedReportCounts[notDeserializedItemReport.XmlNode] = ++notDeserializedReportCount;
            }
            else
            {
                outerContext.Report(report);
            }
        }

        public void ReportUnexpectedNames(XmlNodeList allChildNodes, int registeredDeserializersCount)
        {
            foreach (XmlNode node in allChildNodes)
                if (notDeserializedReportCounts.TryGetValue(node, out int notDeserializedReportCount) && notDeserializedReportCount == registeredDeserializersCount)
                    outerContext.ReportNotDeserializedItem(node, "Not recognized by any sub-deserializers of multi-deserializer.");
        }

    }

}
