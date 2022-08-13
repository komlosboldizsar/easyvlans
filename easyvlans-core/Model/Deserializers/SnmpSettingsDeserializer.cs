using B.XmlDeserializer;
using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.Deserializers
{

    internal class SnmpSettingsDeserializer : ElementDeserializer<Config.SnmpSettings, Config>
    {

        public override string ElementName => ConfigTagNames.SETTINGS_SNMP;

        protected override Config.SnmpSettings createElement(XmlNode elementNode, DeserializationContext context, object parent) => new()
        {
            Enabled = elementNode.AttributeAsBool(ATTR_ENABLED, context).Default(false).Get().Value,
            Port = (int)elementNode.AttributeAsInt(ATTR_PORT, context).Default(161).Min(1).Max(65535).Get().Value,
            CommunityRead = elementNode.AttributeAsString(ATTR_COMMUNITY_READ, context).Mandatory().NotEmpty().Get().Value,
            CommunityWrite = elementNode.AttributeAsString(ATTR_COMMUNITY_WRITE, context).Mandatory().NotEmpty().Get().Value
        };

        private const string ATTR_ENABLED = "enabled";
        private const string ATTR_PORT = "port";
        private const string ATTR_COMMUNITY_READ = "community_read";
        private const string ATTR_COMMUNITY_WRITE = "community_write";

    }

}
