using B.XmlDeserializer.Attributes;
using B.XmlDeserializer.Context;
using B.XmlDeserializer.Relations;
using System.Text.RegularExpressions;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal abstract class SnmpV1V2SwitchOperationMethodCollectionBase : SnmpSwitchOperationMethodCollectionBase
    {

        public abstract class FactoryBase : ISwitchOperationMethodCollection.IDeserializer
        {

            public abstract string Code { get; }
            public string ElementName => Code;

            public ISwitchOperationMethodCollection Parse(XmlNode xmlNode, DeserializationContext context, out IRelationBuilder<Config> relationBuilder, object parent = null)
            {
                relationBuilder = null;
                XmlAttributeData<string> ipAddressAttribute = xmlNode.AttributeAsString(ATTR_IP, context).Mandatory().NotEmpty().Get();
                string ip = ipAddressAttribute.Value;
                if (!REGEXP_IP_ADDRESS.IsMatch(ip))
                    throw new AttributeValueInvalidException($"Invalid IP address.", ipAddressAttribute.Attribute);
                int port = (int)xmlNode.AttributeAsInt(ATTR_PORT, context).Default(161).Min(1).Max(65535).Get().Value;
                string communityString = xmlNode.AttributeAsString(ATTR_COMMUNITY_STRING, context).Mandatory().Get().Value;
                string accessVlanMembershipMethod = xmlNode.AttributeAsString(ATTR_METHOD_ACCESS_VLAN_MEMBERSHIP, context).Mandatory().Get().Value;
                string persistChangesMethod = xmlNode.AttributeAsString(ATTR_METHOD_PERSIST_CHANGES, context).Mandatory().Get().Value;
                return createInstance(parent as Switch, ip, port, communityString, accessVlanMembershipMethod, persistChangesMethod);
            }

            protected abstract ISwitchOperationMethodCollection createInstance(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, string persistChangesMethodName);

            private const string ATTR_IP = "ip";
            private const string ATTR_PORT = "port";
            private const string ATTR_COMMUNITY_STRING = "community_string";
            private const string ATTR_METHOD_ACCESS_VLAN_MEMBERSHIP = "method_access_vlan_membership";
            private const string ATTR_METHOD_PERSIST_CHANGES = "method_persist_changes";

            private readonly Regex REGEXP_IP_ADDRESS = new(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        }

        public SnmpV1V2SwitchOperationMethodCollectionBase(Switch @switch, string accessVlanMembershipMethodName, string persistChangesMethodName)
            : base(@switch, accessVlanMembershipMethodName, persistChangesMethodName) { }

    }
}
