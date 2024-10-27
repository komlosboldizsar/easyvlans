using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibQBridge
    {
        internal class ReadVlanMembershipMethod : MethodBase, IReadVlanMembershipMethod
        {

            public ReadVlanMembershipMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task DoAsync(IEnumerable<Port> ports = null)
            {
                Dictionary<int, QBridgeSnmpVlan> snmpVlans = await readSnmpVlansAsync();
                Dictionary<int, QBridgeSnmpPort> snmpPorts = await readSnmpPortsAsync(ports);
                bindUserToSnmpVlans(snmpVlans);
                calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts, ports);
            }

            public async Task<Dictionary<int, QBridgeSnmpVlan>> readSnmpVlansAsync()
            {
                Dictionary<int, QBridgeSnmpVlan> snmpVlans = new();
                foreach (Variable portVlanStaticTableRow in await _snmpConnection.WalkAsync(OID_DOT1Q_VLAN_STATIC_TABLE))
                {
                    SnmpVariableHelpers.IdParts idParts = portVlanStaticTableRow.GetIdParts();
                    QBridgeSnmpVlan snmpVlan = snmpVlans.GetAnyway(idParts.RowId, id => new QBridgeSnmpVlan(id));
                    switch (idParts.NodeId)
                    {
                        case OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS:
                            snmpVlan.EgressPorts = (portVlanStaticTableRow.Data as OctetString).GetRaw();
                            break;
                        case OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS:
                            snmpVlan.UntaggedPorts = (portVlanStaticTableRow.Data as OctetString).GetRaw();
                            break;
                    }
                }
                return snmpVlans;
            }

            public void bindUserToSnmpVlans(Dictionary<int, QBridgeSnmpVlan> snmpVlans)
            {
                foreach (Vlan userVlan in _snmpConnection.Switch.Config.Vlans.Values)
                    if (snmpVlans.TryGetValue(userVlan.ID, out QBridgeSnmpVlan snmpVlan))
                        snmpVlan.UserVlan = userVlan;
            }

            public async Task<Dictionary<int, QBridgeSnmpPort>> readSnmpPortsAsync(IEnumerable<Port> userPorts = null)
            {
                Dictionary<int, QBridgeSnmpPort> snmpPorts = new();
                void processQBridgePortPvid(QBridgeSnmpPort p, Variable v) => v.ToInt(i => p.PVID = i);
                if (userPorts == null)
                {
                    await WalkAndProcess(OID_DOT1Q_PVID, snmpPorts, id => new(id), processQBridgePortPvid);
                }
                else
                {
                    List<string> oids = new();
                    foreach (Port userPort in userPorts)
                        if (userPort.Switch == _snmpConnection.Switch)
                            oids.Add($"{OID_DOT1Q_PVID}.{userPort.Index + _commonData.PortIndexOffset}");
                    Action<string, Variable, QBridgeSnmpPort> processQBridgePortVlanTableRow = (nodeId, qBridgePortVlanTableRow, snmpPort) =>
                    {
                        switch (nodeId)
                        {
                            case OID_DOT1Q_PVID:
                                processQBridgePortPvid(snmpPort, qBridgePortVlanTableRow);
                                break;
                        }
                    };
                    TableProcessHelpers.ProcessTableRows(await _snmpConnection.GetAsync(oids), snmpPorts, id => new QBridgeSnmpPort(id), processQBridgePortVlanTableRow);
                }
                return snmpPorts;
            }

            public void calculateSnmpPortVlanMemberships(Dictionary<int, QBridgeSnmpVlan> snmpVlans, Dictionary<int, QBridgeSnmpPort> snmpPorts, IEnumerable<Port> userPorts)
            {
                foreach (Port userPort in _snmpConnection.Switch.Ports)
                {
                    if ((userPorts != null) && !userPorts.Contains(userPort))
                        continue;
                    if (!snmpPorts.TryGetValue(userPort.Index + _commonData.PortIndexOffset, out QBridgeSnmpPort snmpPort))
                    {
                        userPort.CurrentVlan = null;
                        continue;
                    }
                    (int portByteIndex, int portBitIndex) = getByteBitIndex(userPort.Index + _commonData.PortIndexOffset);
                    int ownerVlans = 0;
                    QBridgeSnmpVlan lastOwnerSnmpVlan = null;
                    foreach (QBridgeSnmpVlan snmpVlan in snmpVlans.Values)
                    {
                        bool isUntagged = snmpVlan.UntaggedPorts.GetBit(portByteIndex, portBitIndex);
                        bool isEgress = snmpVlan.EgressPorts.GetBit(portByteIndex, portBitIndex);
                        if (isUntagged && isEgress)
                        {
                            ownerVlans++;
                            lastOwnerSnmpVlan = snmpVlan;
                        }
                    }
                    if ((ownerVlans == 1) && ((lastOwnerSnmpVlan?.ID == snmpPort.PVID) || _commonData.NoPvid))
                    {
                        userPort.CurrentVlan = lastOwnerSnmpVlan.UserVlan;
                        userPort.HasComplexMembership = false;
                    }
                    else
                    {
                        userPort.CurrentVlan = null;
                        if ((ownerVlans > 1) || ((lastOwnerSnmpVlan?.ID != snmpPort.PVID) && !_commonData.NoPvid))
                            userPort.HasComplexMembership = true;
                    }
                }
            }

        }
    }
}
