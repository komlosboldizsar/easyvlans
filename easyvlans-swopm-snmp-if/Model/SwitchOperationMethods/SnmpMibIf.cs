using BToolbox.Helpers;
using BToolbox.SNMP;
using BToolbox.XmlDeserializer.Context;
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
                => new ReadInterfaceStatusMethod(snmpConnection);

            protected override void subscribeTraps(ISnmpConnection snmpConnection, string[] trapFilter, object commonData)
            {
                bool subscribeDown = trapFilter.NullOrContains(TRAP_FILTER_LINK_DOWN);
                bool subscribeUp = trapFilter.NullOrContains(TRAP_FILTER_LINK_UP);
                TrapLinkUpOrDown trapSubscriber = null;
                if (subscribeDown || subscribeUp)
                    trapSubscriber = new(snmpConnection);
                if (subscribeDown)
                    snmpConnection.SubscribeForTrap(trapSubscriber, GenericTrapDescriptors.LINK_DOWN);
                if (subscribeUp)
                    snmpConnection.SubscribeForTrap(trapSubscriber, GenericTrapDescriptors.LINK_UP);
            }

        }

        private const string OID_IF_TABLE = "1.3.6.1.2.1.2.2.1";
        private const string OID_IF_INDEX = $"{OID_IF_TABLE}.1";
        private const string OID_IF_ADMIN_STATUS = $"{OID_IF_TABLE}.7";
        private const string OID_IF_OPER_STATUS = $"{OID_IF_TABLE}.8";
        private const string OID_IF_LAST_CHANGE = $"{OID_IF_TABLE}.9";

        private const string TRAP_FILTER_LINK_UP = "link_up";
        private const string TRAP_FILTER_LINK_DOWN = "link_down";

        private const string STRING_DOWN = "down";
        private const string STRING_UP = "up";
        private const string STRING_TESTING = "testing";
        private const string STRING_UNKNOWN = "unknown";

        private static readonly EnumConverter<int, PortStatus> ADMINISTRATIVE_STATUS_VALUES = new(PortStatus.Unknown)
        {
            { 1, PortStatus.Up },
            { 2, PortStatus.Down },
            { 3, PortStatus.Other }
        };

        private static readonly EnumToStringConverter<int> ADMINISTRATIVE_STATUS_STRINGS = new(STRING_UNKNOWN)
        {
            { 1, STRING_UP },
            { 2, STRING_DOWN },
            { 3, STRING_TESTING }
        };

        private static readonly EnumConverter<int, PortStatus> OPERATIONAL_STATUS_VALUES = new(PortStatus.Unknown)
        {
            { 1, PortStatus.Up },
            { 2, PortStatus.Down },
            { 3, PortStatus.Other },
            { 4, PortStatus.Unknown },
            { 5, PortStatus.Other },
            { 6, PortStatus.Other },
            { 7, PortStatus.Other }
        };

        private static readonly EnumToStringConverter<int> OPERATIONAL_STATUS_STRINGS = new(STRING_UNKNOWN)
        {
            { 1, STRING_UP },
            { 2, STRING_DOWN },
            { 3, STRING_TESTING },
            { 4, STRING_UNKNOWN },
            { 5, "dormant" },
            { 6, "not present" },
            { 7, "lower layer down" }
        };

        public static void UpdatePort(Port port, int adminStatus, int operStatus, DateTime? lastStatusChange = null)
        {
            port.AdministrativeStatus = ADMINISTRATIVE_STATUS_VALUES.Convert(adminStatus);
            port.AdministrativeStatusString = ADMINISTRATIVE_STATUS_STRINGS.Convert(adminStatus);
            port.OperationalStatus = OPERATIONAL_STATUS_VALUES.Convert(operStatus);
            port.OperationalStatusString = OPERATIONAL_STATUS_STRINGS.Convert(operStatus);
            port.LastStatusChange = lastStatusChange ?? DateTime.Now;
        }

    }

}
