using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibQBridge
    {
        internal class SetPortToVlanMethod : MethodBase, ISetPortToVlanMethod
        {

            public SetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task<bool> DoAsync(Port port, Vlan vlan)
            {
                List<Variable> pvidVariables = new(),
                    egressToUnset = new(),
                    egressToSet = new(),
                    untaggedToUnset = new(),
                    untaggedToSet = new();
                if (!_commonData.NoPvid)
                    pvidVariables.Add(new Variable(new ObjectIdentifier($"{OID_DOT1Q_PVID}.{port.Index}"), new Gauge32(vlan.ID)));
                (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index);
                await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS, vlan.ID, portByteIndex, portBitIndex, egressToSet, egressToUnset);
                await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS, vlan.ID, portByteIndex, portBitIndex, untaggedToSet, untaggedToUnset);
                await _snmpConnection.SetAsync(pvidVariables);
                await _snmpConnection.SetAsync(untaggedToUnset);
                await _snmpConnection.SetAsync(egressToSet);
                await _snmpConnection.SetAsync(egressToUnset);
                await _snmpConnection.SetAsync(untaggedToSet);
                return true;
            }

            private async Task getVlansBitfieldsForPort(string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesToSet, List<Variable> variablesToUnset)
            {
                foreach (Variable oldRow in await _snmpConnection.WalkAsync(tableObjectIdentifier))
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
}
