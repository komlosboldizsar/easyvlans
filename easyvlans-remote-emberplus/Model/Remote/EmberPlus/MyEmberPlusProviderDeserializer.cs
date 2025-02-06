using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using easyvlans.Model.Remote.EmberPlus;
using System.Xml;

namespace easyvlans.Model.Remote.EmberPlus
{
    internal class MyEmberPlusProviderDeserializer : IRemoteMethodDeserializer
    {

        public string ElementName => ELEMENT_NAME;
        public const string ELEMENT_NAME = "snmp";

        public IRemoteMethod Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<Config> relationBuilder, object parent = null)
        {
            int port = (int)xmlNode.AttributeAsInt(ATTR_PORT, context).Default(161).Min(1).Max(65535).Get().Value;
            string identity = xmlNode.AttributeAsString(ATTR_IDENTITY, context).Get().Value;
            MyEmberPlusProvider agent = new(port, identity);
            relationBuilder = new RelationBuilder(agent);
            return agent;
        }

        private class RelationBuilder : IRelationBuilder<Config>
        {

            private readonly MyEmberPlusProvider _agent;

            public RelationBuilder(MyEmberPlusProvider agent)
                => _agent = agent;

            public void BuildRelations(Config config, DeserializationContext context)
                => _agent.MeetConfig(config);

        }

        private const string ATTR_PORT = "port";
        private const string ATTR_IDENTITY = "identity";
    }

}
