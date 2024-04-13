using BToolbox.SNMP;
using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace easyvlans.Model.Remote.Snmp
{
    internal class TrapReceiverDeserializer : ElementDeserializer<TrapReceiver, Config>
    {

        public override string ElementName => ELEMENT_NAME;
        public const string ELEMENT_NAME = "trap_receiver";

        protected override TrapReceiver createElement(XmlNode elementNode, DeserializationContext context, object parent)
        {
            List<string> filter = filterDeserializer.Parse(elementNode, context, out IRelationBuilder<Config> _);
            return new TrapReceiver()
            {
                IP = elementNode.AttributeAsString(ATTR_IP, context).Get().Value,
                Port = (int)elementNode.AttributeAsInt(ATTR_PORT, context).Default(162).Min(1).Max(65535).Get().Value,
                Version = elementNode.AttributeAsEnum<TrapSendingConfig.TrapReceiverVersion>(ATTR_VERSION, context)
                                     .Translation("v1", TrapSendingConfig.TrapReceiverVersion.V1)
                                     .Translation("v2", TrapSendingConfig.TrapReceiverVersion.V2)
                                     .Get().Value,
                Community = elementNode.AttributeAsString(ATTR_COMMUNITY, context).Get().Value,
                Filter = (filter.Count > 0) ? filter : null,
                SendMyIp = elementNode.AttributeAsBool(ATTR_SEND_MY_IP, context).Default(false).Get().Value
            };
        }

        private const string ATTR_IP = "ip";
        private const string ATTR_PORT = "port";
        private const string ATTR_VERSION = "version";
        private const string ATTR_COMMUNITY = "community";
        private const string ATTR_SEND_MY_IP = "send_my_ip";

        private static readonly SimpleListDeserializer<string, Config> filterDeserializer = new(ELEMENT_NAME, new StringValueDeserializer<Config>(TAG_FILTER));

        private const string TAG_FILTER = "filter";

    }
}
