using easyvlans.Helpers;
using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibQBridge
    {
        internal partial class SetPortToVlanMethod : MethodBase, ISetPortToVlanMethod
        {

            public SetPortToVlanMethod(ISnmpConnection snmpConnection, object commonData)
                : base(snmpConnection, commonData)
            {
                string variantName = ((CommonData)commonData).SetMembershipVariant;
                if ((variantName == null) || !VARIANTS.TryGetValue(variantName, out _variant))
                    _variant = DEFAULT_VARIANT;
            }

            private readonly IVariant _variant;

            public async Task<bool> DoAsync(Port port, Vlan vlan)
            {
                List<Variable> pvidVariables = new(),
                    egressToUnset = new(),
                    egressToSet = new(),
                    untaggedToUnset = new(),
                    untaggedToSet = new();
                if (!_commonData.NoPvid)
                    pvidVariables.Add(new Variable(new ObjectIdentifier($"{OID_DOT1Q_PVID}.{port.Index + _commonData.PortIndexOffset}"), new Gauge32(vlan.ID)));
                (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index + _commonData.PortIndexOffset);
                await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS, vlan.ID, portByteIndex, portBitIndex, egressToSet, egressToUnset);
                await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS, vlan.ID, portByteIndex, portBitIndex, untaggedToSet, untaggedToUnset);
                await _variant.SetVariables(_snmpConnection, pvidVariables, egressToUnset, egressToSet, untaggedToUnset, untaggedToSet);
                return true;
            }

            private async Task getVlansBitfieldsForPort(string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesToSet, List<Variable> variablesToUnset)
            {
                foreach (Variable oldRow in await _snmpConnection.WalkAsync(tableObjectIdentifier))
                {
                    SnmpVariableHelpers.IdParts idParts = oldRow.GetIdParts();
                    if (idParts.RowId == 1)
                        continue;
                    bool valueToSet = idParts.RowId == targetVlanId;
                    byte[] snmpDataBytes = (oldRow.Data as OctetString).GetRaw();
                    snmpDataBytes.SetBit(portByteIndex, portBitIndex, valueToSet);
                    Variable newRow = new(oldRow.Id, new OctetString(snmpDataBytes));
                    (valueToSet ? variablesToSet : variablesToUnset).Add(newRow);
                }
            }

            #region Variants
            private static readonly IVariant[] VARIANTS_TO_REGISTER = new IVariant[]
            {
                new VariantCbs(),
                new VariantSg220()
            };

            internal static readonly Dictionary<string, IVariant> VARIANTS = new();

            static SetPortToVlanMethod()
            {
                foreach (IVariant variant in VARIANTS_TO_REGISTER)
                    VARIANTS.Add(variant.Name, variant);
            }

            internal static IVariant DEFAULT_VARIANT = new VariantCbs();
            #endregion

        }
    }
}
