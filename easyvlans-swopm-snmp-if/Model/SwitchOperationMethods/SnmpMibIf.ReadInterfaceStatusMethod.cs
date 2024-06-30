using BToolbox.Helpers;
using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibIf
    {
        internal class ReadInterfaceStatusMethod : SnmpMethodBase, IReadInterfaceStatusMethod
        {

            public ReadInterfaceStatusMethod(ISnmpConnection snmpConnection)
                : base(snmpConnection) { }

            public override string MibName => MIB_NAME;

            public async Task DoAsync()
            {
                Dictionary<int, IfSnmpPort> snmpPorts = await readSnmpPortsAsync();
                updateUserPorts(snmpPorts);
            }

            private async Task<Dictionary<int, IfSnmpPort>> readSnmpPortsAsync()
            {
                Dictionary<int, IfSnmpPort> snmpPorts = new();
                async Task WaP(string oid, Action<IfSnmpPort, Variable> act) => await WalkAndProcess(oid, snmpPorts, id => new(id), act);
                await WaP(OID_IF_ADMIN_STATUS, (p, v) => v.ToInt(i => p.AdminStatus = i));
                await WaP(OID_IF_OPER_STATUS, (p, v) => v.ToInt(i => p.OperStatus = i));
                await WaP(OID_IF_LAST_CHANGE, (p, v) => v.ToUInt(i => p.LastChange = i));
                return snmpPorts;
            }

            private void updateUserPorts(Dictionary<int, IfSnmpPort> snmpPorts)
            {
                foreach (Port userPort in _snmpConnection.Switch.Ports)
                {
                    if (!snmpPorts.TryGetValue(userPort.Index, out IfSnmpPort snmpPort))
                    {
                        userPort.AdministrativeStatus = PortStatus.Unknown;
                        userPort.AdministrativeStatusString = STRING_UNKNOWN;
                        userPort.OperationalStatus = PortStatus.Unknown;
                        userPort.OperationalStatusString = STRING_UNKNOWN;
                        userPort.LastStatusChange = null;
                        continue;
                    }
                    userPort.AdministrativeStatus = ADMINISTRATIVE_STATUS_VALUES.Convert(snmpPort.AdminStatus);
                    userPort.AdministrativeStatusString = ADMINISTRATIVE_STATUS_STRINGS.Convert(snmpPort.AdminStatus); ;
                    userPort.OperationalStatus = OPERATIONAL_STATUS_VALUES.Convert(snmpPort.OperStatus);
                    userPort.OperationalStatusString = OPERATIONAL_STATUS_STRINGS.Convert(snmpPort.OperStatus);
                    userPort.LastStatusChange = new DateTime(snmpPort.LastChange * 100L);
                }
            }

            private const string STRING_DOWN = "down";
            private const string STRING_UP = "up";
            private const string STRING_TESTING = "testing";
            private const string STRING_UNKNOWN = "unknown";

            private readonly EnumConverter<int, PortStatus> ADMINISTRATIVE_STATUS_VALUES = new(PortStatus.Unknown)
            {
                { 1, PortStatus.Up },
                { 2, PortStatus.Down },
                { 3, PortStatus.Other }
            };

            private readonly EnumToStringConverter<int> ADMINISTRATIVE_STATUS_STRINGS = new(STRING_UNKNOWN)
            {
                { 1, STRING_UP },
                { 2, STRING_DOWN },
                { 3, STRING_TESTING }
            };

            private readonly EnumConverter<int, PortStatus> OPERATIONAL_STATUS_VALUES = new(PortStatus.Unknown)
            {
                { 1, PortStatus.Up },
                { 2, PortStatus.Down },
                { 3, PortStatus.Other },
                { 4, PortStatus.Unknown },
                { 5, PortStatus.Other },
                { 6, PortStatus.Other },
                { 7, PortStatus.Other }
            };

            private readonly EnumToStringConverter<int> OPERATIONAL_STATUS_STRINGS = new(STRING_UNKNOWN)
            {
                { 1, STRING_UP },
                { 2, STRING_DOWN },
                { 3, STRING_TESTING },
                { 4, STRING_UNKNOWN },
                { 5, "dormant" },
                { 6, "not present" },
                { 7, "lower layer down" }
            };

        }
    }
}
