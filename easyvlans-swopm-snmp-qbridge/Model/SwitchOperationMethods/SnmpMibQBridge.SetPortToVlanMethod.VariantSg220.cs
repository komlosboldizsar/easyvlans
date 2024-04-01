using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibQBridge
    {
        internal partial class SetPortToVlanMethod
        {
            internal class VariantSg220 : IVariant
            {
                public string Name => "sg220";
                public async Task SetVariables(ISnmpConnection snmpConnection, List<Variable> pvidVariables, List<Variable> egressToUnset, List<Variable> egressToSet, List<Variable> untaggedToUnset, List<Variable> untaggedToSet)
                {
                    await snmpConnection.SetAsync(pvidVariables);
                    //await _snmpConnection.SetAsync(untaggedToUnset); // not needed
                    await snmpConnection.SetAsync(egressToSet);
                    await snmpConnection.SetAsync(egressToUnset);
                    await snmpConnection.SetAsync(untaggedToSet);
                }
            }
        }
    }
}
