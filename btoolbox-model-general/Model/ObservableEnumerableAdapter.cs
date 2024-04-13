using System.Collections;

namespace BToolbox.Model
{

    public abstract class ObservableEnumerableAdapter<TThis, TAdaptee> : IObservableEnumerable<TThis>
    {

        protected IObservableEnumerable<TAdaptee> Adaptee { get; init; }

        private ObservableEnumerableItemsChangedDelegate<TThis> _itemsAdded;
        public event ObservableEnumerableItemsChangedDelegate<TThis> ItemsAdded
        {
            add
            {
                if (_itemsAdded == null)
                    Adaptee.ItemsAdded += registeredItemAdded;
                _itemsAdded += value;
            }
            remove
            {
                _itemsAdded -= value;
                if (_itemsAdded == null)
                    Adaptee.ItemsAdded -= registeredItemAdded;
            }
        }

        private void registeredItemAdded(IEnumerable<IObservableEnumerable<TAdaptee>.ItemWithPosition> affectedItemsWithPositions)
            => _itemsAdded?.Invoke(convertAffectedItemsWithPositionsEnumerable(affectedItemsWithPositions));

        private ObservableEnumerableItemsChangedDelegate<TThis> _itemsRemoved;
        public event ObservableEnumerableItemsChangedDelegate<TThis> ItemsRemoved
        {
            add
            {
                if (_itemsRemoved == null)
                    Adaptee.ItemsRemoved += registeredItemRemoved;
                _itemsRemoved += value;
            }
            remove
            {
                _itemsRemoved -= value;
                if (_itemsRemoved == null)
                    Adaptee.ItemsRemoved -= registeredItemRemoved;
            }
        }

        private void registeredItemRemoved(IEnumerable<IObservableEnumerable<TAdaptee>.ItemWithPosition> affectedItemsWithPositions)
            => _itemsRemoved?.Invoke(convertAffectedItemsWithPositionsEnumerable(affectedItemsWithPositions));

        protected abstract TThis convertAdaptee(TAdaptee adaptee);

        private IEnumerable<IObservableEnumerable<TThis>.ItemWithPosition> convertAffectedItemsWithPositionsEnumerable(IEnumerable<IObservableEnumerable<TAdaptee>.ItemWithPosition> affectedItemsWithPositions) =>
            affectedItemsWithPositions.Select(aiwp => new IObservableEnumerable<TThis>.ItemWithPosition(convertAdaptee(aiwp.Item), aiwp.Position));


        public IEnumerator<TThis> GetEnumerator() => getEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => getEnumerator();
        protected abstract IEnumerator<TThis> getEnumerator();

    }

}
