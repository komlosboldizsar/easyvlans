using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibCiscoVlanMemebership
    {
        internal partial class SetPortToVlanMethod
        {
            internal class VariantCbs : IVariant
            {
                public string Name => "cbs";
                public async Task SetVariables(ISnmpConnection snmpConnection, List<Variable> pvidVariables, List<Variable> egressToUnset, List<Variable> egressToSet, List<Variable> untaggedToUnset, List<Variable> untaggedToSet)
                {
                    await snmpConnection.SetAsync(pvidVariables);
                    foreach (Variable v in untaggedToUnset)
                        await snmpConnection.SetAsync(v);
                    foreach (Variable v in egressToSet)
                        await snmpConnection.SetAsync(v);
                    foreach (Variable v in egressToUnset)
                        await snmpConnection.SetAsync(v);
                    foreach (Variable v in untaggedToSet)
                        await snmpConnection.SetAsync(v);
                }
            }
        }
    }
}
