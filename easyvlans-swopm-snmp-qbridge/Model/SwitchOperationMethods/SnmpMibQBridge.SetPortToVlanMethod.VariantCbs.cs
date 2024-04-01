using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibQBridge
    {
        internal partial class SetPortToVlanMethod
        {
            internal class VariantCbs : IVariant
            {
                public string Name => "cbs";
                public async Task SetVariables(ISnmpConnection snmpConnection, List<Variable> pvidVariables, List<Variable> egressToUnset, List<Variable> egressToSet, List<Variable> untaggedToUnset, List<Variable> untaggedToSet)
                {
                    await snmpConnection.SetAsync(pvidVariables);
                    untaggedToUnset.ForEach(async v => await snmpConnection.SetAsync(v));
                    egressToSet.ForEach(async v => await snmpConnection.SetAsync(v));
                    egressToUnset.ForEach(async v => await snmpConnection.SetAsync(v));
                    untaggedToSet.ForEach(async v => await snmpConnection.SetAsync(v));
                }
            }
        }
    }
}
