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
using BToolbox.Model;

namespace easyvlans.Model.SwitchOperationMethods
{
    public partial class TrapReceiver
    {

        private int _port;
        private SnmpEngine _engine;

        public TrapReceiver(int port)
        {
            _port = port;
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
            if ((e.Message.Version == VersionCode.V1) && (e.Message is TrapV1Message trapV1Message))
            {
                handleTrapV1Message(trapV1Message, e.Sender.Address);
                return;
            }
            if ((e.Message.Version == VersionCode.V2) && (e.Message is TrapV2Message trapV2Message))
            {
                handleTrapV2Message(trapV2Message, e.Sender.Address);
                return;
            }
        }

        private interface ISubscription
        {
            ITrapSubscriber Subscriber { get; }
            string CommunityString { get; }
            IPAddress IPAddress { get; }
        }

        private static void checkSubscriptionsForMessage(ISnmpMessage message, IPAddress remoteIpAddress, IEnumerable<ISubscription> subscriptions, IEnumerable<Variable> variables)
        {
            string userName = message.Parameters.UserName.ToString();
            subscriptions.Where(s => ((s.IPAddress.Equals(remoteIpAddress)) && (s.CommunityString == userName))).Foreach(s => s.Subscriber.TrapReceived(message, variables));
        }

        #region V1 traps
        public void SubscribeForV1Trap(ITrapSubscriber subscriber, GenericCode genericCode, int? specificCode, string enterpriseFilter, IPAddress ipAddress, string communityString)
        {
            if ((specificCode != null) && (genericCode != GenericCode.EnterpriseSpecific))
                throw new ArgumentException("Specific code can only be defined when generic code is 'enterprise specific'.", nameof(genericCode));
            _subscriptionsV1.GetAnyway(new(genericCode, specificCode)).Add(new SubscriptionV1(subscriber, ipAddress, communityString, enterpriseFilter));
        }
        
        private readonly Dictionary<TrapCodeV1, List<SubscriptionV1>> _subscriptionsV1 = new(TrapCodeV1.EQUALITY_COMPARER);
        private record SubscriptionV1(ITrapSubscriber Subscriber, IPAddress IPAddress, string CommunityString, string EnterpriseFilter) : ISubscription;

        private void handleTrapV1Message(TrapV1Message message, IPAddress remoteIpAddress)
        {
            if (!_subscriptionsV1.TryGetValue(new(message.Generic, message.Specific), out List<SubscriptionV1> subscriptionsForTrap))
                return;
            checkSubscriptionsForMessage(message, remoteIpAddress, subscriptionsForTrap, message.Variables());
        }
        #endregion

        #region V2 traps
        public void SubscribeForV2Trap(ITrapSubscriber subscriber, ObjectIdentifier trapOid, IPAddress ipAddress, string communityString)
            => _subscriptionsV2.GetAnyway(trapOid).Add(new SubscriptionV2(subscriber, ipAddress, communityString));

        private readonly Dictionary<ObjectIdentifier, List<SubscriptionV2>> _subscriptionsV2 = new();

        private record SubscriptionV2(ITrapSubscriber Subscriber, IPAddress IPAddress, string CommunityString) : ISubscription;

        private void handleTrapV2Message(TrapV2Message message, IPAddress remoteIpAddress)
        {
            IList<Variable> variables = message.Variables();
            /*if (variables.Count < 2)
                return;
            if (variables[1].Id != SNMP_TRAP_OID)
                return;
            if (variables[1].Data is not ObjectIdentifier snmpTrapOID)
                return;*/
            if (!_subscriptionsV2.TryGetValue(message.Enterprise, out List<SubscriptionV2> subscriptionsForTrap))
                return;
            checkSubscriptionsForMessage(message, remoteIpAddress, subscriptionsForTrap, variables);
        }

        private static readonly ObjectIdentifier SNMP_TRAP_OID = new("1.3.6.1.6.3.1.1.4.1.0");
        #endregion

        private class MyLogger : ILogger
        {
            public void Log(ISnmpContext context) { }
        }


    }
}
