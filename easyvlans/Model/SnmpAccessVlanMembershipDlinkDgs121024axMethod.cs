using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121024axMethod : ISnmpAccessVlanMembershipMethod
    {

        public const string CODE = "dlinkdgs121024ax";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121024axMethod(parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;
        public SnmpAccessVlanMembershipDlinkDgs121024axMethod(ISnmpSwitchOperationMethodCollection parent) => _parent = parent;
        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IReadConfigMethod.DoAsync()
        {
            Dictionary<int, SnmpVlan> snmpVlans = await readSnmpVlansAsync();
            Dictionary<int, SnmpPort> snmpPorts = await readSnmpPortsAsync();
            bindUserToSnmpVlans(snmpVlans);
            calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
        }

        private async Task<Dictionary<int, SnmpVlan>> readSnmpVlansAsync()
        {
            Dictionary<int, SnmpVlan> snmpVlans = new();
            foreach (Variable portVlanStaticTableRow in await _parent.SnmpConnection.BulkWalkAsync(OID_DOT1Q_VLAN))
            {
                SnmpVariableHelpers.IdParts idParts = portVlanStaticTableRow.GetIdParts();
                SnmpVlan snmpVlan = snmpVlans.GetAnyway(idParts.RowId, id => new SnmpVlan(id));
                switch (idParts.NodeId)
                {
                    case OID_DOT1Q_VLAN_ALL_MEMBERSHIP:
                        snmpVlan.EgressPorts = (portVlanStaticTableRow.Data as OctetString).GetRaw();
                        break;
                    case OID_DOT1Q_VLAN_UNTAGGED_MEMBERSHIP:
                        snmpVlan.UntaggedPorts = (portVlanStaticTableRow.Data as OctetString).GetRaw();
                        break;
                }
            }
            return snmpVlans;
        }

        private const string OID_DOT1Q_VLAN = "1.3.6.1.4.1.171.10.76.10.7.6.1";
        private const string OID_DOT1Q_VLAN_ALL_MEMBERSHIP = "1.3.6.1.4.1.171.10.76.10.7.6.1.2";
        private const string OID_DOT1Q_VLAN_UNTAGGED_MEMBERSHIP = "1.3.6.1.4.1.171.10.76.10.7.6.1.4";

        private void bindUserToSnmpVlans(Dictionary<int, SnmpVlan> snmpVlans)
        {
            foreach (Vlan userVlan in _parent.Switch.Config.Vlans.Values)
                if (snmpVlans.TryGetValue(userVlan.ID, out SnmpVlan snmpVlan))
                    snmpVlan.UserVlan = userVlan;
        }

        private async Task<Dictionary<int, SnmpPort>> readSnmpPortsAsync()
        {
            Dictionary<int, SnmpPort> snmpPorts = new();
            foreach (Variable portVlanTableRow in await _parent.SnmpConnection.BulkWalkAsync(OID_DOT1Q_PVID))
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

        private const string OID_DOT1Q_PVID = "1.3.6.1.4.1.171.10.76.10.7.7.1.1";

        private void calculateSnmpPortVlanMemberships(Dictionary<int, SnmpVlan> snmpVlans, Dictionary<int, SnmpPort> snmpPorts)
        {
            foreach (Port userPort in _parent.Switch.Ports)
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

        async Task<bool> ISetPortToVlanMethod.DoAsync(Port port, Vlan vlan)
        {
            List<Variable> egressSet = new(), egressClear = new(), untaggedSet = new(), untaggedClear = new(), pvidValue = new()
            {
                new Variable(new ObjectIdentifier($"{OID_DOT1Q_PVID}.{port.Index}"), new Gauge32(vlan.ID))
            };
            (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_ALL_MEMBERSHIP, vlan.ID, portByteIndex, portBitIndex, egressClear, egressSet);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_UNTAGGED_MEMBERSHIP, vlan.ID, portByteIndex, portBitIndex, untaggedClear, untaggedSet);
            await _parent.SnmpConnection.SetAsync(pvidValue);
            await _parent.SnmpConnection.SetAsync(untaggedClear);
            await _parent.SnmpConnection.SetAsync(egressClear);
            await _parent.SnmpConnection.SetAsync(egressSet);
            await _parent.SnmpConnection.SetAsync(untaggedSet);
            LogDispatcher.I($"Setting membership of port [{port.Label}] @ switch [{_parent.Switch.Label}] to VLAN [{vlan.Label}] ready.");
            return true;
        }

        private async Task getVlansBitfieldsForPort(string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesClear, List<Variable> variablesSet)
        {
            foreach (Variable oldRow in await _parent.SnmpConnection.BulkWalkAsync(tableObjectIdentifier))
            {
                SnmpVariableHelpers.IdParts idParts = oldRow.GetIdParts();
                bool valueToSet = (idParts.RowId == targetVlanId);
                byte[] snmpDataBytes = (oldRow.Data as OctetString).GetRaw();
                snmpDataBytes.SetBit(portByteIndex, portBitIndex, valueToSet);
                Variable newRow = new(oldRow.Id, new OctetString(snmpDataBytes));
                (valueToSet ? variablesSet : variablesClear).Add(newRow);
            }
        }

    }

}
