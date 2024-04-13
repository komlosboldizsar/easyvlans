using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace BToolbox.Model
{
    public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>
    {

        private Dictionary<TKey, TValue> underlying = new();

        public TValue this[TKey key]
        {
            get => underlying[key];
            set => underlying[key] = value;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => underlying.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)underlying).GetEnumerator();

        public ICollection<TKey> Keys => new KeyCollection(this, underlying.Keys);
        public ICollection<TValue> Values => new ValueCollection(this, underlying.Values);

        public int Count => underlying.Count;
        public bool IsReadOnly => ((IDictionary<TKey, TValue>)underlying).IsReadOnly;

        public event ObservableEnumerableItemsChangedDelegate<KeyValuePair<TKey, TValue>> ItemsAdded;
        public event ObservableEnumerableItemsChangedDelegate<KeyValuePair<TKey, TValue>> ItemsRemoved;

        public void Add(TKey key, TValue value)
        {
            underlying.Add(key, value);
            KeyValuePair<TKey, TValue> keyValuePair = KeyValuePair.Create(key, value);
            ItemsAdded?.Invoke(new IObservableCollection<KeyValuePair<TKey, TValue>>.ItemWithPosition[] { new(keyValuePair, this.GetIndexOf(keyValuePair)) });
        }

        public void Add(KeyValuePair<TKey, TValue> keyValuePair) => Add(keyValuePair.Key, keyValuePair.Value);

        public bool ChangeKey(TKey oldKey, TKey newKey)
        {
            if (EqualityComparer<TKey>.Default.Equals(oldKey, newKey))
                return true;
            if (!underlying.TryGetValue(newKey, out TValue value))
                return false;
            if (underlying.ContainsKey(newKey))
                return false;
            underlying.Remove(oldKey);
            underlying.Add(newKey, value);
            return true;
        }

        public bool ChangeKeyOfItem(TValue item, TKey newKey)
            => ChangeKey(underlying.FirstOrDefault(kvp => EqualityComparer<TValue>.Default.Equals(kvp.Value, item)).Key, newKey);

        public void Clear()
        {
            int count = underlying.Count;
            IEnumerable<IObservableEnumerable<KeyValuePair<TKey, TValue>>.ItemWithPosition> removedItems = underlying.Select((kvp, i) => new IObservableEnumerable<KeyValuePair<TKey, TValue>>.ItemWithPosition(kvp, i));
            underlying.Clear();
            if (count > 0)
                ItemsRemoved?.Invoke(removedItems);
        }

        public bool ContainsValue(TValue value) => Values.Contains(value);

        public bool Remove(TKey key)
        {
            int position = -1;
            if (ItemsRemoved != null)
                position = Keys.GetIndexOf(key);
            if (underlying.Remove(key, out TValue removedItem))
            {
                ItemsRemoved?.Invoke(new IObservableEnumerable<KeyValuePair<TKey, TValue>>.ItemWithPosition[] { new(KeyValuePair.Create(key, removedItem), position) });
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int position = -1;
            if (ItemsRemoved != null)
                position = this.GetIndexOf(keyValuePair);
            if (((IDictionary<TKey, TValue>)underlying).Remove(keyValuePair))
            {
                ItemsRemoved?.Invoke(new IObservableEnumerable<KeyValuePair<TKey, TValue>>.ItemWithPosition[] { new(keyValuePair, position) });
                return true;
            }
            return false;
        }

        public bool Contains(KeyValuePair<TKey, TValue> keyValuePair) => ((IDictionary<TKey, TValue>)underlying).Contains(keyValuePair);
        public bool ContainsKey(TKey key) => underlying.ContainsKey(key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)underlying).CopyTo(array, arrayIndex);
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => underlying.TryGetValue(key, out value);

        private abstract class KeyValueCollection<T> : ObservableEnumerableAdapter<T, KeyValuePair<TKey, TValue>>, IObservableCollection<T>
        {

            private ICollection<T> underlying;

            public KeyValueCollection(ObservableDictionary<TKey, TValue> dictionary, ICollection<T> underlying)
            {
                Adaptee = dictionary;
                this.underlying = underlying;
            }

            protected override IEnumerator<T> getEnumerator() => underlying.GetEnumerator();

            public int Count => underlying.Count;
            public bool IsReadOnly => underlying.IsReadOnly;
            public void Add(T item) => underlying.Add(item);
            public void Clear() => underlying.Clear();
            public bool Contains(T item) => underlying.Contains(item);
            public void CopyTo(T[] array, int arrayIndex) => underlying.CopyTo(array, arrayIndex);
            public bool Remove(T item) => underlying.Remove(item);

        }

        private class KeyCollection : KeyValueCollection<TKey>
        {
            public KeyCollection(ObservableDictionary<TKey, TValue> dictionary, ICollection<TKey> underlying) : base(dictionary, underlying) { }
            protected override TKey convertAdaptee(KeyValuePair<TKey, TValue> adaptee) => adaptee.Key;
        }

        private class ValueCollection : KeyValueCollection<TValue>
        {
            public ValueCollection(ObservableDictionary<TKey, TValue> dictionary, ICollection<TValue> underlying) : base(dictionary, underlying) { }
            protected override TValue convertAdaptee(KeyValuePair<TKey, TValue> adaptee) => adaptee.Value;
        }

    }

}
