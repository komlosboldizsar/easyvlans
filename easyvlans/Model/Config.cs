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
        public Dictionary<int, UserVlan> Vlans { get; } = new Dictionary<int, UserVlan>();
        public List<UserPort> Ports { get; } = new List<UserPort>();

        public Config(Dictionary<string, Switch> switches, Dictionary<int, UserVlan> vlans, List<UserPort> ports)
        {
            foreach (KeyValuePair<string, Switch> switchKVP in switches)
            {
                Switches.Add(switchKVP.Key, switchKVP.Value);
                switchKVP.Value.AssignConfig(this);
            }
            foreach (KeyValuePair<int, UserVlan> vlanKVP in vlans)
                Vlans.Add(vlanKVP.Key, vlanKVP.Value);
            Ports.AddRange(ports);
        }

    }

}
