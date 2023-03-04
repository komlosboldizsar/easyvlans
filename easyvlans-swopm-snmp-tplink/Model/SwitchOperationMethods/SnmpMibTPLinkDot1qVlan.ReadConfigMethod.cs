using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibTPLinkDot1qVlan
    {
        internal class ReadConfigMethod : MethodBase, IReadConfigMethod
        {

            public ReadConfigMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task DoAsync()
            {
                Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans = await readSnmpVlansAsync();
                Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts = await readSnmpPortsAsync();
                bindUserToSnmpVlans(snmpVlans);
                calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
            }

            public async Task<Dictionary<int, TPLinkDot1qSnmpVlan>> readSnmpVlansAsync()
            {
                Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans = new();
                foreach (Variable portVlanStaticTableRow in await _snmpConnection.WalkAsync(OID_VLAN_CONFIG_TABLE))
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
            public void bindUserToSnmpVlans(Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans)
            {
                foreach (Vlan userVlan in _snmpConnection.Switch.Config.Vlans.Values)
                    if (snmpVlans.TryGetValue(userVlan.ID, out TPLinkDot1qSnmpVlan snmpVlan))
                        snmpVlan.UserVlan = userVlan;
            }

            public async Task<Dictionary<int, TPLinkDot1qSnmpPort>> readSnmpPortsAsync()
            {
                Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts = new();
                foreach (Variable portVlanTableRow in await _snmpConnection.WalkAsync(OID_VLAN_PORT_CONFIG_TABLE))
                {
                    SnmpVariableHelpers.IdParts idParts = portVlanTableRow.GetIdParts();
                    int? localIndex = _portMappings.LookupBySnmpIndex(idParts.RowId)?.SnmpIndexToLocalIndex(idParts.RowId);
                    if (localIndex != null)
                    {
                        TPLinkDot1qSnmpPort snmpPort = snmpPorts.GetAnyway((int)localIndex, id => new TPLinkDot1qSnmpPort(id));
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
                }
                return snmpPorts;
            }

            public void calculateSnmpPortVlanMemberships(Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans, Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts)
            {
                foreach (Port userPort in _snmpConnection.Switch.Ports)
                {
                    if (!snmpPorts.TryGetValue(userPort.Index, out TPLinkDot1qSnmpPort snmpPort))
                    {
                        userPort.CurrentVlan = null;
                        continue;
                    }
                    PortMapping mapping = _portMappings.LookupByLocalIndex(userPort.Index);
                    if (mapping == null)
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
                    TPLinkDot1qThreePartPortId threePart = mapping.LocalIndexToThreePartId(userPort.Index);
                    foreach (TPLinkDot1qSnmpVlan snmpVlan in snmpVlans.Values)
                    {
                        if (snmpVlan.ContainsTag(threePart))
                            ownerTaggedVlans++;
                        if (snmpVlan.ContainsUntag(threePart))
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

        }
    }
}
