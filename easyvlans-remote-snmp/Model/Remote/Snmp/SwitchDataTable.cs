using Lextm.SharpSnmpLib;

namespace easyvlans.Model.Remote.Snmp
{
    internal class SwitchDataTable : DataTable<Switch>
    {

        public SwitchDataTable(Switch @switch) : base(@switch) { }

        protected override IVariableFactory[] VariableFactories => new IVariableFactory[]
        {
            new VariableFactory<DataProviders.Id>(INDEX_Id),
            new VariableFactory<DataProviders.Label>(INDEX_Label),
            new VariableFactory<DataProviders.PortsWithPendingChangeCount>(INDEX_PortsWithPendingChangeCount),
            new VariableFactory<DataProviders.ReadVlanConfigStatus>(INDEX_ReadVlanConfigStatus),
            new VariableFactory<DataProviders.PersistVlanConfigStatus>(INDEX_PersistVlanConfigStatus),
            new VariableFactory<DataProviders.DoPersistChanges>(INDEX_DoPersistChanges)
        };

        public const int INDEX_Id = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_PortsWithPendingChangeCount = 3;
        public const int INDEX_ReadVlanConfigStatus = 4;
        public const int INDEX_PersistVlanConfigStatus = 5;
        public const int INDEX_DoPersistChanges = 6;

        protected override string TableOid => $"{SnmpAgent.OID_BASE}.1";

        private class DataProviders
        {

            public class Id : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Item.ID);
            }

            public class Label : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Item.Label);
            }

            public class PortsWithPendingChangeCount : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Item.PortsWithPendingChangeCount);
            }

            public class ReadVlanConfigStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Item.ReadVlanConfigStatus);
            }

            public class PersistVlanConfigStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Item.PersistVlanConfigStatus);
            }

            public class DoPersistChanges : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(0);
                public override async void Set(ISnmpData data)
                {
                    if (data is Integer32 intData)
                    {
                        int value = intData.ToInt32();
                        if ((value != 1) && (value != 2))
                            throw new ArgumentOutOfRangeException(nameof(data), "Value must be a TruthValue (1 or 2) and set to 1 (true) to persist changes of the configuration of the switch.");
                        if (value != 1)
                            return;
                        if (!await Item.PersistChangesAsync())
                            throw new OperationException("Persisting changes of the configuration of the switch not successful.");
                    }
                }
            }

        }

    }
}
