using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibCiscoVlanMemebership
    {
        internal partial class SetPortToVlanMethod
        {
            internal class VariantC3850 : IVariant
            {
                public string Name => "c3850";
                public async Task SetVariables(ISnmpConnection snmpConnection, Variable snmpVlanVariable, Variable snmpVlanTypeVariable)
                {
                    await snmpConnection.SetAsync(snmpVlanVariable);
                    await snmpConnection.SetAsync(snmpVlanTypeVariable);
                }
            }
        }
    }
}
