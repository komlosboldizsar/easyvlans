namespace easyvlans.Model
{
    public class Config
    {

        public SettingsGroups Settings;
        public IDictionary<string, Switch> Switches { get; set; }
        public IDictionary<int, Vlan> Vlans { get; set; }
        public IDictionary<string, Vlanset> Vlansets { get; set; }
        public IList<Port> Ports { get; set; }
        public IList<PortPage> PortPages { get; set; }

        public class SettingsGroups
        {
            public SnmpSettings Snmp;
        }

        public class SnmpSettings
        {
            public bool Enabled;
            public int Port;
            public string CommunityRead;
            public string CommunityWrite;
        }

    }

}
