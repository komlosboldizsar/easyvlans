using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibTPLinkDot1qVlan
    {
        internal class SetPortToVlanMethod : MethodBase, ISetPortToVlanMethod
        {

            public SetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task<bool> DoAsync(Port port, Vlan vlan)
            {
                PortMapping mapping = _portMappings.LookupByLocalIndex(port.Index);
                if (mapping == null)
                    throw new NoMappingForPortException(port.Index);
                int snmpIndex = mapping.LocalIndexToSnmpIndex(port.Index);
                await _snmpConnection.SetAsync(new List<Variable>()
                {
                    new Variable(new ObjectIdentifier($"{OID_VLAN_PORT_TYPE}.{snmpIndex}"), new Gauge32((int)TPLinkDot1qSnmpPort.PortType.General)),
                    new Variable(new ObjectIdentifier($"{OID_VLAN_PORT_PVID}.{snmpIndex}"), new Integer32(vlan.ID))
                });
                OctetString portIdOctetString = new(mapping.LocalIndexToSimpleId(port.Index).ToString());
                await _snmpConnection.SetAsync($"{OID_VLAN_PORT_MEMBER_REMOVE}.{vlan.ID}", portIdOctetString);
                await _snmpConnection.SetAsync($"{OID_VLAN_TAG_PORT_MEMBER_ADD}.{vlan.ID}", portIdOctetString);
                return true;
            }

        }
    }
}
