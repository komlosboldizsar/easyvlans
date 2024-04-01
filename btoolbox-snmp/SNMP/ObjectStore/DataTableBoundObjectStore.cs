using BToolbox.Model;
using Lextm.SharpSnmpLib.Pipeline;

namespace BToolbox.SNMP
{
    public class DataTableBoundObjectStoreAdapter<TModel, TTable>
        where TModel : class
        where TTable : ObjectDataTable<TModel>, new()
    {

        private SnmpAgent _snmpAgent;
        private IEnumerable<TModel> _objectList;
        private MyObjectStore _objectStore;

        public DataTableBoundObjectStoreAdapter(SnmpAgent snmpAgent, ObservableList<TModel> objectList, MyObjectStore objectStore = null)
        {
            ctorBase(snmpAgent, objectList, objectStore);
            generateEntriesForExistingListItems();
            subscribeToList(objectList);
        }

        public DataTableBoundObjectStoreAdapter(SnmpAgent snmpAgent, IEnumerable<TModel> objectList, MyObjectStore objectStore = null)
        {
            ctorBase(snmpAgent, objectList, objectStore);
            generateEntriesForExistingListItems();
        }

        private void ctorBase(SnmpAgent snmpAgent, IEnumerable<TModel> objectList, MyObjectStore objectStore = null)
        {
            _snmpAgent = snmpAgent;
            _objectList = objectList;
            _objectStore = objectStore ?? snmpAgent.ObjectStore;
        }

        private void generateEntriesForExistingListItems()
        {
            foreach (TModel model in _objectList)
                addRow(model);
        }

        private void subscribeToList(IObservableList<TModel> objectList)
        {
            objectList.ItemsAdded += itemsAddedHandler;
            objectList.ItemsRemoved += itemsRemovedHandler;
        }

        private void addRow(TModel model)
        {
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
