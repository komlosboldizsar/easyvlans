using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class SwitchDataTable : DataTable<Switch>
    {

        public SwitchDataTable(Switch @switch) : base(@switch) { }
        protected override ScalarObject[] getObjects() => new ScalarObject[]
        {
            new IdVariable(_item),
            new LabelVariable(_item),
            new PortsWithPendingChangeCountVariable(_item),
            new ReadVlanConfigStatusVariable(_item),
            new PersistVlanConfigStatusVariable(_item)
        };

        public static readonly string OID_TABLE = $"{SnmpAgent.OID_BASE}.1";
        public const int INDEX_Id = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_PortsWithPendingChangeCount = 3;
        public const int INDEX_ReadVlanConfigStatus = 4;
        public const int INDEX_PersistVlanConfigStatus = 5;

        private abstract class OidGeneratorBase : OidGeneratorBaseBase
        {
            protected override string TableID => OID_TABLE;
            protected override int GetItemIndex(Switch item) => (int)item.SnmpIndex;
        }

        private class IdVariable : Variable<IdVariable.OidGenerator>
        {
            public IdVariable(Switch @switch) : base(@switch) { }
            public override ISnmpData Data
            {
                get => new OctetString(_item.ID);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_Id;
            }
        }

        private class LabelVariable : Variable<LabelVariable.OidGenerator>
        {
            public LabelVariable(Switch @switch) : base(@switch) { }
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

        private class PortsWithPendingChangeCountVariable : Variable<PortsWithPendingChangeCountVariable.OidGenerator>
        {
            public PortsWithPendingChangeCountVariable(Switch @switch) : base(@switch) { }
            public override ISnmpData Data
            {
                get => new Integer32(_item.PortsWithPendingChangeCount);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_PortsWithPendingChangeCount;
            }
        }

        private class ReadVlanConfigStatusVariable : Variable<ReadVlanConfigStatusVariable.OidGenerator>
        {
            public ReadVlanConfigStatusVariable(Switch @switch) : base(@switch) { }
            public override ISnmpData Data
            {
                get => new Integer32(_item.PortsWithPendingChangeCount);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_ReadVlanConfigStatus;
            }
        }

        private class PersistVlanConfigStatusVariable : Variable<PersistVlanConfigStatusVariable.OidGenerator>
        {
            public PersistVlanConfigStatusVariable(Switch @switch) : base(@switch) { }
            public override ISnmpData Data
            {
                get => new Integer32(_item.PortsWithPendingChangeCount);
                set => throw new AccessFailureException();
            }
            public class OidGenerator : OidGeneratorBase
            {
                protected override int PropertyIndex => INDEX_PersistVlanConfigStatus;
            }
        }

    }
}
