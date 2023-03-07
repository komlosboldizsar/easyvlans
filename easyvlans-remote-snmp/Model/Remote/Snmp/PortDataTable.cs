using Lextm.SharpSnmpLib;

namespace easyvlans.Model.Remote.Snmp
{
    internal class PortDataTable : DataTable<Port>
    {

        public PortDataTable(Port port) : base(port) { }

        protected override IVariableFactory[] VariableFactories => new IVariableFactory[]
        {
            new VariableFactory<DataProviders.Index>(INDEX_Index),
            new VariableFactory<DataProviders.Label>(INDEX_Label),
            new VariableFactory<DataProviders.SwitchRemoteIndex>(INDEX_SwitchRemoteIndex),
            new VariableFactory<DataProviders.SwitchLabel>(INDEX_SwitchLabel),
            new VariableFactory<DataProviders.CurrentVlanId>(INDEX_CurrentVlanId),
            new VariableFactory<DataProviders.CurrentVlanName>(INDEX_CurrentVlanName),
            new VariableFactory<DataProviders.HasComplexMembership>(INDEX_HasComplexMembership),
            new VariableFactory<DataProviders.HasNotAllowedMembership>(INDEX_HasNotAllowedMembership),
            new VariableFactory<DataProviders.SetVlanMembershipStatus>(INDEX_SetVlanMembershipStatus),
            new VariableFactory<DataProviders.PendingChanges>(INDEX_PendingChanges)
        };

        protected override string TableOid => $"{SnmpAgent.OID_BASE}.2";

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

        private class DataProviders
        {

            // .1
            public class Index : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Item.Index);
            }
            
            // .2
            public class Label : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Item.Label);
            }

            // .3
            public class SwitchRemoteIndex : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Item.Switch.RemoteIndex ?? 0);
            }
            
            // .4
            public class SwitchLabel : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Item.Switch.Label);
            }

            // .5
            public class CurrentVlanId : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Item.CurrentVlan?.ID ?? 0);
                public override async void Set(ISnmpData data)
                {
                    if (data is Integer32 intData)
                    {
                        int vlanId = intData.ToInt32();
                        Vlan vlan = Item.Vlans.FirstOrDefault(v => v.ID == vlanId);
                        if (vlan == null)
                            throw new SnmpErrorCodeException(ErrorCode.BadValue, "VLAN with given ID not found or not valid for this port.");
                        if (!await Item.SetVlanTo(vlan))
                            throw new SnmpErrorCodeException(ErrorCode.GenError, "Setting port to be the member of the given VLAN not successful.");
                    }
                }
            }

            // .6
            public class CurrentVlanName : VariableDataProvider
            {
                public override ISnmpData Get() => new OctetString(Item.CurrentVlan?.Name ?? CURRENT_VLAN_UNKNOWN);
                private const string CURRENT_VLAN_UNKNOWN = "(unknown)";
            }

            // .7
            public class HasComplexMembership : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Item.HasComplexMembership);
            }

            // .8
            public class HasNotAllowedMembership : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Item.HasNotAllowedMembership);
            }

            // .9
            public class SetVlanMembershipStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32((int)Item.SetVlanMembershipStatus);
            }

            // .10
            public class PendingChanges : VariableDataProvider
            {
                public override ISnmpData Get() => TruthValue.Create(Item.PendingChanges);
            }

        }

    }
}
