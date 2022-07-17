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

        protected override IVariableFactory[] VariableFactories => new IVariableFactory[]
        {
            new VariableFactory<DataProviders.Id>(INDEX_Id),
            new VariableFactory<DataProviders.Label>(INDEX_Label),
            new VariableFactory<DataProviders.PortsWithPendingChangeCount>(INDEX_PortsWithPendingChangeCount),
            new VariableFactory<DataProviders.ReadVlanConfigStatus>(INDEX_ReadVlanConfigStatus),
            new VariableFactory<DataProviders.PersistVlanConfigStatus>(INDEX_PersistVlanConfigStatus)
        };

        public const int INDEX_Id = 1;
        public const int INDEX_Label = 2;
        public const int INDEX_PortsWithPendingChangeCount = 3;
        public const int INDEX_ReadVlanConfigStatus = 4;
        public const int INDEX_PersistVlanConfigStatus = 5;

        protected override string TableOid => $"{SnmpAgent.OID_BASE}.1";
        protected override int GetItemIndex() => (int)_item.SnmpIndex;

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
                public override ISnmpData Get() => new Integer32(Item.PortsWithPendingChangeCount);
            }

            public class PersistVlanConfigStatus : VariableDataProvider
            {
                public override ISnmpData Get() => new Integer32(Item.PortsWithPendingChangeCount);
            }

        }

    }
}
