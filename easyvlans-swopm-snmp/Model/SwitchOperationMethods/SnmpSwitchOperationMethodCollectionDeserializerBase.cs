using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using BToolbox.Logger;
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

            // Connection basics
            string ip = elementNode.AttributeAsIPv4(ATTR_IP, context).Mandatory().Get().Value;
            int port = (int)elementNode.AttributeAsInt(ATTR_PORT, context).Default(161).Min(1).Max(65535).Get().Value;
            string communityString = elementNode.AttributeAsString(ATTR_COMMUNITY_STRING, context).Mandatory().Get().Value;
            
            // Traps
            int? trapPort = elementNode.AttributeAsInt(ATTR_TRAP_PORT, context).Min(1).Max(65535).Get().Value;
            string trapCommunityString = elementNode.AttributeAsString(ATTR_TRAP_COMMUNITY_STRING, context).Get().Value;
            bool trapVersionStrict = elementNode.AttributeAsBool(ATTR_TRAP_VERSION_STRICT, context).Default(true).Get().Value;

            // Create connection
            ISnmpConnection snmpConnection = createConnection(parent as Switch, ip, port, communityString, trapPort, trapCommunityString, trapVersionStrict);

            // MIBs
            List<ISwitchOperationMethodCollection> operationMethods = operationMethodsDeserializer.ParseWithGivenParent(elementNode, context, out IRelationBuilder<Config> _, snmpConnection);
            ISwitchOperationMethodCollection finalOperationMethodCollection = MixedSwitchOperationMethodCollection.Create(operationMethods, out MixedSwitchOperationMethodCollection.MethodCounts methodCounts);
            reportMethodCount(elementNode, context, methodCounts.ReadSwitchBoottimeMethodCount, "reading switch boot time");
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

        protected abstract ISnmpConnection createConnection(Switch @switch, string ip, int port, string communityStrings, int? trapPort, string trapCommunityString, bool trapVersionStrict);
        private readonly HeterogenousListDeserializer<ISwitchOperationMethodCollection, Config> operationMethodsDeserializer;

        private const string ATTR_IP = "ip";
        private const string ATTR_PORT = "port";
        private const string ATTR_COMMUNITY_STRING = "community_string";
        private const string ATTR_TRAP_PORT = "trap_port";
        private const string ATTR_TRAP_COMMUNITY_STRING = "trap_community_string";
        private const string ATTR_TRAP_VERSION_STRICT = "trap_version_strict";

        public static void RegisterMibDeserializer(IDeserializer<ISwitchOperationMethodCollection, Config> deserializer)
        {
            deserializers.Add(deserializer);
            LogDispatcher.VV($"Registered XML deserializer for SNMP MIB: [{deserializer.ElementName}]");
        }

        private static readonly List<IDeserializer<ISwitchOperationMethodCollection, Config>> deserializers = new();

    }

}
