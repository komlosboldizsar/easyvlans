using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using BToolbox.SNMP;
using easyvlans.Helpers;
using Lextm.SharpSnmpLib.Messaging;
using System.Text.RegularExpressions;

namespace easyvlans.Model.SwitchOperationMethods
{
    public class TrapReceiver
    {

        private int _port;
        private IPAddress _boundIpAddress = null;
        private SnmpEngine _engine;

        public TrapReceiver(int port, IPAddress boundIpAddress = null)
        {
            _port = port;
            _boundIpAddress = boundIpAddress ?? IPAddress.Any;
            createEngine();
        }

        public void Start() => _engine.Start();

        public void Stop() => _engine.Stop();

        private void createEngine()
        {
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                new HandlerMapping("v1", "GET", new GetV1MessageHandler()),
                new HandlerMapping("v2", "GET", new GetMessageHandler())
            });
            var pipelineFactory = new SnmpApplicationFactory(new MyLogger(), new ObjectStore(), new AnyMembershipProvider(), handlerFactory);
            _engine = new SnmpEngine(pipelineFactory, new Listener(), new EngineGroup());
            _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, _port));
            _engine.Listener.MessageReceived += messageReceivedHandler;
        }

        private void messageReceivedHandler(object sender, MessageReceivedEventArgs e)
        {

            ObjectIdentifier enterpriseFull = null;
            if ((e.Message.Version == VersionCode.V1) && (e.Message is TrapV1Message trapV1Message))
            {
                TrapEnterprise enterpirseConverter = new(trapV1Message.Enterprise.ToString(), trapV1Message.Specific);
                enterpriseFull = enterpirseConverter.EnterpriseFull;
            }
            if ((e.Message.Version == VersionCode.V2) && (e.Message is TrapV2Message trapV2Message))
            {
                enterpriseFull = trapV2Message.Enterprise;
            }
            if (enterpriseFull == null)
                return;

            if (!_subscriptions.TryGetValue(enterpriseFull, out List<Subscription> subscriptionsForTrap))
                return;

            string userName = e.Message.Parameters.UserName.ToString();
            foreach (Subscription subscription in subscriptionsForTrap)
                if (((subscription.Version == null) || (subscription.Version == e.Message.Version)) && (userName == subscription.CommunityString))
                    subscription.Subscriber.TrapReceived(e.Message, e.Message.Variables());

        }

        public void SubscribeForTrap(ITrapSubscriber subscriber, TrapEnterprise enterprise, VersionCode? version, string communityString)
            => _subscriptions.GetAnyway(enterprise.EnterpriseFull).Add(new Subscription(subscriber, version, communityString));

        private readonly Dictionary<ObjectIdentifier, List<Subscription>> _subscriptions = new();

        private record Subscription(ITrapSubscriber Subscriber, VersionCode? Version, string CommunityString);

        private class MyLogger : ILogger
        {
            public void Log(ISnmpContext context) { }
        }


    }
}
