namespace BToolbox.XmlDeserializer;

public class ResultCollection
{

    private readonly Dictionary<IDeserializerRegistration, object> results = new();

    internal void AddForRegistration<TElement>(IDeserializerRegistrationForElement<TElement> registration, TElement result)
        => results.Add(registration, result);

    private bool getForRegistration<TElement>(IDeserializerRegistrationForElement<TElement> registration, out TElement collection)
    {
        if (!results.TryGetValue(registration, out object foundCollection))
        {
            collection = default;
            return false;
        }
        collection = (TElement)foundCollection;
        return true;
    }

    public bool GetForRegistration<TElement>(IDeserializerRegistrationForElement<TElement> registration, out TElement collection)
        => getForRegistration(registration, out collection);

    public bool GetForRegistration<TElement>(IDeserializerRegistrationForElement<TElement> registration, Action<TElement> action)
    {
        if (getForRegistration(registration, out TElement collection))
        {
            action(collection);
            return true;
        }
        return false;
    }

}

