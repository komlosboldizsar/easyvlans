using B.XmlDeserializer.Context;
using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibDlinkDgs1210
    {
        internal class ReadConfigMethod : MethodBase, IReadConfigMethod
        {

            public ReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task DoAsync()
            {
                Dictionary<int, Dgs1210SnmpVlan> snmpVlans = await readSnmpVlansAsync();
                Dictionary<int, Dgs1210SnmpPort> snmpPorts = await readSnmpPortsAsync();
                bindUserToSnmpVlans(snmpVlans);
                calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
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

            private async Task<Dictionary<int, Dgs1210SnmpPort>> readSnmpPortsAsync()
            {
                Dictionary<int, Dgs1210SnmpPort> snmpPorts = new();
                foreach (Variable portVlanTableRow in await _snmpConnection.WalkAsync(_oidsForModel.OID_DOT1Q_PORT_PVID))
                {
                    SnmpVariableHelpers.IdParts idParts = portVlanTableRow.GetIdParts();
                    Dgs1210SnmpPort snmpPort = snmpPorts.GetAnyway(idParts.RowId, id => new Dgs1210SnmpPort(id));
                    if (idParts.NodeId == _oidsForModel.OID_DOT1Q_PORT_PVID)
                    {
                        if (int.TryParse(portVlanTableRow.Data.ToString(), out int pvid))
                            snmpPort.PVID = pvid;
                    }
                }
                return snmpPorts;
            }

            private void calculateSnmpPortVlanMemberships(Dictionary<int, Dgs1210SnmpVlan> snmpVlans, Dictionary<int, Dgs1210SnmpPort> snmpPorts)
            {
                foreach (Port userPort in _snmpConnection.Switch.Ports)
                {
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
