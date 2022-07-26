using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Config
    {

        public readonly SettingsGroups Settings;
        public Dictionary<string, Switch> Switches { get; } = new Dictionary<string, Switch>();
        public Dictionary<int, Vlan> Vlans { get; } = new Dictionary<int, Vlan>();
        public List<Port> Ports { get; } = new List<Port>();
        public List<PortPage> PortPages { get; } = new List<PortPage>();

        public Config(SettingsGroups settings, Dictionary<string, Switch> switches, Dictionary<int, Vlan> vlans, List<Port> ports, List<PortPage> portPages)
        {
            Settings = settings;
            foreach (KeyValuePair<string, Switch> switchKVP in switches)
            {
                Switches.Add(switchKVP.Key, switchKVP.Value);
                switchKVP.Value.Config = this;
            }
            foreach (KeyValuePair<int, Vlan> vlanKVP in vlans)
                Vlans.Add(vlanKVP.Key, vlanKVP.Value);
            Ports.AddRange(ports);
            PortPages.AddRange(portPages);
        }

        public class SettingsGroups
        {
            public SnmpSettings Snmp;
        }

        public class SnmpSettings
        {
            public bool? Enabled;
            public int? Port;
            public string CommunityRead;
            public string CommunityWrite;
        }

    }

}
