using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class SnmpAgent
    {

        private ObjectStore _objectStore;
        private int _port;
        private SnmpEngine _engine;

        private const int PORT_DEFAULT = 161;
        private const string COMMUNITY_READ_DEFAULT = "public";
        private const string COMMUNITY_WRITE_DEFAULT = "public";

        public void CreateEngine(Config.SnmpSettings config)
        {
            _objectStore = new ObjectStore();
            IMembershipProvider v1MembershipProvider = new Version1MembershipProvider(
                new OctetString(config.CommunityRead ?? COMMUNITY_READ_DEFAULT),
                new OctetString(config.CommunityWrite ?? COMMUNITY_WRITE_DEFAULT));
            IMembershipProvider membershipProvider = new ComposedMembershipProvider(new IMembershipProvider[] { v1MembershipProvider });
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                new HandlerMapping("v1", "GET", new GetV1MessageHandler()),
                new HandlerMapping("v1", "GETNEXT", new GetNextV1MessageHandler())
            });
            var pipelineFactory = new SnmpApplicationFactory(new MyLogger(), _objectStore, membershipProvider, handlerFactory);
            _port = config.Port ?? PORT_DEFAULT;
            _engine = new SnmpEngine(pipelineFactory, new Listener(), new EngineGroup());
        }

        public void AddDataFromConfig(Config config)
        {
            _objectStore.AddRange(config.Switches.Values.Where(s => s.RemoteIndex != null).Select(s => new SwitchDataTable(s)));
            _objectStore.AddRange(config.Ports.Where(p => p.RemoteIndex != null).Select(p => new PortDataTable(p)));
        }

        public void StartListening()
        {
            LogDispatcher.I($"Starting SNMP service at UDP port {_port}...");
            _engine.Listener.ClearBindings();
            if (Socket.OSSupportsIPv4)
                _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, _port));
            _engine.Start();
        }

        private class MyLogger : ILogger
        {
            public void Log(ISnmpContext context) { }
        }

        public const string OID_BASE = "1.3.6.1.4.1.59150.1";

    }
}
