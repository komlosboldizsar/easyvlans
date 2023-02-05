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
                (string accessVlanMembershipMethodName, XmlNode accessVlanMembershipMethodData) = getMethodNameAndData(xmlNode, context, TAG_METHOD_ACCESS_VLAN_MEMBERSHIP);
                (string persistChangesMethodName, XmlNode persistChangesMethodData) = getMethodNameAndData(xmlNode, context, TAG_METHOD_PERSIST_CHANGES);
                return createInstance(parent as Switch, ip, port, communityString, accessVlanMembershipMethodName, accessVlanMembershipMethodData, persistChangesMethodName, persistChangesMethodData);
            }

            private (string, XmlNode) getMethodNameAndData(XmlNode parentNode, DeserializationContext context, string methodName)
            {
                string tag = TAG_PREFIX_METHOD + methodName;
                XmlNodeList nodes = parentNode.SelectNodes(tag);
                if (nodes.Count == 0)
                    return (null, null);
                XmlNode methodNode = nodes[0];
                if (nodes.Count > 1)
                    context.Report(DeserializationReportSeverity.Info, parentNode, $"Multiple <{tag}> found for switch, using the first one.");
                string mibName = methodNode.AttributeAsString(ATTR_METHOD_MIB, context).Mandatory().Get().Value;
                if (mibName == null)
                    return (null, null);
                return (mibName, nodes[0]);
            }

            protected abstract ISwitchOperationMethodCollection createInstance(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, XmlNode accessVlanMembershipMethodData, string persistChangesMethodName, XmlNode persistChangesMethodData);

            private const string ATTR_IP = "ip";
            private const string ATTR_PORT = "port";
            private const string ATTR_COMMUNITY_STRING = "community_string";
            private const string ATTR_METHOD_MIB = "mib";
            private const string TAG_PREFIX_METHOD = "method_";
            private const string TAG_METHOD_ACCESS_VLAN_MEMBERSHIP = "access_vlan_membership";
            private const string TAG_METHOD_PERSIST_CHANGES = "persist_changes";

            private readonly Regex REGEXP_IP_ADDRESS = new(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        }

        public SnmpV1V2SwitchOperationMethodCollectionBase(Switch @switch, string accessVlanMembershipMethodName, XmlNode accessVlanMembershipMethodData, string persistChangesMethodName, XmlNode persistChangesMethodData)
            : base(@switch, accessVlanMembershipMethodName, accessVlanMembershipMethodData, persistChangesMethodName, persistChangesMethodData) { }

    }
}
