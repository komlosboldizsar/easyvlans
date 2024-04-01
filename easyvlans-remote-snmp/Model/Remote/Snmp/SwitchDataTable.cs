using BToolbox.SNMP;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.Remote.Snmp
{
    internal class SwitchDataTable : ObjectDataTable<Switch>
    {

        protected override IVariableFactory[] VariableFactories => new IVariableFactory[]
        {
            VARFACT_Index,
            VARFACT_Label,
            VARFACT_PortsWithPendingChangeCount,
            VARFACT_CanReadVlanConfig,
            VARFACT_DoReadVlanConfig,
            VARFACT_ReadVlanConfigStatus,
            VARFACT_CanPersistChanges,
            VARFACT_DoPersistChanges,
            VARFACT_PersistChangesStatus
        };

        protected override IVariableFactory IndexerVariableFactory => VARFACT_Index;

        public const int INDEX_Id = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_PortsWithPendingChangeCount = 3;
        public const int INDEX_CanReadVlanConfig = 11;
        public const int INDEX_DoReadVlanConfig = 12;
        public const int INDEX_ReadVlanConfigStatus = 13;
        public const int INDEX_CanPersistChanges = 21;
        public const int INDEX_DoPersistChanges = 22;
        public const int INDEX_PersistChangesStatus = 23;

        public static readonly IVariableFactory VARFACT_Index = new VariableFactory<DataProviders.Id>(INDEX_Id);
        public static readonly IVariableFactory VARFACT_Label = new VariableFactory<DataProviders.Label>(INDEX_Label);
        public static readonly IVariableFactory VARFACT_PortsWithPendingChangeCount = new VariableFactory<DataProviders.PortsWithPendingChangeCount>(INDEX_PortsWithPendingChangeCount);
        public static readonly IVariableFactory VARFACT_CanReadVlanConfig = new VariableFactory<DataProviders.CanReadVlanConfig>(INDEX_CanReadVlanConfig);
        public static readonly IVariableFactory VARFACT_DoReadVlanConfig = new VariableFactory<DataProviders.DoReadVlanConfig>(INDEX_DoReadVlanConfig);
        public static readonly IVariableFactory VARFACT_ReadVlanConfigStatus = new VariableFactory<DataProviders.ReadVlanConfigStatus>(INDEX_ReadVlanConfigStatus);
        public static readonly IVariableFactory VARFACT_CanPersistChanges = new VariableFactory<DataProviders.CanPersistChanges>(INDEX_CanPersistChanges);
        public static readonly IVariableFactory VARFACT_DoPersistChanges = new VariableFactory<DataProviders.DoPersistChanges>(INDEX_DoPersistChanges);
        public static readonly IVariableFactory VARFACT_PersistChangesStatus = new VariableFactory<DataProviders.PersistConfigStatus>(INDEX_PersistChangesStatus);


        protected override ITrapGeneratorFactory[] TrapGeneratorFactories => Array.Empty<ITrapGeneratorFactory>();

        protected override string TableOid => $"{SnmpAgent.OID_BASE}.1";
        protected override int ItemIndex => (int)Model.RemoteIndex;

        private class DataProviders
        {

            // .1
            public class Id : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Model.ID);
            }

            // .2
            public class Label : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Model.Label);
            }

            // .3
            public class PortsWithPendingChangeCount : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Model.PortsWithPendingChangeCount);
            }

            // .11
            public class CanReadVlanConfig : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.OperationMethodCollection.ReadConfigMethod != null);
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
                    if (Model.OperationMethodCollection.ReadConfigMethod == null)
                        throw new SnmpErrorCodeException(ErrorCode.ResourceUnavailable, "No method provided for reading VLAN configuration of the switch.");
                    await Model.ReadConfigAsync();
                }
            }

            // .13
            public class ReadVlanConfigStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Model.ReadVlanConfigStatus);
            }

            // .21
            public class CanPersistChanges : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.OperationMethodCollection.PersistChangesMethod != null);
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
                    if (Model.OperationMethodCollection.PersistChangesMethod == null)
                        throw new SnmpErrorCodeException(ErrorCode.ResourceUnavailable, "No method provided for persisting changes of the configuration of the switch.");
                    if (!await Model.PersistChangesAsync())
                        throw new SnmpErrorCodeException(ErrorCode.GenError, "Persisting changes of the configuration of the switch not successful.");
                }
            }

            // .23
            public class PersistConfigStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Model.PersistVlanConfigStatus);
            }

        }

    }
}
