namespace BToolbox.Model
{
    public interface IObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IObservableCollection<KeyValuePair<TKey, TValue>>
    { }
}
