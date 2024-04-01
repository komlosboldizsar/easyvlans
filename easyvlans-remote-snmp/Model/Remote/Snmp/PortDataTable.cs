using BToolbox.SNMP;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.Remote.Snmp
{
    internal class PortDataTable : ObjectDataTable<Port>
    {

        protected override IVariableFactory[] VariableFactories => new IVariableFactory[]
        {
            VARFACT_Index,
            VARFACT_Label,
            VARFACT_SwitchRemodeIndex,
            VARFACT_CurrentVlanId,
            VARFACT_CurrentVlanName,
            VARFACT_HasComplexMembership,
            VARFACT_HasNotAllowedMembership,
            VARFACT_SetVlanMembershipStatus,
            VARFACT_PendingChanges
        };

        protected override IVariableFactory IndexerVariableFactory => VARFACT_Index;

        public const int INDEX_Index = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_SwitchRemoteIndex = 3;
        public const int INDEX_SwitchLabel = 4;
        public const int INDEX_CurrentVlanId = 5;
        public const int INDEX_CurrentVlanName = 6;
        public const int INDEX_HasComplexMembership = 7;
        public const int INDEX_HasNotAllowedMembership = 8;
        public const int INDEX_SetVlanMembershipStatus = 9;
        public const int INDEX_PendingChanges = 10;

        public static readonly IVariableFactory VARFACT_Index = new VariableFactory<DataProviders.Index>(INDEX_Index);
        public static readonly IVariableFactory VARFACT_Label = new VariableFactory<DataProviders.Label>(INDEX_Label);
        public static readonly IVariableFactory VARFACT_SwitchRemodeIndex = new VariableFactory<DataProviders.SwitchRemoteIndex>(INDEX_SwitchRemoteIndex);
        public static readonly IVariableFactory VARFACT_SwitchLabel = new VariableFactory<DataProviders.SwitchLabel>(INDEX_SwitchLabel);
        public static readonly IVariableFactory VARFACT_CurrentVlanId = new VariableFactory<DataProviders.CurrentVlanId>(INDEX_CurrentVlanId);
        public static readonly IVariableFactory VARFACT_CurrentVlanName = new VariableFactory<DataProviders.CurrentVlanName>(INDEX_CurrentVlanName);
        public static readonly IVariableFactory VARFACT_HasComplexMembership = new VariableFactory<DataProviders.HasComplexMembership>(INDEX_HasComplexMembership);
        public static readonly IVariableFactory VARFACT_HasNotAllowedMembership = new VariableFactory<DataProviders.HasNotAllowedMembership>(INDEX_HasNotAllowedMembership);
        public static readonly IVariableFactory VARFACT_SetVlanMembershipStatus = new VariableFactory<DataProviders.SetVlanMembershipStatus>(INDEX_SetVlanMembershipStatus);
        public static readonly IVariableFactory VARFACT_PendingChanges = new VariableFactory<DataProviders.PendingChanges>(INDEX_PendingChanges);

        protected override ITrapGeneratorFactory[] TrapGeneratorFactories => Array.Empty<ITrapGeneratorFactory>();

        protected override string TableOid => $"{SnmpAgent.OID_BASE}.2";
        protected override int ItemIndex => (int)Model.RemoteIndex;


        private class DataProviders
        {

            // .1
            public class Index : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Model.Index);
            }
            
            // .2
            public class Label : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Model.Label);
            }

            // .3
            public class SwitchRemoteIndex : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Model.Switch.RemoteIndex ?? 0);
            }
            
            // .4
            public class SwitchLabel : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Model.Switch.Label);
            }

            // .5
            public class CurrentVlanId : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Model.CurrentVlan?.ID ?? 0);
                public override async void Set(ISnmpData data)
                {
                    if (data is Integer32 intData)
                    {
                        int vlanId = intData.ToInt32();
                        Vlan vlan = Model.Vlans.FirstOrDefault(v => v.ID == vlanId);
                        if (vlan == null)
                            throw new SnmpErrorCodeException(ErrorCode.BadValue, "VLAN with given ID not found or not valid for this port.");
                        if (!await Model.SetVlanTo(vlan))
                            throw new SnmpErrorCodeException(ErrorCode.GenError, "Setting port to be the member of the given VLAN not successful.");
                    }
                }
            }

            // .6
            public class CurrentVlanName : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Model.CurrentVlan?.Name ?? CURRENT_VLAN_UNKNOWN);
                private const string CURRENT_VLAN_UNKNOWN = "(unknown)";
            }

            // .7
            public class HasComplexMembership : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.HasComplexMembership);
            }

            // .8
            public class HasNotAllowedMembership : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.HasNotAllowedMembership);
            }

            // .9
            public class SetVlanMembershipStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Model.SetVlanMembershipStatus);
            }

            // .10
            public class PendingChanges : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.PendingChanges);
            }

        }

    }
}
