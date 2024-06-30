using BToolbox.XmlDeserializer;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Relations;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibTPLinkDot1qVlan : ISnmpMib
    {

        public const string MIB_NAME = "tplinkdot1qvlan";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override object createCommonData(XmlNode xmlNode, DeserializationContext context)
            {
                TypedCompositeDeserializer<Parameters, Parameters> parametersDeserializer = new(ElementName, () => new Parameters());
                SimpleListDeserializer<PortMapping, Parameters> portMappingDeserializer = new(PortMappingDeserializer.TAG_PORT_MAPPINGS, new PortMappingDeserializer());
                parametersDeserializer.Register(portMappingDeserializer, (parameters, portMappings) => parameters.PortMappings.AddRange(portMappings));
                Parameters parameters = parametersDeserializer.Parse(xmlNode, context, out IRelationBuilder<Parameters> relationBuilder, this);
                return new CommonData()
                {
                    PortMappings = new(parameters.PortMappings)
                };
            }

            internal class Parameters
            {
                public readonly List<PortMapping> PortMappings = new();
            }

            protected override IReadVlanMembershipMethod createReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
                => new ReadVlanMembershipMethod(snmpConnection, commonData);

            protected override ISetPortToVlanMethod createSetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
                => new SetPortToVlanMethod(snmpConnection, commonData);

        }

        internal class MethodBase : SnmpMethodBase
        {

            public override string MibName => MIB_NAME;

            protected readonly PortMappingCollection _portMappings;

            public MethodBase(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection)
            {
                _portMappings = ((CommonData)commonData).PortMappings;
            }

        }

        internal class CommonData
        {
            public PortMappingCollection PortMappings { get; init; }
        }

        private const string OID_VLAN_CONFIG_TABLE = "1.3.6.1.4.1.11863.6.14.1.2.1";
        private const string OID_VLAN_TAG_PORT_MEMBER_ADD = $"{OID_VLAN_CONFIG_TABLE}.1.3";
        private const string OID_VLAN_UNTAG_PORT_MEMBER_ADD = $"{OID_VLAN_CONFIG_TABLE}.1.4";
        private const string OID_VLAN_PORT_MEMBER_REMOVE = $"{OID_VLAN_CONFIG_TABLE}.1.5";
        private const string OID_VLAN_PORT_CONFIG_TABLE = "1.3.6.1.4.1.11863.6.14.1.1.1";
        private const string OID_VLAN_PORT_TYPE = $"{OID_VLAN_PORT_CONFIG_TABLE}.1.2";
        private const string OID_VLAN_PORT_PVID = $"{OID_VLAN_PORT_CONFIG_TABLE}.1.3";

    }

}
