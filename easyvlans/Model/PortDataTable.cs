using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class PortDataTable : DataTable<Port>
    {

        public PortDataTable(Port port) : base(port) { }
        protected override ScalarObject[] getObjects() => new ScalarObject[]
        {
            new IndexVariable(_item),
            new LabelVariable(_item),
            new SwitchSnmpIndexVariable(_item),
            new SwitchLabelVariable(_item),
            new CurrentVlanIdVariable(_item),
            new CurrentVlanNameVariable(_item),
            new HasComplexMembershipVariable(_item),
            new SetVlanMembershipStatusVariable(_item),
            new PendingChangesVariable(_item)
        };

        public static readonly string OID_TABLE = $"{SnmpAgent.OID_BASE}.2";
        public const int INDEX_Index = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_SwitchSnmpIndex = 3;
        public const int INDEX_SwitchLabel = 4;
        public const int INDEX_CurrentVlanId = 5;
        public const int INDEX_CurrentVlanName = 6;
        public const int INDEX_HasComplexMembership = 7;
        public const int INDEX_SetVlanMembershipStatus = 8;
        public const int INDEX_PendingChanges = 9;

        private abstract class OidGeneratorBase : OidGeneratorBaseBase
        {
            protected override string TableID => OID_TABLE;
            protected override int GetItemIndex(Port port) => (int)port.SnmpIndex;
        }

        private class IndexVariable : Variable<IndexVariable.OidGenerator>
        {
            public IndexVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new Integer32(_item.Index);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_Index;
            }
        }

        private class LabelVariable : Variable<LabelVariable.OidGenerator>
        {
            public LabelVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new OctetString(_item.Label);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_Label;
            }
        }

        private class SwitchSnmpIndexVariable : Variable<SwitchSnmpIndexVariable.OidGenerator>
        {
            public SwitchSnmpIndexVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new Integer32(_item.Switch.SnmpIndex ?? 0);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_SwitchSnmpIndex;
            }
        }

        private class SwitchLabelVariable : Variable<SwitchLabelVariable.OidGenerator>
        {
            public SwitchLabelVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new OctetString(_item.Switch.Label);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_SwitchLabel;
            }
        }

        private class CurrentVlanIdVariable : Variable<CurrentVlanIdVariable.OidGenerator>
        {
            public CurrentVlanIdVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new Integer32(_item.CurrentVlan?.ID ?? 0);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_CurrentVlanId;
            }
        }

        private class CurrentVlanNameVariable : Variable<CurrentVlanNameVariable.OidGenerator>
        {
            public CurrentVlanNameVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new OctetString(_item.CurrentVlan?.Name ?? CURRENT_VLAN_UNKNOWN);
                set => throw new AccessFailureException();
            }
            private const string CURRENT_VLAN_UNKNOWN = "(unknown)";
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_CurrentVlanName;
            }
        }

        private class HasComplexMembershipVariable : Variable<HasComplexMembershipVariable.OidGenerator>
        {
            public HasComplexMembershipVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new Integer32(_item.HasComplexMembership ? 1 : 0);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_HasComplexMembership;
            }
        }

        private class SetVlanMembershipStatusVariable : Variable<SetVlanMembershipStatusVariable.OidGenerator>
        {
            public SetVlanMembershipStatusVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new Integer32((int)_item.SetVlanMembershipStatus);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_SetVlanMembershipStatus;
            }
        }

        private class PendingChangesVariable : Variable<PendingChangesVariable.OidGenerator>
        {
            public PendingChangesVariable(Port port) : base(port) { }
            public override ISnmpData Data
            {
                get => new Integer32(_item.PendingChanges ? 1 : 0);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_PendingChanges;
            }
        }

    }
}
