using System.Collections;

namespace BToolbox.Model
{

    public class ObservableList<T> : IObservableList<T>
    {

        private List<T> underlying = new();

        public IEnumerator<T> GetEnumerator() => underlying.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IList<T>)underlying).GetEnumerator();

        public T this[int index]
        {
            get => underlying[index];
            set => underlying[index] = value;
        }

        public int Count => underlying.Count;
        public bool IsReadOnly => ((IList<T>)underlying).IsReadOnly;

        public event ObservableEnumerableItemsChangedDelegate<T> ItemsAdded;
        public event ObservableEnumerableItemsChangedDelegate<T> ItemsRemoved;

        public void Add(T item)
        {
            underlying.Add(item);
            ItemsAdded?.Invoke(new IObservableEnumerable<T>.ItemWithPosition[] { new(item, underlying.Count - 1) });
        }

        public void AddRange(IEnumerable<T> items)
        {
            int startIndex = underlying.Count;
            underlying.AddRange(items);
            ItemsAdded?.Invoke(items.Select((item, index) => new IObservableEnumerable<T>.ItemWithPosition(item, startIndex + index)));
        }

        public void Clear()
        {
            int count = underlying.Count;
            IEnumerable<IObservableEnumerable<T>.ItemWithPosition> removedItems = null;
            if (ItemsRemoved != null)
                removedItems = underlying.Select((item, index) => new IObservableEnumerable<T>.ItemWithPosition(item, index));
            underlying.Clear();
            if (count > 0)
                ItemsRemoved?.Invoke(removedItems);
        }

        public void Insert(int index, T item)
        {
            underlying.Insert(index, item);
            ItemsAdded?.Invoke(new IObservableEnumerable<T>.ItemWithPosition[] { new(item, index) });
        }

        public bool Remove(T item)
        {
            int index = -1;
            if (ItemsRemoved != null)
                index = underlying.IndexOf(item);
            if (underlying.Remove(item))
            {
                ItemsRemoved?.Invoke(new IObservableEnumerable<T>.ItemWithPosition[] { new(item, index) });
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            T removedItem = default(T);
            if (ItemsRemoved != null)
                removedItem = underlying[index];
            underlying.RemoveAt(index);
            ItemsRemoved?.Invoke(new IObservableEnumerable<T>.ItemWithPosition[] { new(removedItem, index) });
        }

        public bool Contains(T item) => underlying.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => ((IList<T>)underlying).CopyTo(array, arrayIndex);
        public int IndexOf(T item) => underlying.IndexOf(item);

    }

}
