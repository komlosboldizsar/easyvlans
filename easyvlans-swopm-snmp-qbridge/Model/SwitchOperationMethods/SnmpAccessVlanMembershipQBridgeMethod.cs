using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipQBridgeMethod : ISnmpAccessVlanMembershipMethod
    {

        public const string CODE = "qbridge";
        public const string DATA_TAG_NO_PVID = "nopvid";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipQBridgeMethod(data, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;
        private bool _dataNoPvid = false;

        public SnmpAccessVlanMembershipQBridgeMethod(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
        {
            _parent = parent;
            _dataNoPvid = (data.SelectNodes(DATA_TAG_NO_PVID).Count > 0);
        }

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IReadConfigMethod.DoAsync()
        {
            Dictionary<int, QBridgeSnmpVlan> snmpVlans = await readSnmpVlansAsync();
            Dictionary<int, QBridgeSnmpPort> snmpPorts = await readSnmpPortsAsync();
            bindUserToSnmpVlans(snmpVlans);
            calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
        }

        public async Task<Dictionary<int, QBridgeSnmpVlan>> readSnmpVlansAsync()
        {
            Dictionary<int, QBridgeSnmpVlan> snmpVlans = new();
            foreach (Variable portVlanStaticTableRow in await _parent.SnmpConnection.WalkAsync(OID_DOT1Q_VLAN_STATIC_TABLE))
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

        private const string OID_DOT1Q_VLAN_STATIC_TABLE = "1.3.6.1.2.1.17.7.1.4.3";
        private const string OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS = $"{OID_DOT1Q_VLAN_STATIC_TABLE}.1.2";
        private const string OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS = $"{OID_DOT1Q_VLAN_STATIC_TABLE}.1.4";

        public void bindUserToSnmpVlans(Dictionary<int, QBridgeSnmpVlan> snmpVlans)
        {
            foreach (Vlan userVlan in _parent.Switch.Config.Vlans.Values)
                if (snmpVlans.TryGetValue(userVlan.ID, out QBridgeSnmpVlan snmpVlan))
                    snmpVlan.UserVlan = userVlan;
        }

        public async Task<Dictionary<int, QBridgeSnmpPort>> readSnmpPortsAsync()
        {
            Dictionary<int, QBridgeSnmpPort> snmpPorts = new();
            foreach (Variable portVlanTableRow in await _parent.SnmpConnection.WalkAsync(OID_DOT1Q_PORT_VLAN_TABLE))
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

        private const string OID_DOT1Q_PORT_VLAN_TABLE = "1.3.6.1.2.1.17.7.1.4.5";
        private const string OID_DOT1Q_PVID = $"{OID_DOT1Q_PORT_VLAN_TABLE}.1.1";

        public void calculateSnmpPortVlanMemberships(Dictionary<int, QBridgeSnmpVlan> snmpVlans, Dictionary<int, QBridgeSnmpPort> snmpPorts)
        {
            foreach (Port userPort in _parent.Switch.Ports)
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
                if (ownerVlans == 1 && ((lastOwnerSnmpVlan?.ID == snmpPort.PVID) || _dataNoPvid))
                {
                    userPort.CurrentVlan = lastOwnerSnmpVlan.UserVlan;
                    userPort.HasComplexMembership = false;
                }
                else
                {
                    userPort.CurrentVlan = null;
                    if (ownerVlans > 1 || ((lastOwnerSnmpVlan?.ID != snmpPort.PVID) && !_dataNoPvid))
                        userPort.HasComplexMembership = true;
                }
            }
        }

        private (int, int) getByteBitIndex(int portIndex) => ((portIndex - 1) / 8, 7 - (portIndex - 1) % 8);

        async Task<bool> ISetPortToVlanMethod.DoAsync(Port port, Vlan vlan)
        {
            List<Variable> pvidVariables = new(),
                egressToUnset = new(),
                egressToSet = new(),
                untaggedToUnset = new(),
                untaggedToSet = new();
            if (!_dataNoPvid)
                pvidVariables.Add(new Variable(new ObjectIdentifier($"{OID_DOT1Q_PVID}.{port.Index}"), new Gauge32(vlan.ID)));
            (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS, vlan.ID, portByteIndex, portBitIndex, egressToSet, egressToUnset);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS, vlan.ID, portByteIndex, portBitIndex, untaggedToSet, untaggedToUnset);
            await _parent.SnmpConnection.SetAsync(pvidVariables);
            await _parent.SnmpConnection.SetAsync(untaggedToUnset);
            await _parent.SnmpConnection.SetAsync(egressToSet);
            await _parent.SnmpConnection.SetAsync(egressToUnset);
            await _parent.SnmpConnection.SetAsync(untaggedToSet);
            return true;
        }

        private async Task getVlansBitfieldsForPort(string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesToSet, List<Variable> variablesToUnset)
        {
            foreach (Variable oldRow in await _parent.SnmpConnection.WalkAsync(tableObjectIdentifier))
            {
                SnmpVariableHelpers.IdParts idParts = oldRow.GetIdParts();
                bool valueToSet = idParts.RowId == targetVlanId;
                byte[] snmpDataBytes = (oldRow.Data as OctetString).GetRaw();
                snmpDataBytes.SetBit(portByteIndex, portBitIndex, valueToSet);
                Variable newRow = new(oldRow.Id, new OctetString(snmpDataBytes));
                (valueToSet ? variablesToSet : variablesToUnset).Add(newRow);
            }
        }

    }

}
