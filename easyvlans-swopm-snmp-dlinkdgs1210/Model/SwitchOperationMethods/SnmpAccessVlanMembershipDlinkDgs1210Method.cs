using B.XmlDeserializer.Context;
using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal class SnmpAccessVlanMembershipDlinkDgs1210Method : ISnmpAccessVlanMembershipMethod
    {

        public const string CODE = "dlinkdgs1210";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs1210Method(data, deserializationContext, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;
        private readonly string _modelCode = "?";

        public SnmpAccessVlanMembershipDlinkDgs1210Method(XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent)
        {
            _parent = parent;
            Dgs1210Model model = Dgs1210Helpers.GetModel(data, deserializationContext);
            _modelCode = model.Code;
            Dgs1210Helpers.GenerateOid(ref OID_DOT1Q_VLAN, OID_TEMPLATE_DOT1Q_VLAN_ENTRY, model);
            Dgs1210Helpers.GenerateOid(ref OID_DOT1Q_VLAN_EGRESS_PORTS, OID_TEMPLATE_DOT1Q_VLAN_EGRESS_PORTS, model);
            Dgs1210Helpers.GenerateOid(ref OID_DOT1Q_VLAN_UNTAGGED_PORTS, OID_TEMPLATE_DOT1Q_VLAN_UNTAGGED_PORTS, model);
            Dgs1210Helpers.GenerateOid(ref OID_DOT1Q_PORT_PVID, OID_TEMPLATE_DOT1Q_PORT_PVID, model);
        }

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{Code}:{_modelCode}]";

        private const string OID_COMPANY_DOT1Q_VLAN_GROUP = "1.3.6.1.4.1.171.10.76.{0}.7";

        async Task IReadConfigMethod.DoAsync()
        {
            Dictionary<int, Dgs1210SnmpVlan> snmpVlans = await readSnmpVlansAsync();
            Dictionary<int, Dgs1210SnmpPort> snmpPorts = await readSnmpPortsAsync();
            bindUserToSnmpVlans(snmpVlans);
            calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
        }

        private async Task<Dictionary<int, Dgs1210SnmpVlan>> readSnmpVlansAsync()
        {
            Dictionary<int, Dgs1210SnmpVlan> snmpVlans = new();
            foreach (Variable portVlanStaticTableRow in await _parent.SnmpConnection.WalkAsync(OID_DOT1Q_VLAN))
            {
                SnmpVariableHelpers.IdParts idParts = portVlanStaticTableRow.GetIdParts();
                Dgs1210SnmpVlan snmpVlan = snmpVlans.GetAnyway(idParts.RowId, id => new Dgs1210SnmpVlan(id));
                if (idParts.NodeId == OID_DOT1Q_VLAN_EGRESS_PORTS)
                {
                    snmpVlan.EgressPorts = (portVlanStaticTableRow.Data as OctetString).GetRaw();
                }
                else if (idParts.NodeId == OID_DOT1Q_VLAN_UNTAGGED_PORTS)
                {
                    snmpVlan.UntaggedPorts = (portVlanStaticTableRow.Data as OctetString).GetRaw();
                }
            }
            return snmpVlans;
        }

        private const string OID_TEMPLATE_DOT1Q_VLAN_ENTRY = $"{OID_COMPANY_DOT1Q_VLAN_GROUP}.6.1";
        private const string OID_TEMPLATE_DOT1Q_VLAN_EGRESS_PORTS = $"{OID_TEMPLATE_DOT1Q_VLAN_ENTRY}.2";
        private const string OID_TEMPLATE_DOT1Q_VLAN_UNTAGGED_PORTS = $"{OID_TEMPLATE_DOT1Q_VLAN_ENTRY}.4";

        private readonly string OID_DOT1Q_VLAN;
        private readonly string OID_DOT1Q_VLAN_EGRESS_PORTS;
        private readonly string OID_DOT1Q_VLAN_UNTAGGED_PORTS;

        private void bindUserToSnmpVlans(Dictionary<int, Dgs1210SnmpVlan> snmpVlans)
        {
            foreach (Vlan userVlan in _parent.Switch.Config.Vlans.Values)
                if (snmpVlans.TryGetValue(userVlan.ID, out Dgs1210SnmpVlan snmpVlan))
                    snmpVlan.UserVlan = userVlan;
        }

        private async Task<Dictionary<int, Dgs1210SnmpPort>> readSnmpPortsAsync()
        {
            Dictionary<int, Dgs1210SnmpPort> snmpPorts = new();
            foreach (Variable portVlanTableRow in await _parent.SnmpConnection.WalkAsync(OID_DOT1Q_PORT_PVID))
            {
                SnmpVariableHelpers.IdParts idParts = portVlanTableRow.GetIdParts();
                Dgs1210SnmpPort snmpPort = snmpPorts.GetAnyway(idParts.RowId, id => new Dgs1210SnmpPort(id));
                if (idParts.NodeId == OID_DOT1Q_PORT_PVID)
                {
                    if (int.TryParse(portVlanTableRow.Data.ToString(), out int pvid))
                        snmpPort.PVID = pvid;
                }
            }
            return snmpPorts;
        }

        private const string OID_DOT1Q_PORT_ENTRY = $"{OID_COMPANY_DOT1Q_VLAN_GROUP}.7.1";
        private const string OID_TEMPLATE_DOT1Q_PORT_PVID = $"{OID_DOT1Q_PORT_ENTRY}.1";
        private readonly string OID_DOT1Q_PORT_PVID;

        private void calculateSnmpPortVlanMemberships(Dictionary<int, Dgs1210SnmpVlan> snmpVlans, Dictionary<int, Dgs1210SnmpPort> snmpPorts)
        {
            foreach (Port userPort in _parent.Switch.Ports)
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
                if (ownerVlans == 1 && lastOwnerSnmpVlan?.ID == snmpPort.PVID)
                {
                    userPort.CurrentVlan = lastOwnerSnmpVlan.UserVlan;
                    userPort.HasComplexMembership = false;
                }
                else
                {
                    userPort.CurrentVlan = null;
                    if (ownerVlans > 1 || lastOwnerSnmpVlan?.ID != snmpPort.PVID)
                        userPort.HasComplexMembership = true;
                }
            }
        }

        private (int, int) getByteBitIndex(int portIndex) => ((portIndex - 1) / 8, 7 - (portIndex - 1) % 8);

        async Task<bool> ISetPortToVlanMethod.DoAsync(Port port, Vlan vlan)
        {
            List<Variable> egressSet = new(), egressClear = new(), untaggedSet = new(), untaggedClear = new(), pvidValue = new()
            {
                new Variable(new ObjectIdentifier($"{OID_DOT1Q_PORT_PVID}.{port.Index}"), new Gauge32(vlan.ID))
            };
            (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_EGRESS_PORTS, vlan.ID, portByteIndex, portBitIndex, egressClear, egressSet);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_UNTAGGED_PORTS, vlan.ID, portByteIndex, portBitIndex, untaggedClear, untaggedSet);
            await _parent.SnmpConnection.SetAsync(pvidValue);
            await _parent.SnmpConnection.SetAsync(untaggedClear);
            await _parent.SnmpConnection.SetAsync(egressClear);
            await _parent.SnmpConnection.SetAsync(egressSet);
            await _parent.SnmpConnection.SetAsync(untaggedSet);
            return true;
        }

        private async Task getVlansBitfieldsForPort(string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesClear, List<Variable> variablesSet)
        {
            foreach (Variable oldRow in await _parent.SnmpConnection.WalkAsync(tableObjectIdentifier))
            {
                SnmpVariableHelpers.IdParts idParts = oldRow.GetIdParts();
                bool valueToSet = idParts.RowId == targetVlanId;
                byte[] snmpDataBytes = (oldRow.Data as OctetString).GetRaw();
                snmpDataBytes.SetBit(portByteIndex, portBitIndex, valueToSet);
                Variable newRow = new(oldRow.Id, new OctetString(snmpDataBytes));
                (valueToSet ? variablesSet : variablesClear).Add(newRow);
            }
        }

    }

}
