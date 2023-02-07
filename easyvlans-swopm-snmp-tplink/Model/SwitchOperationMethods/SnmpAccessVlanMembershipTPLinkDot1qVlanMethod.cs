using B.XmlDeserializer.Context;
using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipTPLinkDot1qVlanMethod : ISnmpAccessVlanMembershipMethod
    {

        public const string CODE = "tplinkdot1qvlan";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipTPLinkDot1qVlanMethod(data, deserializationContext, parent);
        }

        private ISnmpSwitchOperationMethodCollection _parent;

        public SnmpAccessVlanMembershipTPLinkDot1qVlanMethod(XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent)
        {
            _parent = parent;
        }

        public string Code => CODE;
        public string DetailedCode => $"{_parent.Code}[{CODE}]";

        async Task IReadConfigMethod.DoAsync()
        {
            Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans = await readSnmpVlansAsync();
            Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts = await readSnmpPortsAsync();
            bindUserToSnmpVlans(snmpVlans);
            calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
        }

        public async Task<Dictionary<int, TPLinkDot1qSnmpVlan>> readSnmpVlansAsync()
        {
            Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans = new();
            foreach (Variable portVlanStaticTableRow in await _parent.SnmpConnection.WalkAsync(OID_VLAN_CONFIG_TABLE))
            {
                SnmpVariableHelpers.IdParts idParts = portVlanStaticTableRow.GetIdParts();
                TPLinkDot1qSnmpVlan snmpVlan = snmpVlans.GetAnyway(idParts.RowId, id => new TPLinkDot1qSnmpVlan(id));
                switch (idParts.NodeId)
                {
                    case OID_VLAN_TAG_PORT_MEMBER_ADD:
                        snmpVlan.TagPorts = portVlanStaticTableRow.Data.ToString();
                        break;
                    case OID_VLAN_UNTAG_PORT_MEMBER_ADD:
                        snmpVlan.UntagPorts = portVlanStaticTableRow.Data.ToString();
                        break;
                }
            }
            return snmpVlans;
        }

        private const string OID_VLAN_CONFIG_TABLE = "1.3.6.1.4.1.11863.6.14.1.2.1";
        private const string OID_VLAN_TAG_PORT_MEMBER_ADD = $"{OID_VLAN_CONFIG_TABLE}.1.3";
        private const string OID_VLAN_UNTAG_PORT_MEMBER_ADD = $"{OID_VLAN_CONFIG_TABLE}.1.4";
        private const string OID_VLAN_PORT_MEMBER_REMOVE = $"{OID_VLAN_CONFIG_TABLE}.1.5";

        public void bindUserToSnmpVlans(Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans)
        {
            foreach (Vlan userVlan in _parent.Switch.Config.Vlans.Values)
                if (snmpVlans.TryGetValue(userVlan.ID, out TPLinkDot1qSnmpVlan snmpVlan))
                    snmpVlan.UserVlan = userVlan;
        }

        public async Task<Dictionary<int, TPLinkDot1qSnmpPort>> readSnmpPortsAsync()
        {
            Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts = new();
            foreach (Variable portVlanTableRow in await _parent.SnmpConnection.WalkAsync(OID_VLAN_PORT_CONFIG_TABLE))
            {
                SnmpVariableHelpers.IdParts idParts = portVlanTableRow.GetIdParts();
                TPLinkDot1qSnmpPort snmpPort = snmpPorts.GetAnyway(idParts.RowId - OID_INTERFACE_INDEX_OFFSET, id => new TPLinkDot1qSnmpPort(id));
                switch (idParts.NodeId)
                {
                    case OID_VLAN_PORT_TYPE:
                        if (int.TryParse(portVlanTableRow.Data.ToString(), out int type))
                            snmpPort.Type = (TPLinkDot1qSnmpPort.PortType)type;
                        break;
                    case OID_VLAN_PORT_PVID:
                        if (int.TryParse(portVlanTableRow.Data.ToString(), out int pvid))
                            snmpPort.PVID = pvid;
                        break;
                }
            }
            return snmpPorts;
        }

        private const string OID_VLAN_PORT_CONFIG_TABLE = "1.3.6.1.4.1.11863.6.14.1.1.1";
        private const string OID_VLAN_PORT_TYPE = $"{OID_VLAN_PORT_CONFIG_TABLE}.1.2";
        private const string OID_VLAN_PORT_PVID = $"{OID_VLAN_PORT_CONFIG_TABLE}.1.3";

        private const int OID_INTERFACE_INDEX_OFFSET = 49152;

        public void calculateSnmpPortVlanMemberships(Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans, Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts)
        {
            foreach (Port userPort in _parent.Switch.Ports)
            {
                if (!snmpPorts.TryGetValue(userPort.Index, out TPLinkDot1qSnmpPort snmpPort))
                {
                    userPort.CurrentVlan = null;
                    continue;
                }
                if (snmpPort.Type != TPLinkDot1qSnmpPort.PortType.General)
                {
                    userPort.CurrentVlan = null;
                    userPort.HasComplexMembership = true;
                    continue;
                }
                int ownerTaggedVlans = 0, ownerUntaggedVlans = 0;
                TPLinkDot1qSnmpVlan lastOwnerUntaggedSnmpVlan = null;
                foreach (TPLinkDot1qSnmpVlan snmpVlan in snmpVlans.Values)
                {
                    if (snmpVlan.ContainsTag(snmpPort))
                        ownerTaggedVlans++;
                    if (snmpVlan.ContainsUntag(snmpPort))
                    {
                        ownerUntaggedVlans++;
                        lastOwnerUntaggedSnmpVlan = snmpVlan;
                    }
                }
                if ((ownerTaggedVlans == 0) && (ownerUntaggedVlans == 1) && (lastOwnerUntaggedSnmpVlan?.ID == snmpPort.PVID))
                {
                    userPort.CurrentVlan = lastOwnerUntaggedSnmpVlan.UserVlan;
                    userPort.HasComplexMembership = false;
                }
                else
                {
                    userPort.CurrentVlan = null;
                    if ((ownerTaggedVlans > 0) || (ownerUntaggedVlans > 1) || (lastOwnerUntaggedSnmpVlan?.ID != snmpPort.PVID))
                        userPort.HasComplexMembership = true;
                }
            }
        }

        async Task<bool> ISetPortToVlanMethod.DoAsync(Port port, Vlan vlan)
        {
            int interfaceIndex = port.Index + OID_INTERFACE_INDEX_OFFSET;
            await _parent.SnmpConnection.SetAsync(new List<Variable>()
            {
                new Variable(new ObjectIdentifier($"{OID_VLAN_PORT_TYPE}.{interfaceIndex}"), new Gauge32((int)TPLinkDot1qSnmpPort.PortType.General)),
                new Variable(new ObjectIdentifier($"{OID_VLAN_PORT_PVID}.{interfaceIndex}"), new Gauge32(vlan.ID))
            });
            OctetString portIdOctetString = new(port.Index.ToString());
            await _parent.SnmpConnection.SetAsync($"{OID_VLAN_PORT_MEMBER_REMOVE}.{vlan.ID}", portIdOctetString);
            await _parent.SnmpConnection.SetAsync($"{OID_VLAN_TAG_PORT_MEMBER_ADD}.{vlan.ID}", portIdOctetString);
            return true;
        }

    }

}
