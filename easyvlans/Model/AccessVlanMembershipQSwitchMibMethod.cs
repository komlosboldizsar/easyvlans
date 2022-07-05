using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    internal class AccessVlanMembershipQSwitchMibMethod : IAccessVlanMembershipMethod
    {

        public string Name => "qswitchmib";

        public async Task ReadConfigAsync(Switch @switch)
        {
            Dictionary<int, SnmpVlan> snmpVlans = await ReadSnmpVlansAsync(@switch);
            Dictionary<int, SnmpPort> snmpPorts = await ReadSnmpPortsAsync(@switch);
            BindUserToSnmpVlans(@switch, snmpVlans);
            CalculateSnmpPortVlanMemberships(@switch, snmpVlans, snmpPorts);
        }

        public async Task<Dictionary<int, SnmpVlan>> ReadSnmpVlansAsync(Switch @switch)
        {
            Dictionary<int, SnmpVlan> snmpVlans = new();
            List<Variable> portVlanStaticTable = await @switch.SnmpBulkWalkAsync(OID_DOT1Q_VLAN_STATIC_TABLE);
            foreach (Variable portVlanStaticTableRow in portVlanStaticTable)
            {
                SnmpVariableHelpers.IdParts idParts = portVlanStaticTableRow.GetIdParts();
                SnmpVlan snmpVlan = snmpVlans.GetAnyway(idParts.RowId, id => new SnmpVlan(id));
                switch (idParts.NodeId)
                {
                    case OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS:
                        snmpVlan.EgressPorts = portVlanStaticTableRow.Data.ToBytes().Skip(3).ToArray();
                        break;
                    case OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS:
                        snmpVlan.UntaggedPorts = portVlanStaticTableRow.Data.ToBytes().Skip(3).ToArray();
                        break;
                }
            }
            return snmpVlans;
        }

        private const string OID_DOT1Q_VLAN_STATIC_TABLE = "1.3.6.1.2.1.17.7.1.4.3";
        private const string OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS = "1.3.6.1.2.1.17.7.1.4.3.1.2";
        private const string OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS = "1.3.6.1.2.1.17.7.1.4.3.1.4";

        public void BindUserToSnmpVlans(Switch @switch, Dictionary<int, SnmpVlan> snmpVlans)
        {
            foreach (UserVlan userVlan in @switch.Config.Vlans.Values)
                if (snmpVlans.TryGetValue(userVlan.ID, out SnmpVlan snmpVlan))
                    snmpVlan.UserVlan = userVlan;
        }

        public async Task<Dictionary<int, SnmpPort>> ReadSnmpPortsAsync(Switch @switch)
        {
            Dictionary<int, SnmpPort> snmpPorts = new();
            List<Variable> portVlanTable = await @switch.SnmpBulkWalkAsync(OID_DOT1Q_PORT_VLAN_TABLE);
            foreach (Variable portVlanTableRow in portVlanTable)
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

        public void CalculateSnmpPortVlanMemberships(Switch @switch, Dictionary<int, SnmpVlan> snmpVlans, Dictionary<int, SnmpPort> snmpPorts)
        {
            foreach (UserPort userPort in @switch.Ports)
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
                if ((ownerVlans == 1) && (lastOwnerSnmpVlan.ID == snmpPort.PVID) && userPort.Vlans.Contains(lastOwnerSnmpVlan.UserVlan))
                    userPort.CurrentVlan = lastOwnerSnmpVlan.UserVlan;
                else
                    userPort.CurrentVlan = null;
            }
        }

        private (int, int) getByteBitIndex(int portIndex) => (((portIndex - 1) / 8), (7 - ((portIndex - 1) % 8)));

        public async Task<bool> SetPortToVlanAsync(Switch @switch, UserPort port, UserVlan vlan)
        {
            List<Variable> variablesFirst = new(), variablesLast = new();
            variablesFirst.Add(new Variable(new ObjectIdentifier($"{OID_DOT1Q_PVID}.{port.Index}"), new Gauge32(vlan.ID)));
            (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index);
            await getVlansBitfieldsForPort(@switch, OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS, vlan.ID, portByteIndex, portBitIndex, variablesFirst, variablesLast);
            await getVlansBitfieldsForPort(@switch, OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS, vlan.ID, portByteIndex, portBitIndex, variablesFirst, variablesLast);
            variablesFirst.AddRange(variablesLast);
            await @switch.SnmpSetAsync(variablesFirst);
            LogDispatcher.I($"Setting membership of port [{port.Label}] @ switch [{@switch.Label}] to VLAN [{vlan.Name}] ready.");
            return true;
        }

        private async Task getVlansBitfieldsForPort(Switch @switch, string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesFirst, List<Variable> variablesLast)
        {
            foreach (Variable oldRow in await @switch.SnmpBulkWalkAsync(tableObjectIdentifier))
            {
                SnmpVariableHelpers.IdParts idParts = oldRow.GetIdParts();
                bool valueToSet = idParts.RowId == targetVlanId;
                byte[] snmpDataBytes = oldRow.Data.ToBytes();
                snmpDataBytes.SetBit(portByteIndex + 3, portBitIndex, valueToSet);
                Variable newRow = new Variable(oldRow.Id, DataFactory.CreateSnmpData(snmpDataBytes));
                (valueToSet ? variablesLast : variablesFirst).Add(newRow);
            }
        }

    }

}
