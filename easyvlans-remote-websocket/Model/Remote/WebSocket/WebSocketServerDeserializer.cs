using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using easyvlans.Model.Remote.WebSocket;
using System.Xml;

namespace easyvlans.Model.Remote.WebSocket
{
    internal class MyWebSocketServerDeserializer : IRemoteMethodDeserializer
    {

        public string ElementName => ELEMENT_NAME;
        public const string ELEMENT_NAME = "websocket";

        public IRemoteMethod Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<Config> relationBuilder, object parent = null)
        {
            int port = (int)xmlNode.AttributeAsInt(ATTR_PORT, context).Default(8080).Min(1).Max(65535).Get().Value;
            MyWebSocketServer agent = new(port);
            relationBuilder = new RelationBuilder(agent);
            return agent;
        }

        private class RelationBuilder : IRelationBuilder<Config>
        {

            private readonly MyWebSocketServer _agent;

            public RelationBuilder(MyWebSocketServer agent)
                => _agent = agent;

            public void BuildRelations(Config config, DeserializationContext context)
                => _agent.MeetConfig(config);

        }

        private const string ATTR_PORT = "port";

    }

}
