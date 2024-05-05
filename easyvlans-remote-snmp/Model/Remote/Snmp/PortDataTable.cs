using BToolbox.SNMP;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.Remote.Snmp
{
    internal class PortDataTable : ObjectDataTable<Port>
    {

        protected override IVariableFactory[] VariableFactories => new IVariableFactory[]
        {
            VARFACT_RemoteIndex,
            VARFACT_Index,
            VARFACT_Label,
            VARFACT_SwitchRemodeIndex,
            VARFACT_CurrentVlanId,
            VARFACT_CurrentVlanName,
            VARFACT_DefaultVlanId,
            VARFACT_DefaultVlanName,
            VARFACT_HasDefaultVlanDefined,
            VARFACT_HasComplexMembership,
            VARFACT_HasNotAllowedMembership,
            VARFACT_HasDifferentMembershipFromDefault,
            VARFACT_SetVlanMembershipStatus,
            VARFACT_PendingChanges,
            VARFACT_DoSetToDefaultVlan
        };

        protected override IVariableFactory IndexerVariableFactory => VARFACT_RemoteIndex;

        public const int INDEX_RemoteIndex = 0;
        public const int INDEX_Index = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_SwitchRemoteIndex = 3;
        public const int INDEX_SwitchLabel = 4;
        public const int INDEX_CurrentVlanId = 5;
        public const int INDEX_CurrentVlanName = 6;
        public const int INDEX_DefaultVlanId = 7;
        public const int INDEX_DefaultVlanName = 8;
        public const int INDEX_HasDefaultVlanDefined = 9;
        public const int INDEX_HasComplexMembership = 21;
        public const int INDEX_HasNotAllowedMembership = 22;
        public const int INDEX_HasDifferentMembershipFromDefault = 23;
        public const int INDEX_SetVlanMembershipStatus = 41;
        public const int INDEX_PendingChanges = 42;
        public const int INDEX_DoSetToDefaultVlan = 61;

        public static readonly IVariableFactory VARFACT_RemoteIndex = new VariableFactory<DataProviders.RemoteIndex>(INDEX_RemoteIndex);
        public static readonly IVariableFactory VARFACT_Index = new VariableFactory<DataProviders.Index>(INDEX_Index);
        public static readonly IVariableFactory VARFACT_Label = new VariableFactory<DataProviders.Label>(INDEX_Label);
        public static readonly IVariableFactory VARFACT_SwitchRemodeIndex = new VariableFactory<DataProviders.SwitchRemoteIndex>(INDEX_SwitchRemoteIndex);
        public static readonly IVariableFactory VARFACT_SwitchLabel = new VariableFactory<DataProviders.SwitchLabel>(INDEX_SwitchLabel);
        public static readonly IVariableFactory VARFACT_CurrentVlanId = new VariableFactory<DataProviders.CurrentVlanId>(INDEX_CurrentVlanId);
        public static readonly IVariableFactory VARFACT_CurrentVlanName = new VariableFactory<DataProviders.CurrentVlanName>(INDEX_CurrentVlanName);
        public static readonly IVariableFactory VARFACT_DefaultVlanId = new VariableFactory<DataProviders.DefaultVlanId>(INDEX_DefaultVlanId);
        public static readonly IVariableFactory VARFACT_DefaultVlanName = new VariableFactory<DataProviders.DefaultVlanName>(INDEX_DefaultVlanName);
        public static readonly IVariableFactory VARFACT_HasDefaultVlanDefined = new VariableFactory<DataProviders.HasDefaultVlanDefined>(INDEX_HasDefaultVlanDefined);
        public static readonly IVariableFactory VARFACT_HasComplexMembership = new VariableFactory<DataProviders.HasComplexMembership>(INDEX_HasComplexMembership);
        public static readonly IVariableFactory VARFACT_HasNotAllowedMembership = new VariableFactory<DataProviders.HasNotAllowedMembership>(INDEX_HasNotAllowedMembership);
        public static readonly IVariableFactory VARFACT_HasDifferentMembershipFromDefault = new VariableFactory<DataProviders.HasDifferentMembershipFromDefault>(INDEX_HasDifferentMembershipFromDefault);
        public static readonly IVariableFactory VARFACT_SetVlanMembershipStatus = new VariableFactory<DataProviders.SetVlanMembershipStatus>(INDEX_SetVlanMembershipStatus);
        public static readonly IVariableFactory VARFACT_PendingChanges = new VariableFactory<DataProviders.PendingChanges>(INDEX_PendingChanges);
        public static readonly IVariableFactory VARFACT_DoSetToDefaultVlan = new VariableFactory<DataProviders.DoSetToDefaultVlan>(INDEX_DoSetToDefaultVlan);

        protected override ITrapGeneratorFactory[] TrapGeneratorFactories => new ITrapGeneratorFactory[]
        {
            TRAPGENFACT_VlanMembershipChanged
        };

        public static readonly ITrapGeneratorFactory TRAPGENFACT_VlanMembershipChanged = new TrapGeneratorFactory<TrapGenerators.VlanMembershipChanged>();

        protected override string TableOid => $"{SnmpAgent.OID_BASE}.2";
        protected override int ItemIndex => (int)Model.RemoteIndex;


        private class DataProviders
        {

            // .0
            public class RemoteIndex : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Model.RemoteIndex);
            }

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
            public class DefaultVlanId : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Model.DefaultVlan?.ID ?? 0);
            }

            // .8
            public class DefaultVlanName : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Model.DefaultVlan?.Name ?? DEFAULT_VLAN_UNDEFINED);
                private const string DEFAULT_VLAN_UNDEFINED = "(undefined)";
            }

            // .9
            public class HasDefaultVlanDefined : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.DefaultVlan != null);
            }

            // .21
            public class HasComplexMembership : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.HasComplexMembership);
            }

            // .22
            public class HasNotAllowedMembership : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.HasNotAllowedMembership);
            }

            // .23
            public class HasDifferentMembershipFromDefault : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create((Model.DefaultVlan != null) && (Model.DefaultVlan != Model.CurrentVlan));
            }

            // .41
            public class SetVlanMembershipStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Model.SetVlanMembershipStatus);
            }

            // .42
            public class PendingChanges : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Model.PendingChanges);
            }

            // .61
            public class DoSetToDefaultVlan : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(0);
                public override async void Set(ISnmpData data)
                {
                    if (!TruthValue.CheckForDo(data, "set ports VLAN membership to its default state"))
                        return;
                    if (Model.DefaultVlan == null)
                        throw new SnmpErrorCodeException(ErrorCode.InconsistentValue, "No default VLAN defined for port.");
                    if (!await Model.SetVlanTo(Model.DefaultVlan))
                        throw new SnmpErrorCodeException(ErrorCode.GenError, "Setting port to be the member of its default VLAN not successful.");
                }
            }

        }

        private class TrapGenerators
        {

            public class VlanMembershipChanged : TrapGenerator
            {

                public override string Code => TrapIdentifiers.CODE_PortVlanMembershipChanged;
                public override string EnterpriseBase => $"{Table.SnmpAgent.OID_BASE}.{TrapIdentifiers.EnterpriseBase}";
                public override int SpecificCode => TrapIdentifiers.SPECIFICCODE_PortVlanMembershipChanged;

                public override IEnumerable<IVariableFactory> PayloadVariableFactories => new IVariableFactory[]
                {
                    VARFACT_CurrentVlanId,
                    VARFACT_CurrentVlanName,
                    VARFACT_HasComplexMembership,
                    VARFACT_HasNotAllowedMembership,
                    VARFACT_HasDifferentMembershipFromDefault
                };

                public override void Subscribe()
                {
                    Table.Model.CurrentVlanChanged += handleCurrentVlanChanged;
                }

                public override void Unsubscribe()
                {
                    Table.Model.CurrentVlanChanged -= handleCurrentVlanChanged;
                }

                private void handleCurrentVlanChanged(Port item, Vlan newValue)
                    => SendTrap();

            }

        }

    }
}
