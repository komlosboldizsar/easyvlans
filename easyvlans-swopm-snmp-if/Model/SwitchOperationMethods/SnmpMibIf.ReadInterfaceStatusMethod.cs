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
                    UpdatePort(userPort, snmpPort.AdminStatus, snmpPort.OperStatus, new DateTime(snmpPort.LastChange * 100L));
                }
            }

        }
    }
}
