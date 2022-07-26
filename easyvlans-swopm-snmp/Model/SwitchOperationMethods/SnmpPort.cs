using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    public class SnmpPort
    {
        public int ID { get; init; }
        public int PVID { get; set; }
        public int? VLAN { get; set; }
        public int owningVlans;
        public int lastOwningVlan;
        public SnmpPort(int id) => ID = id;
    }
}
