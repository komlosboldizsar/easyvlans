namespace BToolbox.XmlDeserializer;

public class CollectingCompositeDeserializer<TEnvironment> : CompositeDeserializer<ResultCollection, TEnvironment>
{

    public CollectingCompositeDeserializer(string elementName)
        : base(elementName) { }

    protected override ResultCollection createResult()
        => new();

    public class Registration<TElement> : DeserializerRegistrationBase<ResultCollection, TElement, TEnvironment>
    {
        public Registration(IDeserializer<TElement, TEnvironment> deserializer) : base(deserializer) { }
        protected override void handleResult(ResultCollection resultCollection, TElement element) => resultCollection.AddForRegistration(this, element);
    }

    public Registration<TElement> Register<TElement>(IDeserializer<TElement, TEnvironment> deserializer)
    {
        Registration<TElement> registration = new(deserializer);
        addRegistration(deserializer.ElementName, registration);
        return registration;
    }

}
