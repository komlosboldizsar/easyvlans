using easyvlans.Model.Deserializers;
using easyvlans.Model.Remote.Snmp;
namespace easyvlans.Modules
{
    public class RemoteSnmpModule : IModule
    {
        public void Init()
        {
            RemoteMethodsDeserializer.Instance.Register(new SnmpAgentDeserializer());
        }
    }
}
