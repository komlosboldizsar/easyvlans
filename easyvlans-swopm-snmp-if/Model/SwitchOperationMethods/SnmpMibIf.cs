using BToolbox.Helpers;
using BToolbox.SNMP;
using BToolbox.XmlDeserializer.Context;
using BToolbox.XmlDeserializer.Attributes;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibIf : ISnmpMib
    {

        public const string MIB_NAME = "if";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override IReadInterfaceStatusMethod createReadInterfaceStatusMethod(ISnmpConnection snmpConnection, object commonData)
                => new ReadInterfaceStatusMethod(snmpConnection, (CommonData)commonData);

            protected override object createCommonData(XmlNode xmlNode, DeserializationContext context)
                => new CommonData()
                {
                    OnlyForPorts = (xmlNode.SelectNodes(DATA_TAG_ONLY_FOR_PORTS).Count > 0),
                    PortIndexOffset = xmlNode.SelectSingleNode(DATA_TAG_PORT_INDEX_OFFSET)?.InnerAsInt(context).Min(0).Get().Value ?? 0
                };

            public const string DATA_TAG_ONLY_FOR_PORTS = "only_for_ports";
            public const string DATA_TAG_PORT_INDEX_OFFSET = "port_index_offset";

            protected override void subscribeTraps(ISnmpConnection snmpConnection, string[] trapFilter, object commonData)
            {
                bool subscribeDown = trapFilter.NullOrContains(TRAP_FILTER_LINK_DOWN);
                bool subscribeUp = trapFilter.NullOrContains(TRAP_FILTER_LINK_UP);
                TrapLinkUpOrDown trapSubscriber = null;
                if (subscribeDown || subscribeUp)
                    trapSubscriber = new(snmpConnection, (CommonData)commonData);
                if (subscribeDown)
                    snmpConnection.SubscribeForTrap(trapSubscriber, GenericTrapDescriptors.LINK_DOWN);
                if (subscribeUp)
                    snmpConnection.SubscribeForTrap(trapSubscriber, GenericTrapDescriptors.LINK_UP);
            }

        }

        internal class CommonData
        {
            public bool OnlyForPorts { get; init; }
            public int PortIndexOffset { get; init; }
        }

        private const string OID_IF_TABLE = "1.3.6.1.2.1.2.2.1";
        private const string OID_IF_INDEX = $"{OID_IF_TABLE}.1";
        private const string OID_IF_ADMIN_STATUS = $"{OID_IF_TABLE}.7";
        private const string OID_IF_OPER_STATUS = $"{OID_IF_TABLE}.8";
        private const string OID_IF_LAST_CHANGE = $"{OID_IF_TABLE}.9";
        private const string OID_IF_SPEED = $"{OID_IF_TABLE}.5";

        private const string TRAP_FILTER_LINK_UP = "link_up";
        private const string TRAP_FILTER_LINK_DOWN = "link_down";

        private const int ADMINISTRATIVE_STATUS_UP = 1;
        private const int ADMINISTRATIVE_STATUS_DOWN = 2;
        private const int ADMINISTRATIVE_STATUS_OTHER = 3;

        private const int OPERATIVE_STATUS_UP = 1;
        private const int OPERATIVE_STATUS_DOWN = 2;
        private const int OPERATIVE_STATUS_TESTING = 3;
        private const int OPERATIVE_STATUS_UNKNOWN = 4;
        private const int OPERATIVE_STATUS_DORMANT = 5;
        private const int OPERATIVE_STATUS_NOTPRESENT = 6;
        private const int OPERATIVE_STATUS_LOWERLAYERDOWN = 7;

        private const string STRING_DOWN = "down";
        private const string STRING_UP = "up";
        private const string STRING_TESTING = "testing";
        private const string STRING_UNKNOWN = "unknown";

        private static readonly EnumConverter<int, PortStatus> ADMINISTRATIVE_STATUS_VALUES = new(PortStatus.Unknown)
        {
            { ADMINISTRATIVE_STATUS_UP, PortStatus.Up },
            { ADMINISTRATIVE_STATUS_DOWN, PortStatus.Down },
            { ADMINISTRATIVE_STATUS_OTHER, PortStatus.Other }
        };

        private static readonly EnumToStringConverter<int> ADMINISTRATIVE_STATUS_STRINGS = new(STRING_UNKNOWN)
        {
            { ADMINISTRATIVE_STATUS_UP, STRING_UP },
            { ADMINISTRATIVE_STATUS_DOWN, STRING_DOWN },
            { ADMINISTRATIVE_STATUS_OTHER, STRING_TESTING }
        };

        private static readonly EnumConverter<int, PortStatus> OPERATIONAL_STATUS_VALUES = new(PortStatus.Unknown)
        {
            { OPERATIVE_STATUS_UP, PortStatus.Up },
            { OPERATIVE_STATUS_DOWN, PortStatus.Down },
            { OPERATIVE_STATUS_TESTING, PortStatus.Other },
            { OPERATIVE_STATUS_UNKNOWN, PortStatus.Unknown },
            { OPERATIVE_STATUS_DORMANT, PortStatus.Other },
            { OPERATIVE_STATUS_NOTPRESENT, PortStatus.Other },
            { OPERATIVE_STATUS_LOWERLAYERDOWN, PortStatus.Other }
        };

        private static readonly EnumToStringConverter<int> OPERATIONAL_STATUS_STRINGS = new(STRING_UNKNOWN)
        {
            { OPERATIVE_STATUS_UP, STRING_UP },
            { OPERATIVE_STATUS_DOWN, STRING_DOWN },
            { OPERATIVE_STATUS_TESTING, STRING_TESTING },
            { OPERATIVE_STATUS_UNKNOWN, STRING_UNKNOWN },
            { OPERATIVE_STATUS_DORMANT, "dormant" },
            { OPERATIVE_STATUS_NOTPRESENT, "not present" },
            { OPERATIVE_STATUS_LOWERLAYERDOWN, "lower layer down" }
        };

        public static void UpdatePort(Port port, int adminStatus, int operStatus, long? interfaceSpeed = null, uint? lastStatusChange = null)
        {
            port.AdministrativeStatus = ADMINISTRATIVE_STATUS_VALUES.Convert(adminStatus);
            port.AdministrativeStatusString = ADMINISTRATIVE_STATUS_STRINGS.Convert(adminStatus);
            port.OperationalStatus = OPERATIONAL_STATUS_VALUES.Convert(operStatus);
            port.OperationalStatusString = OPERATIONAL_STATUS_STRINGS.Convert(operStatus);
            port.Speed = ((operStatus == OPERATIVE_STATUS_UP) || (operStatus == OPERATIVE_STATUS_TESTING)) ? interfaceSpeed : null;
            if (lastStatusChange != null)
                port.LastStatucChangeUpdateBootimeRelative((lastStatusChange != null) ? (new TimeSpan((uint)lastStatusChange * 100L)) : null);
        }

    }

}
