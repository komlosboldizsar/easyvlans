using System.Collections;

namespace BToolbox.Model
{

    public class ObservableProxyEnumerable<TProxy, TOriginal> : IObservableEnumerable<TProxy>
    {

        private IObservableEnumerable<TOriginal> originalCollection;
        private List<TProxy> proxyList = new();

        public delegate TProxy OriginalToProxyConverterMethodDelegate(TOriginal original);
        protected OriginalToProxyConverterMethodDelegate converterMethod;

        public ObservableProxyEnumerable(IObservableEnumerable<TOriginal> originalCollection, OriginalToProxyConverterMethodDelegate converterMethod)
        {
            this.originalCollection = originalCollection;
            this.converterMethod = converterMethod;
            buildProxies();
            originalCollection.ItemsAdded += originalCollectionItemsAdded;
            originalCollection.ItemsRemoved += originalCollectionItemsRemoved;
        }

        private void buildProxies()
        {
            proxyList.Clear();
            originalCollection.Foreach(originalItem => proxyList.Add(converterMethod(originalItem)));
        }

        #region Events and handlers
        public event ObservableEnumerableItemsChangedDelegate<TProxy> ItemsAdded;
        public event ObservableEnumerableItemsChangedDelegate<TProxy> ItemsRemoved;

        private void originalCollectionItemsAdded(IEnumerable<IObservableCollection<TOriginal>.ItemWithPosition> affectedItems)
        {
            List<IObservableCollection<TProxy>.ItemWithPosition> eventData = new();
            foreach (IObservableCollection<TOriginal>.ItemWithPosition affectedItem in affectedItems)
            {
                TProxy proxy = converterMethod(affectedItem.Item);
                proxyList.Insert(affectedItem.Position, proxy);
                eventData.Add(new(proxy, affectedItem.Position));
            }
            ItemsAdded?.Invoke(eventData);
        }

        private void originalCollectionItemsRemoved(IEnumerable<IObservableCollection<TOriginal>.ItemWithPosition> affectedItems)
        {

            List<IObservableCollection<TProxy>.ItemWithPosition> eventData = new();
            foreach (IObservableCollection<TOriginal>.ItemWithPosition affectedItem in affectedItems)
            {
                TProxy removedProxy = proxyList[affectedItem.Position];
                proxyList.RemoveAt(affectedItem.Position);
                eventData.Add(new(removedProxy, affectedItem.Position));
            }
            ItemsRemoved?.Invoke(eventData);
        }
        #endregion

        #region Enumerators
        public IEnumerator<TProxy> GetEnumerator() => proxyList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)proxyList).GetEnumerator();
        #endregion


    }

}
