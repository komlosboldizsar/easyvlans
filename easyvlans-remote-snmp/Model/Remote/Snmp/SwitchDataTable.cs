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
            new VariableFactory<DataProviders.CanReadVlanConfig>(INDEX_CanReadVlanConfig),
            new VariableFactory<DataProviders.DoReadVlanConfig>(INDEX_DoReadVlanConfig),
            new VariableFactory<DataProviders.ReadVlanConfigStatus>(INDEX_ReadVlanConfigStatus),
            new VariableFactory<DataProviders.CanPersistChanges>(INDEX_CanPersistChanges),
            new VariableFactory<DataProviders.DoPersistChanges>(INDEX_DoPersistChanges),
            new VariableFactory<DataProviders.PersistConfigStatus>(INDEX_PersistChangesStatus),
        };

        public const int INDEX_Id = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_PortsWithPendingChangeCount = 3;
        public const int INDEX_CanReadVlanConfig = 11;
        public const int INDEX_DoReadVlanConfig = 12;
        public const int INDEX_ReadVlanConfigStatus = 13;
        public const int INDEX_CanPersistChanges = 21;
        public const int INDEX_DoPersistChanges = 22;
        public const int INDEX_PersistChangesStatus = 23;

        protected override string TableOid => $"{SnmpAgent.OID_BASE}.1";

        private class DataProviders
        {

            // .1
            public class Id : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Item.ID);
            }

            // .2
            public class Label : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Item.Label);
            }

            // .3
            public class PortsWithPendingChangeCount : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Item.PortsWithPendingChangeCount);
            }

            // .11
            public class CanReadVlanConfig : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Item.OperationMethodCollection.ReadConfigMethod != null);
            }

            // .12
            public class DoReadVlanConfig : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(0);
                public override async void Set(ISnmpData data)
                {
                    static void throwInvalidInputException(ErrorCode errorCode)
                        => throw new SnmpErrorCodeException(errorCode, $"Value must be a TruthValue ({TruthValue.VALUE_TRUE} or {TruthValue.VALUE_FALSE}) and set to 1 ({TruthValue.VALUE_TRUE}) to read VLAN configuration of the switch.");
                    if (data is not Integer32 intData)
                        throwInvalidInputException(ErrorCode.WrongType);
                    int value = intData.ToInt32();
                    if ((value != TruthValue.VALUE_TRUE) && (value != TruthValue.VALUE_FALSE))
                        throwInvalidInputException(ErrorCode.WrongValue);
                    if (value != TruthValue.VALUE_TRUE)
                        return;
                    if (Item.OperationMethodCollection.ReadConfigMethod == null)
                        throw new SnmpErrorCodeException(ErrorCode.ResourceUnavailable, "No method provided for reading VLAN configuration of the switch.");
                    await Item.ReadConfigAsync();
                }
            }

            // .13
            public class ReadVlanConfigStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Item.ReadVlanConfigStatus);
            }

            // .21
            public class CanPersistChanges : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Item.OperationMethodCollection.PersistChangesMethod != null);
            }

            // .22
            public class DoPersistChanges : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(0);
                public override async void Set(ISnmpData data)
                {
                    static void throwInvalidInputException(ErrorCode errorCode)
                        => throw new SnmpErrorCodeException(errorCode, $"Value must be a TruthValue ({TruthValue.VALUE_TRUE} or {TruthValue.VALUE_FALSE}) and set to 1 ({TruthValue.VALUE_TRUE}) to persist changes of the configuration of the switch.");
                    if (data is not Integer32 intData)
                        throwInvalidInputException(ErrorCode.WrongType);
                    int value = intData.ToInt32();
                    if ((value != TruthValue.VALUE_TRUE) && (value != TruthValue.VALUE_FALSE))
                        throwInvalidInputException(ErrorCode.WrongValue);
                    if (value != TruthValue.VALUE_TRUE)
                        return;
                    if (Item.OperationMethodCollection.PersistChangesMethod == null)
                        throw new SnmpErrorCodeException(ErrorCode.ResourceUnavailable, "No method provided for persisting changes of the configuration of the switch.");
                    if (!await Item.PersistChangesAsync())
                        throw new SnmpErrorCodeException(ErrorCode.GenError, "Persisting changes of the configuration of the switch not successful.");
                }
            }

            // .23
            public class PersistConfigStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Item.PersistVlanConfigStatus);
            }

        }

    }
}
