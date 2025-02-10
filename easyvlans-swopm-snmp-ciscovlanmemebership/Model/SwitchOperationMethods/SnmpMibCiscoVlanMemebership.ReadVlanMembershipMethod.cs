using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibCiscoVlanMemebership
    {
        internal class ReadVlanMembershipMethod : MethodBase, IReadVlanMembershipMethod
        {

            public ReadVlanMembershipMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task DoAsync(IEnumerable<Port> ports = null)
            { 
                Dictionary<int, CiscoVlanMemebershipSnmpPort> snmpPorts = await readSnmpPortsAsync(ports);
                calculateSnmpPortVlanMemberships(snmpPorts, ports);
            }

            public async Task<Dictionary<int, CiscoVlanMemebershipSnmpPort>> readSnmpPortsAsync(IEnumerable<Port> userPorts = null)
            {
                Dictionary<int, CiscoVlanMemebershipSnmpPort> snmpPorts = new();
                void processCiscoVlanMembershipPortVlan(CiscoVlanMemebershipSnmpPort p, Variable v) => v.ToInt(i => p.VLAN = i);
                void processCiscoVlanMembershipPortType(CiscoVlanMemebershipSnmpPort p, Variable v) => v.ToInt(i => p.TYPE = i);
                if (userPorts == null)
                {
                    await WalkAndProcess(OID_CISCOVLANMEMEBERSHIP_TABLE_VLAN, snmpPorts, id => new(id), processCiscoVlanMembershipPortVlan);
                }
                else
                {
                    List<string> oids = new();
                    foreach (Port userPort in userPorts)
                        if (userPort.Switch == _snmpConnection.Switch)
                        {
                            oids.Add($"{OID_CISCOVLANMEMEBERSHIP_TABLE_VLAN}.{userPort.Index + _commonData.PortIndexOffset}");
                            oids.Add($"{OID_CISCOVLANMEMEBERSHIP_TABLE_TYPE}.{userPort.Index + _commonData.PortIndexOffset}");
                        }
                    Action<string, Variable, CiscoVlanMemebershipSnmpPort> processCiscoVlanMembershipTableRow = (nodeId, ciscoVlanMembershipTableRow, snmpPort) =>
                    {
                        switch (nodeId)
                        {
                            case OID_CISCOVLANMEMEBERSHIP_TABLE_VLAN:
                                processCiscoVlanMembershipPortVlan(snmpPort, ciscoVlanMembershipTableRow);
                                break;
                            case OID_CISCOVLANMEMEBERSHIP_TABLE_TYPE:
                                processCiscoVlanMembershipPortType(snmpPort, ciscoVlanMembershipTableRow);
                                break;
                        }
                    };
                    TableProcessHelpers.ProcessTableRows(await _snmpConnection.GetAsync(oids), snmpPorts, id => new CiscoVlanMemebershipSnmpPort(id), processCiscoVlanMembershipTableRow);
                }
                return snmpPorts;
            }
            private Vlan getVlanById(int id)
            {
                foreach (Vlan userVlan in _snmpConnection.Switch.Config.Vlans.Values)
                    if (userVlan.ID == id)
                        return userVlan;
                return null;
            }
            public void calculateSnmpPortVlanMemberships(Dictionary<int, CiscoVlanMemebershipSnmpPort> snmpPorts, IEnumerable<Port> userPorts)
            {
                foreach (Port userPort in _snmpConnection.Switch.Ports)
                {
                    if ((userPorts != null) && !userPorts.Contains(userPort))
                        continue;
                    if (!snmpPorts.TryGetValue(userPort.Index + _commonData.PortIndexOffset, out CiscoVlanMemebershipSnmpPort snmpPort))
                    {
                        userPort.CurrentVlan = null;
                        continue;
                    }
                  
                        userPort.CurrentVlan = getVlanById(snmpPort.ID);
                        userPort.HasComplexMembership = snmpPort.TYPE == 1 && snmpPort.VLAN != 0 ? true : false;
                }
            }

        }
    }
}
