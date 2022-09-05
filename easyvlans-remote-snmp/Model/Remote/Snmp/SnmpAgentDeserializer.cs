using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Context;
using B.XmlDeserializer.Relations;
using System.Xml;

namespace easyvlans.Model.Remote.Snmp
{
    internal class SnmpAgentDeserializer : IRemoteMethodDeserializer
    {

        public string ElementName => "snmp";

        public IRemoteMethod Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<Config> relationBuilder, object parent = null)
        {
            relationBuilder = null;
            int port = (int)xmlNode.AttributeAsInt(ATTR_PORT, context).Default(161).Min(1).Max(65535).Get().Value;
            string communityRead = xmlNode.AttributeAsString(ATTR_COMMUNITY_READ, context).Get().Value;
            string communityWrite = xmlNode.AttributeAsString(ATTR_COMMUNITY_WRITE, context).Get().Value;
            return new SnmpAgent(port, communityRead, communityWrite);
        }

        private const string ATTR_PORT = "port";
        private const string ATTR_COMMUNITY_READ = "community_read";
        private const string ATTR_COMMUNITY_WRITE = "community_write";

    }

}
