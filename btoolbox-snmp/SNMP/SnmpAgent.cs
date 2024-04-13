using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System.Net;
using System.Net.Sockets;

namespace BToolbox.SNMP
{
    public abstract class SnmpAgent
    {

        public readonly int Port;
        private readonly string _communityRead;
        private readonly string _communityWrite;
        public readonly TrapSendingConfig TrapSendingConfig;

        public readonly MyObjectStore ObjectStore = new();
        private SnmpEngine _engine;

        public bool Started { get; private set; } = false;
        public Exception StartException { get; private set; } = null;

        public delegate void StatusChangedDelegate(bool started, Exception startException);
        public event StatusChangedDelegate StatusChanged;

        public SnmpAgent(int port, string communityRead, string communityWrite, TrapSendingConfig trapSendingConfig)
        {
            Port = port;
            _communityRead = communityRead;
            _communityWrite = communityWrite;
            TrapSendingConfig = trapSendingConfig;
            createEngine();
        }

        private void createEngine()
        {
            IMembershipProvider v1MembershipProvider = new Version1MembershipProvider(new OctetString(_communityRead), new OctetString(_communityWrite));
            IMembershipProvider v2MembershipProvider = new Version2MembershipProvider(new OctetString(_communityRead), new OctetString(_communityWrite));
            IMembershipProvider membershipProvider = new ComposedMembershipProvider(new IMembershipProvider[] {
                v1MembershipProvider,
                v2MembershipProvider
            });
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                new HandlerMapping("v1", "GET", new GetV1MessageHandler()),
                new HandlerMapping("v1", "GETNEXT", new GetNextV1MessageHandler()),
                new HandlerMapping("v1", "SET", new MySetV1MessageHandler()),
                new HandlerMapping("v2", "GET", new GetMessageHandler()),
                new HandlerMapping("v2", "GETNEXT", new GetNextMessageHandler()),
                new HandlerMapping("v2", "GETBULK", new GetBulkMessageHandler()),
                new HandlerMapping("v2", "SET", new MySetMessageHandler())
            });
            var pipelineFactory = new SnmpApplicationFactory(new MyLogger(), ObjectStore, membershipProvider, handlerFactory);
            _engine = new SnmpEngine(pipelineFactory, new Listener(), new EngineGroup());
        }

        public void Start()
        {
            if (Started)
                return;
            try
            {
                _engine.Listener.ClearBindings();
                if (Socket.OSSupportsIPv4)
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, Port));
                _engine.Start();
                OnSuccessfulStart();
                statusChanged(true, null);
            }
            catch (Exception ex)
            {
                //LogDispatcher.E($"Couldn't start SNMP service at UDP port {_port}, because IP endpoint is in use by another application.");
                statusChanged(false, ex);
            }
        }

        public void SendTraps(string code, TrapEnterprise enterprise, IList<Variable> variables)
            => TrapSendingConfig.SendAll(code, enterprise, variables);

        public void SendTraps(string code, string enterpriseBase, int specificCode, IList<Variable> variables)
            => TrapSendingConfig.SendAll(code, new TrapEnterprise(enterpriseBase, specificCode), variables);

        private void statusChanged(bool started, Exception startException)
        {
            Started = started;
            StartException = startException;
            StatusChanged?.Invoke(started, startException);
        }

        protected virtual void OnSuccessfulStart() { } // hook

        private class MyLogger : ILogger
        {
            public void Log(ISnmpContext context) { }
        }

        public abstract string OID_BASE { get; }

    }
}
