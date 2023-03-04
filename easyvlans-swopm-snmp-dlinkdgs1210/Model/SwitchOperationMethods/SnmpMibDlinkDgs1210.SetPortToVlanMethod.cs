using B.XmlDeserializer.Context;
using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibDlinkDgs1210
    {
        internal class SetPortToVlanMethod : MethodBase, ISetPortToVlanMethod
        {

            public SetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData) { }

            public async Task<bool> DoAsync(Port port, Vlan vlan)
            {
                List<Variable> egressSet = new(), egressClear = new(), untaggedSet = new(), untaggedClear = new(), pvidValue = new()
                {
                    new Variable(new ObjectIdentifier($"{_oidsForModel.OID_DOT1Q_PORT_PVID}.{port.Index}"), new Gauge32(vlan.ID))
                };
                (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index);
                await getVlansBitfieldsForPort(_oidsForModel.OID_DOT1Q_VLAN_EGRESS_PORTS, vlan.ID, portByteIndex, portBitIndex, egressClear, egressSet);
                await getVlansBitfieldsForPort(_oidsForModel.OID_DOT1Q_VLAN_UNTAGGED_PORTS, vlan.ID, portByteIndex, portBitIndex, untaggedClear, untaggedSet);
                await _snmpConnection.SetAsync(pvidValue);
                await _snmpConnection.SetAsync(untaggedClear);
                await _snmpConnection.SetAsync(egressClear);
                await _snmpConnection.SetAsync(egressSet);
                await _snmpConnection.SetAsync(untaggedSet);
                return true;
            }

            private async Task getVlansBitfieldsForPort(string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesClear, List<Variable> variablesSet)
            {
                foreach (Variable oldRow in await _snmpConnection.WalkAsync(tableObjectIdentifier))
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
}
