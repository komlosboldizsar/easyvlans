using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using easyvlans.Logger;
using System.Text.RegularExpressions;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    public abstract class SnmpSwitchOperationMethodCollectionDeserializerBase : IDeserializer<ISwitchOperationMethodCollection, Config>
    {

        public abstract string ElementName { get; }

        public SnmpSwitchOperationMethodCollectionDeserializerBase()
        {
            operationMethodsDeserializer = new(ElementName);
            deserializers.ForEach(d => operationMethodsDeserializer.Register(d));
        }

        public ISwitchOperationMethodCollection Parse(XmlNode elementNode, DeserializationContext context, out IRelationBuilder<Config> relationBuilder, object parent = null)
        {
            relationBuilder = null;
            XmlAttributeData<string> ipAddressAttribute = elementNode.AttributeAsString(ATTR_IP, context).Mandatory().NotEmpty().Get();
            string ip = ipAddressAttribute.Value;
            if (!REGEXP_IP_ADDRESS.IsMatch(ip))
                throw new AttributeValueInvalidException($"Invalid IP address.", ipAddressAttribute.Attribute);
            int port = (int)elementNode.AttributeAsInt(ATTR_PORT, context).Default(161).Min(1).Max(65535).Get().Value;
            string communityString = elementNode.AttributeAsString(ATTR_COMMUNITY_STRING, context).Mandatory().Get().Value;
            ISnmpConnection snmpConnection = createConnection(parent as Switch, ip, port, communityString);
            List<ISwitchOperationMethodCollection> operationMethods = operationMethodsDeserializer.ParseWithGivenParent(elementNode, context, out IRelationBuilder<Config> _, snmpConnection);
            ISwitchOperationMethodCollection finalOperationMethodCollection = MixedSwitchOperationMethodCollection.Create(operationMethods, out MixedSwitchOperationMethodCollection.MethodCounts methodCounts);
            reportMethodCount(elementNode, context, methodCounts.ReadInterfaceStatusMethodCount, "reading interface status");
            reportMethodCount(elementNode, context, methodCounts.ReadVlanMembershipMethodCount, "reading VLAN membership");
            reportMethodCount(elementNode, context, methodCounts.SetPortToVlanMethodCount, "setting VLAN membership");
            reportMethodCount(elementNode, context, methodCounts.PersistChangesMethodCount, "persisting changes", true);
            return finalOperationMethodCollection;
        }

        private static void reportMethodCount(XmlNode elementNode, DeserializationContext context, int methodCount, string methodPurpose, bool suppressWarning = false)
        {
            if ((methodCount == 0) && !suppressWarning)
                context.Report(DeserializationReportSeverity.Warning, elementNode, $"No MIB supporting {methodPurpose} found for switch.");
            if (methodCount > 1)
                context.Report(DeserializationReportSeverity.Info, elementNode, $"Multiple MIBs supporting {methodPurpose} found for switch, using the first one.");
        }

        protected abstract ISnmpConnection createConnection(Switch @switch, string ip, int port, string communityStrings);
        private readonly HeterogenousListDeserializer<ISwitchOperationMethodCollection, Config> operationMethodsDeserializer;

        private const string ATTR_IP = "ip";
        private const string ATTR_PORT = "port";
        private const string ATTR_COMMUNITY_STRING = "community_string";

        private readonly Regex REGEXP_IP_ADDRESS = new(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static void RegisterMibDeserializer(IDeserializer<ISwitchOperationMethodCollection, Config> deserializer)
        {
            deserializers.Add(deserializer);
            LogDispatcher.VV($"Registered XML deserializer for SNMP MIB: [{deserializer.ElementName}]");
        }

        private static readonly List<IDeserializer<ISwitchOperationMethodCollection, Config>> deserializers = new();

    }

}
