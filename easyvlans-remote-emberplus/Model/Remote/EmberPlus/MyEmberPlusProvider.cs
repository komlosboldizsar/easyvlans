using easyvlans.Model.Remote;
using EmberPlusProviderClassLib;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.EmberPlus
{
    class MyEmberPlusProvider : IRemoteMethod
    {
        private int _port;
        private EmberPlusProvider _tree;
        public string Code => "ember";

        public void Start() { }

        public MyEmberPlusProvider(int port, string? identity = null)
        {
            _port = port;
            _tree = new EmberPlusProvider(
        _port,
        "EasyVLANs",
        "EasyVLANs controlling");

            _tree.CreateIdentityNode(
                1,
                identity ?? "EasyVLANs",
                "EasyVLANs",
                "Komlós Boldizsár",
                "v1.0.0");

        }
        public void MeetConfig(Config config)
        {
            _ = new DataTableBoundObjectStoreAdapter<Switch, SwitchDataTable>(this, config.Switches.Values, s => (s.RemoteIndex != null));
            _ = new DataTableBoundObjectStoreAdapter<Port, PortDataTable>(this, config.Ports, p => (p.RemoteIndex != null));
        }

    }
}
