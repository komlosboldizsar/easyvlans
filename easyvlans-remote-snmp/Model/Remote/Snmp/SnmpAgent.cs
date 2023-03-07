using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;
using System.Net;
using System.Net.Sockets;

namespace easyvlans.Model.Remote.Snmp
{
    public class SnmpAgent : IRemoteMethod
    {

        public string Code => "snmp";

        private readonly int _port;
        private readonly string _communityRead;
        private readonly string _communityWrite;

        private MyObjectStore _objectStore;
        private SnmpEngine _engine;

        private const string COMMUNITY_READ_DEFAULT = "public";
        private const string COMMUNITY_WRITE_DEFAULT = "public";

        public SnmpAgent(int port, string communityRead, string communityWrite)
        {
            _port = port;
            _communityRead = communityRead;
            _communityWrite = communityWrite;
            createEngine();
        }

        private void createEngine()
        {
            _objectStore = new();
            IMembershipProvider v1MembershipProvider = new Version1MembershipProvider(
                new OctetString(_communityRead ?? COMMUNITY_READ_DEFAULT),
                new OctetString(_communityWrite ?? COMMUNITY_WRITE_DEFAULT));
            IMembershipProvider membershipProvider = new ComposedMembershipProvider(new IMembershipProvider[] { v1MembershipProvider });
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                new HandlerMapping("v1", "GET", new GetV1MessageHandler()),
                new HandlerMapping("v1", "GETNEXT", new GetNextV1MessageHandler()),
                new HandlerMapping("v1", "SET", new MySetV1MessageHandler())
            });
            var pipelineFactory = new SnmpApplicationFactory(new MyLogger(), _objectStore, membershipProvider, handlerFactory);
            _engine = new SnmpEngine(pipelineFactory, new Listener(), new EngineGroup());
        }

        public void MeetConfig(Config config)
        {
            _objectStore.AddRange(config.Switches.Values.Where(s => s.RemoteIndex != null).Select(s => new SwitchDataTable(s)));
            _objectStore.AddRange(config.Ports.Where(p => p.RemoteIndex != null).Select(p => new PortDataTable(p)));
        }

        public void Start()
        {
            LogDispatcher.I($"Starting SNMP service at UDP port {_port}...");
            try
            {
                _engine.Listener.ClearBindings();
                if (Socket.OSSupportsIPv4)
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, _port));
                _engine.Start();
            }
            catch (Exception)
            {
                LogDispatcher.E($"Couldn't start SNMP service at UDP port {_port}, because IP endpoint is in use by another application.");
            }
        }

        private class MyLogger : ILogger
        {
            public void Log(ISnmpContext context) { }
        }

        public const string OID_BASE = "1.3.6.1.4.1.59150.1";

    }
}
