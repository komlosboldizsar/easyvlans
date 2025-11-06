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
                Task SetVariables(ISnmpConnection snmpConnection, Variable snmpVlanVariable, Variable snmpVlanTypeVariable);
            }
        }
    }
}
