using BToolbox.SNMP;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.Remote.Snmp
{
    internal class SwitchDataTable : ObjectDataTable<Switch>
    {

        protected override IVariableFactory[] VariableFactories => new IVariableFactory[]
        {
            VARFACT_RemoteIndex,
            VARFACT_Id,
            VARFACT_Label,
            VARFACT_PortsWithPendingChangeCount,
            VARFACT_CanReadVlanConfig,
            VARFACT_DoReadVlanConfig,
            VARFACT_ReadVlanConfigStatus,
            VARFACT_CanPersistChanges,
            VARFACT_DoPersistChanges,
            VARFACT_PersistChangesStatus
        };

        protected override IVariableFactory IndexerVariableFactory => VARFACT_RemoteIndex;

        public const int INDEX_RemoteIndex = 0;
        public const int INDEX_Id = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_PortsWithPendingChangeCount = 3;
        public const int INDEX_CanReadVlanConfig = 11;
        public const int INDEX_DoReadVlanConfig = 12;
        public const int INDEX_ReadVlanConfigStatus = 13;
        public const int INDEX_ReadVlanConfigStatusUpdateTime = 14;
        public const int INDEX_CanPersistChanges = 21;
        public const int INDEX_DoPersistChanges = 22;
        public const int INDEX_PersistChangesStatus = 23;

        public static readonly IVariableFactory VARFACT_RemoteIndex = new VariableFactory<DataProviders.RemoteIndex>(INDEX_RemoteIndex);
        public static readonly IVariableFactory VARFACT_Id = new VariableFactory<DataProviders.Id>(INDEX_Id);
        public static readonly IVariableFactory VARFACT_Label = new VariableFactory<DataProviders.Label>(INDEX_Label);
        public static readonly IVariableFactory VARFACT_PortsWithPendingChangeCount = new VariableFactory<DataProviders.PortsWithPendingChangeCount>(INDEX_PortsWithPendingChangeCount);
        public static readonly IVariableFactory VARFACT_CanReadVlanConfig = new VariableFactory<DataProviders.CanReadVlanConfig>(INDEX_CanReadVlanConfig);
        public static readonly IVariableFactory VARFACT_DoReadVlanConfig = new VariableFactory<DataProviders.DoReadVlanConfig>(INDEX_DoReadVlanConfig);
        public static readonly IVariableFactory VARFACT_ReadVlanConfigStatus = new VariableFactory<DataProviders.ReadVlanConfigStatus>(INDEX_ReadVlanConfigStatus);
        public static readonly IVariableFactory VARFACT_ReadVlanConfigStatusUpdateTime = new VariableFactory<DataProviders.ReadVlanConfigStatusUpdateTime>(INDEX_ReadVlanConfigStatusUpdateTime);
        public static readonly IVariableFactory VARFACT_CanPersistChanges = new VariableFactory<DataProviders.CanPersistChanges>(INDEX_CanPersistChanges);
        public static readonly IVariableFactory VARFACT_DoPersistChanges = new VariableFactory<DataProviders.DoPersistChanges>(INDEX_DoPersistChanges);
        public static readonly IVariableFactory VARFACT_PersistChangesStatus = new VariableFactory<DataProviders.PersistConfigStatus>(INDEX_PersistChangesStatus);

        protected override ITrapGeneratorFactory[] TrapGeneratorFactories => new ITrapGeneratorFactory[]
        {
            TRAPGENFACT_PortsWithPendingChangeCountChanged,
            TRAPGENFACT_ReadVlanConfigStatusChanged
        };

        public static readonly ITrapGeneratorFactory TRAPGENFACT_PortsWithPendingChangeCountChanged = new TrapGeneratorFactory<TrapGenerators.PortsWithPendingChangeCountChanged>();
        public static readonly ITrapGeneratorFactory TRAPGENFACT_ReadVlanConfigStatusChanged = new TrapGeneratorFactory<TrapGenerators.ReadVlanConfigStatusChanged>();

        protected override string TableOid => $"{SnmpAgent.OID_BASE}.1";
        protected override int ItemIndex => (int)Model.RemoteIndex;

        private class DataProviders
        {

            // .0
            public class RemoteIndex : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Model.RemoteIndex);
            }

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
                public override ISnmpData Get() => TruthValue.Create(Model.OperationMethodCollection.ReadVlanMembershipMethod != null);
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
                    if (Model.OperationMethodCollection.ReadVlanMembershipMethod == null)
                        throw new SnmpErrorCodeException(ErrorCode.ResourceUnavailable, "No method provided for reading VLAN configuration of the switch.");
                    await Model.ReadConfigAsync();
                }
            }

            // .13
            public class ReadVlanConfigStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Model.ReadVlanConfigStatus);
            }

            // .14
            public class ReadVlanConfigStatusUpdateTime : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Model.ReadVlanConfigStatusUpdateTime.ToString("HH:mm:ss"));
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
                    if (!TruthValue.CheckForDo(data, "persist changes of the configuration of the switch"))
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

        private class TrapGenerators
        {

            public class PortsWithPendingChangeCountChanged : TrapGenerator
            {

                public override string Code => TrapIdentifiers.CODE_SwitchPortsWithPendingChangeCountChanged;
                public override string EnterpriseBase => $"{Table.SnmpAgent.OID_BASE}.{TrapIdentifiers.EnterpriseBase}";
                public override int SpecificCode => TrapIdentifiers.SPECIFICCODE_SwitchPortsWithPendingChangeCountChanged;

                public override IEnumerable<IVariableFactory> PayloadVariableFactories => new IVariableFactory[]
                {
                    VARFACT_PortsWithPendingChangeCount
                };

                public override void Subscribe()
                    => Table.Model.PortsWithPendingChangeCountChanged += handlePortsWithPendingChangeCountChanged;

                public override void Unsubscribe()
                    => Table.Model.PortsWithPendingChangeCountChanged -= handlePortsWithPendingChangeCountChanged;

                private void handlePortsWithPendingChangeCountChanged(Switch item, int newValue)
                    => SendTrap();

            }

            public class ReadVlanConfigStatusChanged : TrapGenerator
            {

                public override string Code => TrapIdentifiers.CODE_SwitchReadVlanConfigStatusChanged;
                public override string EnterpriseBase => $"{Table.SnmpAgent.OID_BASE}.{TrapIdentifiers.EnterpriseBase}";
                public override int SpecificCode => TrapIdentifiers.SPECIFICCODE_SwitchReadVlanConfigStatusChanged;

                public override IEnumerable<IVariableFactory> PayloadVariableFactories => new IVariableFactory[]
                {
                    VARFACT_ReadVlanConfigStatus,
                    VARFACT_ReadVlanConfigStatusUpdateTime
                };

                public override void Subscribe()
                {
                    Table.Model.ReadVlanConfigStatusChanged += handleReadVlanConfigStatusChanged;
                    Table.Model.ReadVlanConfigStatusUpdateTimeChanged += handleReadVlanConfigStatusUpdateTimeChanged;
                }

                public override void Unsubscribe()
                {
                    Table.Model.ReadVlanConfigStatusChanged += handleReadVlanConfigStatusChanged;
                    Table.Model.ReadVlanConfigStatusUpdateTimeChanged -= handleReadVlanConfigStatusUpdateTimeChanged;
                }

                private void handleReadVlanConfigStatusChanged(Switch item, Status newValue)
                    => SendTrap();

                private void handleReadVlanConfigStatusUpdateTimeChanged(Switch item, DateTime newValue)
                    => SendTrap();

            }

        }

    }
}
