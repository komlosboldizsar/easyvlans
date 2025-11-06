using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using easyvlans.Model.Remote.WebSocket;
using System.Xml;

namespace easyvlans.Model.Remote.WebSocket
{
    internal class WebSocketServerDeserializer : IRemoteMethodDeserializer
    {

        public string ElementName => ELEMENT_NAME;
        public const string ELEMENT_NAME = "websocket";

        public IRemoteMethod Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<Config> relationBuilder, object parent = null)
        {
            int port = (int)xmlNode.AttributeAsInt(ATTR_PORT, context).Default(9000).Min(1).Max(65535).Get().Value;
            string identity = xmlNode.AttributeAsString(ATTR_IDENTITY, context).Default("EasyVLANs").Get().Value;
            bool autoPersist = xmlNode.AttributeAsBool(ATTR_AUTO_PERSIST, context).Default(true).Get().Value;
            WebSocketServer agent = new(port, identity, autoPersist);
            relationBuilder = new RelationBuilder(agent);
            return agent;
        }

        private class RelationBuilder : IRelationBuilder<Config>
        {

            private readonly WebSocketServer _agent;

            public RelationBuilder(WebSocketServer agent)
                => _agent = agent;

            public void BuildRelations(Config config, DeserializationContext context)
                => _agent.MeetConfig(config);

        }

        private const string ATTR_PORT = "port";
        private const string ATTR_IDENTITY = "identity";
        private const string ATTR_AUTO_PERSIST = "auto_persist";

    }

}
