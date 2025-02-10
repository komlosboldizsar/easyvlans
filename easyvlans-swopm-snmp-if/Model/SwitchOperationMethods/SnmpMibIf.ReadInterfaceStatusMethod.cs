using BToolbox.Helpers;
using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibIf
    {
        internal class ReadInterfaceStatusMethod : SnmpMethodBase, IReadInterfaceStatusMethod
        {

            public ReadInterfaceStatusMethod(ISnmpConnection snmpConnection, CommonData commonData)
                : base(snmpConnection)
                => _commonData = commonData;

            public override string MibName => MIB_NAME;
            private readonly CommonData _commonData;

            public async Task DoAsync(IEnumerable<Port> ports = null)
            {
                Dictionary<int, IfSnmpPort> snmpPorts = await readSnmpPortsAsync(ports);
                updateUserPorts(snmpPorts, ports);
            }

            private async Task<Dictionary<int, IfSnmpPort>> readSnmpPortsAsync(IEnumerable<Port> userPorts = null)
            {
                Dictionary<int, IfSnmpPort> snmpPorts = new();
                void processIfAdminStatus(IfSnmpPort p, Variable v) => v.ToInt(i => p.AdminStatus = i);
                void processIfOperStatus(IfSnmpPort p, Variable v) => v.ToInt(i => p.OperStatus = i);
                void processIfLastChange(IfSnmpPort p, Variable v) => v.ToUInt(i => p.LastChange = i);
                void processIfSpeed(IfSnmpPort p, Variable v) => v.ToUInt(i => p.InterfaceSpeed = i);

                if ((userPorts == null) && _commonData.OnlyForPorts)
                    userPorts = _snmpConnection.Switch.Ports;
                if (userPorts == null)
                {
                    async Task WaP(string oid, Action<IfSnmpPort, Variable> act) => await WalkAndProcess(oid, snmpPorts, id => new(id), act);
                    await WaP(OID_IF_ADMIN_STATUS, processIfAdminStatus);
                    await WaP(OID_IF_OPER_STATUS, processIfOperStatus);
                    await WaP(OID_IF_LAST_CHANGE, processIfLastChange);
                    await WaP(OID_IF_SPEED, processIfSpeed);
                }
                else
                {
                    List<string> oids = new();
                    foreach (Port userPort in userPorts)
                    {
                        if (userPort.Switch == _snmpConnection.Switch)
                        {
                            oids.Add($"{OID_IF_ADMIN_STATUS}.{userPort.Index + _commonData.PortIndexOffset}");
                            oids.Add($"{OID_IF_OPER_STATUS}.{userPort.Index + _commonData.PortIndexOffset}");
                            oids.Add($"{OID_IF_OPER_STATUS}.{userPort.Index + _commonData.PortIndexOffset}");
                            oids.Add($"{OID_IF_SPEED}.{userPort.Index + _commonData.PortIndexOffset}");
                        }
                    }
                    Action<string, Variable, IfSnmpPort> processIfTableRow = (nodeId, ifTableRow, snmpPort) =>
                    {
                        switch (nodeId)
                        {
                            case OID_IF_ADMIN_STATUS:
                                processIfAdminStatus(snmpPort, ifTableRow);
                                break;
                            case OID_IF_OPER_STATUS:
                                processIfOperStatus(snmpPort, ifTableRow);
                                break;
                            case OID_IF_LAST_CHANGE:
                                processIfLastChange(snmpPort, ifTableRow);
                                break;
                            case OID_IF_SPEED:
                                processIfSpeed(snmpPort, ifTableRow);
                                break;
                        }
                    };
                    TableProcessHelpers.ProcessTableRows(await _snmpConnection.GetAsync(oids), snmpPorts, id => new IfSnmpPort(id), processIfTableRow);
                }
                return snmpPorts;
            }

            private void updateUserPorts(Dictionary<int, IfSnmpPort> snmpPorts, IEnumerable<Port> userPorts = null)
            {
                foreach (Port userPort in userPorts ?? _snmpConnection.Switch.Ports)
                {
                    if (!snmpPorts.TryGetValue(userPort.Index + _commonData.PortIndexOffset, out IfSnmpPort snmpPort))
                    {
                        userPort.AdministrativeStatus = PortStatus.Unknown;
                        userPort.AdministrativeStatusString = STRING_UNKNOWN;
                        userPort.OperationalStatus = PortStatus.Unknown;
                        userPort.OperationalStatusString = STRING_UNKNOWN;
                        userPort.LastStatucChangeUpdateBootimeRelative(null);
                        userPort.Speed = null;
                        continue;
                    }
                    UpdatePort(userPort, snmpPort.AdminStatus, snmpPort.OperStatus, snmpPort.InterfaceSpeed, snmpPort.LastChange);
                }
            }

        }
    }
}
