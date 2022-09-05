using easyvlans.Model.Remote;

namespace easyvlans.Model
{
    public class Config
    {
        public IList<IRemoteMethod> Remotes { get; set; }
        public IDictionary<string, Switch> Switches { get; set; }
        public IDictionary<int, Vlan> Vlans { get; set; }
        public IDictionary<string, Vlanset> Vlansets { get; set; }
        public IList<Port> Ports { get; set; }
        public IList<PortPage> PortPages { get; set; }
    }
}
