using B.XmlDeserializer;
using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Context;
using B.XmlDeserializer.Relations;
using BToolbox.SNMP;
using easyvlans.Model.SwitchOperationMethods;
using System;
using System.Xml;

namespace easyvlans.Model.Remote.Snmp
{
    internal class MySnmpAgentDeserializer : IRemoteMethodDeserializer
    {

        public string ElementName => ELEMENT_NAME;
        public const string ELEMENT_NAME = "snmp";

        public IRemoteMethod Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<Config> relationBuilder, object parent = null)
        {
            int port = (int)xmlNode.AttributeAsInt(ATTR_PORT, context).Default(161).Min(1).Max(65535).Get().Value;
            string communityRead = xmlNode.AttributeAsString(ATTR_COMMUNITY_READ, context).Get().Value;
            string communityWrite = xmlNode.AttributeAsString(ATTR_COMMUNITY_WRITE, context).Get().Value;
            List<TrapReceiver> trapReceivers = trapReceiversDeserializer.Parse(xmlNode, context, out IRelationBuilder<Config> _);
            TrapSendingConfig trapSendingConfig = new();
            trapReceivers.ForEach(trd => trapSendingConfig.AddReceiver(trd.IP, trd.Port, trd.Version, trd.Community, trd.Filter, trd.SendMyIp));
            MySnmpAgent agent = new(port, communityRead, communityWrite, trapSendingConfig);
            relationBuilder = new RelationBuilder(agent);
            return agent;
        }

        private static readonly SimpleListDeserializer<TrapReceiver, Config> trapReceiversDeserializer = new(ELEMENT_NAME, new TrapReceiverDeserializer());

        private class RelationBuilder : IRelationBuilder<Config>
        {

            private readonly MySnmpAgent _agent;

            public RelationBuilder(MySnmpAgent agent)
                => _agent = agent;

            public void BuildRelations(Config config, DeserializationContext context)
                => _agent.MeetConfig(config);

        }

        private const string ATTR_PORT = "port";
        private const string ATTR_COMMUNITY_READ = "community_read";
        private const string ATTR_COMMUNITY_WRITE = "community_write";

    }

}
