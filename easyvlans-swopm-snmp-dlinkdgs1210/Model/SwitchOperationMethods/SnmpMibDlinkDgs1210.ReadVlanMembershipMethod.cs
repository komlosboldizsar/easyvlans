using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibDlinkDgs1210
    {
        internal class ReadVlanMembershipMethod : MethodBase, IReadVlanMembershipMethod
        {

            public ReadVlanMembershipMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task DoAsync(IEnumerable<Port> ports = null)
            {
                Dictionary<int, Dgs1210SnmpVlan> snmpVlans = await readSnmpVlansAsync();
                Dictionary<int, Dgs1210SnmpPort> snmpPorts = await readSnmpPortsAsync(ports);
                bindUserToSnmpVlans(snmpVlans);
                calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts, ports);
            }

            private async Task<Dictionary<int, Dgs1210SnmpVlan>> readSnmpVlansAsync()
            {
                Dictionary<int, Dgs1210SnmpVlan> snmpVlans = new();
                foreach (Variable portVlanStaticTableRow in await _snmpConnection.WalkAsync(_oidsForModel.OID_DOT1Q_VLAN))
                {
                    SnmpVariableHelpers.IdParts idParts = portVlanStaticTableRow.GetIdParts();
                    Dgs1210SnmpVlan snmpVlan = snmpVlans.GetAnyway(idParts.RowId, id => new Dgs1210SnmpVlan(id));
                    if (idParts.NodeId == _oidsForModel.OID_DOT1Q_VLAN_EGRESS_PORTS)
                    {
                        snmpVlan.EgressPorts = (portVlanStaticTableRow.Data as OctetString).GetRaw();
                    }
                    else if (idParts.NodeId == _oidsForModel.OID_DOT1Q_VLAN_UNTAGGED_PORTS)
                    {
                        snmpVlan.UntaggedPorts = (portVlanStaticTableRow.Data as OctetString).GetRaw();
                    }
                }
                return snmpVlans;
            }

            private void bindUserToSnmpVlans(Dictionary<int, Dgs1210SnmpVlan> snmpVlans)
            {
                foreach (Vlan userVlan in _snmpConnection.Switch.Config.Vlans.Values)
                    if (snmpVlans.TryGetValue(userVlan.ID, out Dgs1210SnmpVlan snmpVlan))
                        snmpVlan.UserVlan = userVlan;
            }

            private async Task<Dictionary<int, Dgs1210SnmpPort>> readSnmpPortsAsync(IEnumerable<Port> userPorts = null)
            {
                Dictionary<int, Dgs1210SnmpPort> snmpPorts = new();
                void processDgs1210PortPvid(Dgs1210SnmpPort p, Variable v) => v.ToInt(i => p.PVID = i);
                if (userPorts == null)
                {
                    await WalkAndProcess(_oidsForModel.OID_DOT1Q_PORT_PVID, snmpPorts, id => new(id), processDgs1210PortPvid);
                }
                else
                {
                    List<string> oids = new();
                    foreach (Port userPort in userPorts)
                        if (userPort.Switch == _snmpConnection.Switch)
                            oids.Add($"{_oidsForModel.OID_DOT1Q_PORT_PVID}.{userPort.Index}");
                    Action<string, Variable, Dgs1210SnmpPort> processDgs1210VlanPortTableRow = (nodeId, dgs1210PortTableRow, snmpPort) =>
                    {
                        if (nodeId == _oidsForModel.OID_DOT1Q_PORT_PVID)
                            processDgs1210PortPvid(snmpPort, dgs1210PortTableRow);
                    };
                }
                return snmpPorts;
            }

            private void calculateSnmpPortVlanMemberships(Dictionary<int, Dgs1210SnmpVlan> snmpVlans, Dictionary<int, Dgs1210SnmpPort> snmpPorts, IEnumerable<Port> userPorts = null)
            {
                foreach (Port userPort in _snmpConnection.Switch.Ports)
                {
                    if ((userPorts != null) && !userPorts.Contains(userPort))
                        continue;
                    if (!snmpPorts.TryGetValue(userPort.Index, out Dgs1210SnmpPort snmpPort))
                    {
                        userPort.CurrentVlan = null;
                        continue;
                    }
                    (int portByteIndex, int portBitIndex) = getByteBitIndex(userPort.Index);
                    int ownerVlans = 0;
                    Dgs1210SnmpVlan lastOwnerSnmpVlan = null;
                    foreach (Dgs1210SnmpVlan snmpVlan in snmpVlans.Values)
                    {
                        bool isUntagged = snmpVlan.UntaggedPorts.GetBit(portByteIndex, portBitIndex);
                        bool isEgress = snmpVlan.EgressPorts.GetBit(portByteIndex, portBitIndex);
                        if (isUntagged && isEgress)
                        {
                            ownerVlans++;
                            lastOwnerSnmpVlan = snmpVlan;
                        }
                    }
                    if ((ownerVlans == 1) && (lastOwnerSnmpVlan?.ID == snmpPort.PVID))
                    {
                        userPort.CurrentVlan = lastOwnerSnmpVlan.UserVlan;
                        userPort.HasComplexMembership = false;
                    }
                    else
                    {
                        userPort.CurrentVlan = null;
                        if ((ownerVlans > 1) || (lastOwnerSnmpVlan?.ID != snmpPort.PVID))
                            userPort.HasComplexMembership = true;
                    }
                }
            }

        }
    }
}
