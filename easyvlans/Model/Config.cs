using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Config
    {

        public Dictionary<string, Switch> Switches { get; } = new Dictionary<string, Switch>();
        public Dictionary<int, Vlan> Vlans { get; } = new Dictionary<int, Vlan>();
        public List<Port> Ports { get; } = new List<Port>();

        public Config(Dictionary<string, Switch> switches, Dictionary<int, Vlan> vlans, List<Port> ports)
        {
            foreach (KeyValuePair<string, Switch> switchKVP in switches)
                Switches.Add(switchKVP.Key, switchKVP.Value);
            foreach (KeyValuePair<int, Vlan> vlanKVP in vlans)
                Vlans.Add(vlanKVP.Key, vlanKVP.Value);
            Ports.AddRange(ports);
        }

    }

}
