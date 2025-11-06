using easyvlans.Helpers;
using Lextm.SharpSnmpLib;
using static easyvlans.Model.SwitchOperationMethods.SnmpMibCiscoVlanMemebership.SetPortToVlanMethod;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibCiscoVlanMemebership
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
                ObjectIdentifier objectIdentifier = new($"{OID_CISCOVLANMEMEBERSHIP_TABLE_VLAN}.{ port.Index + _commonData.PortIndexOffset }");
                Variable snmpVlanVariable = new(objectIdentifier, new Integer32(vlan.ID));
                Variable snmpVlanTypeVariable = new(objectIdentifier, new Integer32(1));
               
                await _variant.SetVariables(_snmpConnection, snmpVlanVariable, snmpVlanTypeVariable);
                return true;
            }


            #region Variants
            private static readonly IVariant[] VARIANTS_TO_REGISTER = new IVariant[]
            {
                new VariantC3850()
            };

            internal static readonly Dictionary<string, IVariant> VARIANTS = new();

            static SetPortToVlanMethod()
            {
                foreach (IVariant variant in VARIANTS_TO_REGISTER)
                    VARIANTS.Add(variant.Name, variant);
            }

            internal static IVariant DEFAULT_VARIANT = new VariantC3850();
            #endregion

        }
    }
}
