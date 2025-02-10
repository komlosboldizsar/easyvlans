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
                public async Task SetVariables(ISnmpConnection snmpConnection, List<Variable> switchportMode, List<Variable> accesVlan)
                {
                    await snmpConnection.SetAsync(switchportMode);
                    await snmpConnection.SetAsync(accesVlan);
                }
            }
        }
    }
}
