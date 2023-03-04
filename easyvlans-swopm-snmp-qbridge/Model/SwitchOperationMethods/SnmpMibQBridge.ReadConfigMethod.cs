using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibQBridge
    {
        internal class ReadConfigMethod : MethodBase, IReadConfigMethod
        {

            public ReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task DoAsync()
            {
                Dictionary<int, QBridgeSnmpVlan> snmpVlans = await readSnmpVlansAsync();
                Dictionary<int, QBridgeSnmpPort> snmpPorts = await readSnmpPortsAsync();
                bindUserToSnmpVlans(snmpVlans);
                calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
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

            public async Task<Dictionary<int, QBridgeSnmpPort>> readSnmpPortsAsync()
            {
                Dictionary<int, QBridgeSnmpPort> snmpPorts = new();
                foreach (Variable portVlanTableRow in await _snmpConnection.WalkAsync(OID_DOT1Q_PORT_VLAN_TABLE))
                {
                    SnmpVariableHelpers.IdParts idParts = portVlanTableRow.GetIdParts();
                    QBridgeSnmpPort snmpPort = snmpPorts.GetAnyway(idParts.RowId, id => new QBridgeSnmpPort(id));
                    switch (idParts.NodeId)
                    {
                        case OID_DOT1Q_PVID:
                            if (int.TryParse(portVlanTableRow.Data.ToString(), out int pvid))
                                snmpPort.PVID = pvid;
                            break;
                    }
                }
                return snmpPorts;
            }

            public void calculateSnmpPortVlanMemberships(Dictionary<int, QBridgeSnmpVlan> snmpVlans, Dictionary<int, QBridgeSnmpPort> snmpPorts)
            {
                foreach (Port userPort in _snmpConnection.Switch.Ports)
                {
                    if (!snmpPorts.TryGetValue(userPort.Index, out QBridgeSnmpPort snmpPort))
                    {
                        userPort.CurrentVlan = null;
                        continue;
                    }
                    (int portByteIndex, int portBitIndex) = getByteBitIndex(userPort.Index);
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
