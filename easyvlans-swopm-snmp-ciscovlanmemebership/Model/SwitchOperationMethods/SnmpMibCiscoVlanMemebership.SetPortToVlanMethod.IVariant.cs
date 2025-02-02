using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibCiscoVlanMemebership
    {
        internal partial class SetPortToVlanMethod
        {
            internal interface IVariant
            {
                string Name { get; }
                Task SetVariables(ISnmpConnection snmpConnection, List<Variable> pvidVariables, List<Variable> egressToUnset, List<Variable> egressToSet, List<Variable> untaggedToUnset, List<Variable> untaggedToSet);
            }
        }
    }
}
