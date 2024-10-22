using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibTPLinkDot1qVlan
    {
        internal class ReadVlanMembershipMethod : MethodBase, IReadVlanMembershipMethod
        {

            public ReadVlanMembershipMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task DoAsync(IEnumerable<Port> ports = null)
            {
                Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans = await readSnmpVlansAsync();
                Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts = await readSnmpPortsAsync(ports);
                bindUserToSnmpVlans(snmpVlans);
                calculateSnmpPortVlanMemberships(snmpVlans, snmpPorts, ports);
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

            public async Task<Dictionary<int, TPLinkDot1qSnmpPort>> readSnmpPortsAsync(IEnumerable<Port> userPorts = null)
            {
                Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts = new();
                void processTpLinkDot1qPortType(TPLinkDot1qSnmpPort p, Variable v) => v.ToInt(i => p.Type = (TPLinkDot1qSnmpPort.PortType)i);
                void processTpLinkDot1qPortPvid(TPLinkDot1qSnmpPort p, Variable v) => v.ToInt(i => p.PVID = i);
                if (userPorts == null)
                {
                    await WalkAndProcess(OID_VLAN_PORT_TYPE, snmpPorts, id => new(id), processTpLinkDot1qPortType);
                    await WalkAndProcess(OID_VLAN_PORT_PVID, snmpPorts, id => new(id), processTpLinkDot1qPortPvid);
                }
                else
                {
                    List<string> oids = new();
                    foreach (Port userPort in userPorts)
                    {
                        if (userPort.Switch == _snmpConnection.Switch)
                        {
                            oids.Add($"{OID_VLAN_PORT_TYPE}.{userPort.Index}");
                            oids.Add($"{OID_VLAN_PORT_PVID}.{userPort.Index}");
                        }
                    }
                    Action<string, Variable, TPLinkDot1qSnmpPort> processTpLinkDot1qVlanPortConfigTableRow = (nodeId, tpLinkDot1QportRow, snmpPort) =>
                    {
                        switch (nodeId)
                        {
                            case OID_VLAN_PORT_TYPE:
                                processTpLinkDot1qPortType(snmpPort, tpLinkDot1QportRow);
                                break;
                            case OID_VLAN_PORT_PVID:
                                processTpLinkDot1qPortPvid(snmpPort, tpLinkDot1QportRow);
                                break;
                        }
                    };
                    TableProcessHelpers.ProcessTableRows(await _snmpConnection.GetAsync(oids), snmpPorts, id => new TPLinkDot1qSnmpPort(id), processTpLinkDot1qVlanPortConfigTableRow);
                }
                return snmpPorts;
            }

            public void calculateSnmpPortVlanMemberships(Dictionary<int, TPLinkDot1qSnmpVlan> snmpVlans, Dictionary<int, TPLinkDot1qSnmpPort> snmpPorts, IEnumerable<Port> userPorts = null)
            {
                foreach (Port userPort in _snmpConnection.Switch.Ports)
                {
                    if ((userPorts != null) && !userPorts.Contains(userPort))
                        continue;
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
