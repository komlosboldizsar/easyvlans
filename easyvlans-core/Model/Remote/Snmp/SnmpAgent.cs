﻿using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using System.Net;
using System.Net.Sockets;

namespace easyvlans.Model.Remote.Snmp
{
    public class SnmpAgent
    {

        private MyObjectStore _objectStore;
        private int _port;
        private SnmpEngine _engine;

        private const string COMMUNITY_READ_DEFAULT = "public";
        private const string COMMUNITY_WRITE_DEFAULT = "public";

        public void CreateEngine(Config.SnmpSettings config)
        {
            _objectStore = new();
            IMembershipProvider v1MembershipProvider = new Version1MembershipProvider(
                new OctetString(config.CommunityRead ?? COMMUNITY_READ_DEFAULT),
                new OctetString(config.CommunityWrite ?? COMMUNITY_WRITE_DEFAULT));
            IMembershipProvider membershipProvider = new ComposedMembershipProvider(new IMembershipProvider[] { v1MembershipProvider });
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                new HandlerMapping("v1", "GET", new GetV1MessageHandler()),
                new HandlerMapping("v1", "GETNEXT", new GetNextV1MessageHandler()),
                new HandlerMapping("v1", "SET", new SetV1MessageHandler())
            });
            var pipelineFactory = new SnmpApplicationFactory(new MyLogger(), _objectStore, membershipProvider, handlerFactory);
            _port = config.Port;
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
