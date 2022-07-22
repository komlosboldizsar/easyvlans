using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    internal sealed class AccessVlanMembershipQSwitchMibMethod : MethodBase, IAccessVlanMembershipMethod
    {

        public string Name => "qswitchmib";

        public async Task ReadConfigAsync()
        {
            Dictionary<int, SnmpVlan> snmpVlans = await ReadSnmpVlansAsync();
            Dictionary<int, SnmpPort> snmpPorts = await ReadSnmpPortsAsync();
            BindUserToSnmpVlans(snmpVlans);
            CalculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
        }

        public async Task<Dictionary<int, SnmpVlan>> ReadSnmpVlansAsync()
        {
            Dictionary<int, SnmpVlan> snmpVlans = new();
            foreach (Variable portVlanStaticTableRow in await Switch.SnmpBulkWalkAsync(OID_DOT1Q_VLAN_STATIC_TABLE))
            {
                SnmpVariableHelpers.IdParts idParts = portVlanStaticTableRow.GetIdParts();
                SnmpVlan snmpVlan = snmpVlans.GetAnyway(idParts.RowId, id => new SnmpVlan(id));
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

        private const string OID_DOT1Q_VLAN_STATIC_TABLE = "1.3.6.1.2.1.17.7.1.4.3";
        private const string OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS = "1.3.6.1.2.1.17.7.1.4.3.1.2";
        private const string OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS = "1.3.6.1.2.1.17.7.1.4.3.1.4";

        public void BindUserToSnmpVlans(Dictionary<int, SnmpVlan> snmpVlans)
        {
            foreach (Vlan userVlan in Switch.Config.Vlans.Values)
                if (snmpVlans.TryGetValue(userVlan.ID, out SnmpVlan snmpVlan))
                    snmpVlan.UserVlan = userVlan;
        }

        public async Task<Dictionary<int, SnmpPort>> ReadSnmpPortsAsync()
        {
            Dictionary<int, SnmpPort> snmpPorts = new();
            foreach (Variable portVlanTableRow in await Switch.SnmpBulkWalkAsync(OID_DOT1Q_PORT_VLAN_TABLE))
            {
                SnmpVariableHelpers.IdParts idParts = portVlanTableRow.GetIdParts();
                SnmpPort snmpPort = snmpPorts.GetAnyway(idParts.RowId, id => new SnmpPort(id));
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

        private const string OID_DOT1Q_PORT_VLAN_TABLE = "1.3.6.1.2.1.17.7.1.4.5";
        private const string OID_DOT1Q_PVID = "1.3.6.1.2.1.17.7.1.4.5.1.1";

        public void CalculateSnmpPortVlanMemberships(Dictionary<int, SnmpVlan> snmpVlans, Dictionary<int, SnmpPort> snmpPorts)
        {
            foreach (Port userPort in Switch.Ports)
            {
                if (!snmpPorts.TryGetValue(userPort.Index, out SnmpPort snmpPort))
                {
                    userPort.CurrentVlan = null;
                    continue;
                }
                (int portByteIndex, int portBitIndex) = getByteBitIndex(userPort.Index);
                int ownerVlans = 0;
                SnmpVlan lastOwnerSnmpVlan = null;
                foreach (SnmpVlan snmpVlan in snmpVlans.Values)
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

        private (int, int) getByteBitIndex(int portIndex) => (((portIndex - 1) / 8), (7 - ((portIndex - 1) % 8)));

        public async Task<bool> SetPortToVlanAsync(Port port, Vlan vlan)
        {
            List<Variable> variablesLast = new(), variablesFirst = new() {
                new Variable(new ObjectIdentifier($"{OID_DOT1Q_PVID}.{port.Index}"), new Gauge32(vlan.ID))
            };
            (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS, vlan.ID, portByteIndex, portBitIndex, variablesFirst, variablesLast);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS, vlan.ID, portByteIndex, portBitIndex, variablesFirst, variablesLast);
            variablesFirst.AddRange(variablesLast);
            await Switch.SnmpSetAsync(variablesFirst);
            LogDispatcher.I($"Setting membership of port [{port.Label}] @ switch [{Switch.Label}] to VLAN [{vlan.Label}] ready.");
            return true;
        }

        private async Task getVlansBitfieldsForPort(string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesFirst, List<Variable> variablesLast)
        {
            foreach (Variable oldRow in await Switch.SnmpBulkWalkAsync(tableObjectIdentifier))
            {
                SnmpVariableHelpers.IdParts idParts = oldRow.GetIdParts();
                bool valueToSet = (idParts.RowId == targetVlanId);
                byte[] snmpDataBytes = (oldRow.Data as OctetString).GetRaw();
                snmpDataBytes.SetBit(portByteIndex, portBitIndex, valueToSet);
                Variable newRow = new(oldRow.Id, new OctetString(snmpDataBytes));
                (valueToSet ? variablesLast : variablesFirst).Add(newRow);
            }
        }

    }

}
