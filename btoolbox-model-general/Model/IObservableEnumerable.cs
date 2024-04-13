using System.Collections;

namespace BToolbox.Model
{

    public delegate void ObservableEnumerableItemsChangedDelegate<T>(IEnumerable<IObservableEnumerable<T>.ItemWithPosition> affectedItemsWithPositions);

    public interface IObservableEnumerable<T> : IEnumerable, IEnumerable<T>
    {
        event ObservableEnumerableItemsChangedDelegate<T> ItemsAdded;
        event ObservableEnumerableItemsChangedDelegate<T> ItemsRemoved;
        public record ItemWithPosition(T Item, int Position);
    }

}
