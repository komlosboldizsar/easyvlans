using BToolbox.XmlDeserializer.Attributes;
using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibCiscoVlanMemebership : ISnmpMib
    {

        public const string MIB_NAME = "ciscovlanmembership";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override object createCommonData(XmlNode xmlNode, DeserializationContext context)
            {
                string setMembershipVariant = xmlNode.SelectSingleNode(DATA_TAG_SET_MEMBERSHIP_VARIANT)?.InnerText;
                if (setMembershipVariant == null)
                {
                    context.Report(DeserializationReportSeverity.Info, xmlNode, "No variant defined for setting membership, using the default one.");
                }
                else
                {
                    if (!SetPortToVlanMethod.VARIANTS.TryGetValue(setMembershipVariant, out _))
                    {
                        context.Report(DeserializationReportSeverity.Warning, xmlNode, "Variant defined for setting membership not found, using the default one.");
                        setMembershipVariant = null;
                    }
                }
                return new CommonData()
                {
                    //NoPvid = (xmlNode.SelectNodes(DATA_TAG_NO_PVID).Count > 0),
                    SetMembershipVariant = setMembershipVariant,
                    PortIndexOffset = xmlNode.SelectSingleNode(DATA_TAG_PORT_INDEX_OFFSET)?.InnerAsInt(context).Min(0).Get().Value ?? 0
                };
            }

            //public const string DATA_TAG_NO_PVID = "nopvid";
            public const string DATA_TAG_PORT_INDEX_OFFSET = "port_index_offset";
            public const string DATA_TAG_SET_MEMBERSHIP_VARIANT = "set_membership_variant";

            protected override IReadVlanMembershipMethod createReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
                => new ReadVlanMembershipMethod(snmpConnection, commonData);

            protected override ISetPortToVlanMethod createSetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
                => new SetPortToVlanMethod(snmpConnection, commonData);

        }

        internal class MethodBase : SnmpMethodBase
        {

            public override string MibName => MIB_NAME;

            protected readonly CommonData _commonData;

            public MethodBase(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection)
                => _commonData = (CommonData)commonData;

            protected static (int, int) getByteBitIndex(int portIndex) => ((portIndex - 1) / 8, 7 - (portIndex - 1) % 8);

        }

        internal class CommonData
        {
            //public bool NoPvid { get; init; }
            public string SetMembershipVariant { get; init; }
            public int PortIndexOffset { get; init; }
        }

        private const string OID_CISCOVLANMEMEBERSHIP_MEMBERSHIP_TABLE = "1.3.6.1.4.1.9.9.68.1.2.2";
        private const string OID_CISCOVLANMEMEBERSHIP_TABLE_TYPE = $"{OID_CISCOVLANMEMEBERSHIP_MEMBERSHIP_TABLE}.1.1";
        private const string OID_CISCOVLANMEMEBERSHIP_TABLE_VLAN = $"{OID_CISCOVLANMEMEBERSHIP_MEMBERSHIP_TABLE}.1.2";

    }

}
