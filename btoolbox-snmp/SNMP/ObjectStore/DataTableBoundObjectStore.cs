using BToolbox.Model;

namespace BToolbox.SNMP
{
    public class DataTableBoundObjectStoreAdapter<TModel, TTable>
        where TModel : class
        where TTable : ObjectDataTable<TModel>, new()
    {

        private SnmpAgent _snmpAgent;
        private IEnumerable<TModel> _objectList;
        private Predicate<TModel> _filter;
        private MyObjectStore _objectStore;

        public DataTableBoundObjectStoreAdapter(SnmpAgent snmpAgent, ObservableList<TModel> objectList, Predicate<TModel> filter = null, MyObjectStore objectStore = null)
        {
            ctorBase(snmpAgent, objectList, filter, objectStore);
            generateEntriesForExistingListItems();
            subscribeToList(objectList);
        }

        public DataTableBoundObjectStoreAdapter(SnmpAgent snmpAgent, IEnumerable<TModel> objectList, Predicate<TModel> filter = null, MyObjectStore objectStore = null)
        {
            ctorBase(snmpAgent, objectList, filter, objectStore);
            generateEntriesForExistingListItems();
        }

        private void ctorBase(SnmpAgent snmpAgent, IEnumerable<TModel> objectList, Predicate<TModel> filter = null, MyObjectStore objectStore = null)
        {
            _snmpAgent = snmpAgent;
            _objectList = objectList;
            _filter = filter;
            _objectStore = objectStore ?? snmpAgent.ObjectStore;
        }

        private void generateEntriesForExistingListItems()
        {
            foreach (TModel model in _objectList)
                if ((_filter == null) || _filter(model))
                    addRow(model);
        }

        private void subscribeToList(IObservableList<TModel> objectList)
        {
            objectList.ItemsAdded += itemsAddedHandler;
            objectList.ItemsRemoved += itemsRemovedHandler;
        }

        private void addRow(TModel model)
        {
            if ((_filter != null) && !_filter(model))
                return;
            TTable newTableObject = new();
            newTableObject.Init(model, _snmpAgent);
            _objectStore.Add(newTableObject);
            rowTableAssociations.Add(model, newTableObject);
        }

        Dictionary<TModel, TTable> rowTableAssociations = new();

        private void itemsAddedHandler(IEnumerable<IObservableEnumerable<TModel>.ItemWithPosition> affectedItemsWithPositions)
            => affectedItemsWithPositions.Foreach(aiwp => addRow(aiwp.Item));

        private void itemsRemovedHandler(IEnumerable<IObservableEnumerable<TModel>.ItemWithPosition> affectedItemsWithPositions)
        {
            foreach (IObservableEnumerable<TModel>.ItemWithPosition riwp in affectedItemsWithPositions)
            {
                if (rowTableAssociations.TryGetValue(riwp.Item, out TTable tableToRemove))
                {
                    _objectStore.Remove(tableToRemove);
                    tableToRemove.End();
                }
            }
        }

    }
}
