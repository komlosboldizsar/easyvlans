using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Config
    {
        private Dictionary<string, Switch> Switches = new Dictionary<string, Switch>();
        private Dictionary<string, Vlan> Vlans = new Dictionary<string, Vlan>();
        private List<Port> Ports = new List<Port>();

        public Config(Dictionary<string, Switch> switches, Dictionary<string, Vlan> vlans, List<Port> ports)
        {
            foreach (KeyValuePair<string, Switch> switchKVP in switches)
                Switches.Add(switchKVP.Key, switchKVP.Value);
            foreach (KeyValuePair<string, Vlan> vlanKVP in vlans)
                Vlans.Add(vlanKVP.Key, vlanKVP.Value);
            Ports.AddRange(ports);
        }

    }

}
