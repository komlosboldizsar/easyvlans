using easyvlans.Logger;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal abstract class SnmpSwitchOperationMethodCollectionBase : ISnmpSwitchOperationMethodCollection
    {

        public abstract string Code { get; }
        public string DetailedCode => $"{Code}[{ReadConfigMethod.Code},{PersistChangesMethod.Code}]";

        public IReadConfigMethod ReadConfigMethod { get; private init; }
        public ISetPortToVlanMethod SetPortToVlanMethod { get; private init; }
        public IPersistChangesMethod PersistChangesMethod { get; private init; }

        public ISnmpConnection SnmpConnection { get; protected init; }
        public Switch Switch { get; private set; }

        public SnmpSwitchOperationMethodCollectionBase(Switch @switch, string accessVlanMembershipMethodName, XmlNode accessVlanMembershipMethodData, string persistChangesMethodName, XmlNode persistChangesMethodData)
        {
            Switch = @switch;
            ISnmpAccessVlanMembershipMethod accessVlanMembershipMethod = SnmpAccessVlanMembershipMethodRegister.Instance.GetMethodInstance(accessVlanMembershipMethodName, accessVlanMembershipMethodData, this);
            logMethodFoundOrNot(@switch, METHOD_PURPOSE_ACCESS_VLAN_MEMBERSHIP, accessVlanMembershipMethodName, accessVlanMembershipMethod);
            ReadConfigMethod = accessVlanMembershipMethod;
            SetPortToVlanMethod = accessVlanMembershipMethod;
            if (persistChangesMethodName != null)
            {
                ISnmpPersistChangesMethod persistChangesMethod = SnmpPersistChangesMethodRegister.Instance.GetMethodInstance(persistChangesMethodName, persistChangesMethodData, this);
                logMethodFoundOrNot(@switch, METHOD_PURPOSE_PERSIST_CHANGES, persistChangesMethodName, persistChangesMethod);
                PersistChangesMethod = persistChangesMethod;
            }
            else
            {
                LogDispatcher.V($"No SNMP method defined for {METHOD_PURPOSE_PERSIST_CHANGES} of switch [{@switch.Label}].");
                PersistChangesMethod = null;
            }
        }

        private static void logMethodFoundOrNot(Switch @switch, string methodPurpose, string methodName, ISnmpMethod method)
        {
            if (method == null)
                LogDispatcher.W($"No SNMP method found with name [{methodName}] for {methodPurpose} of switch [{@switch.Label}].");
            else
                LogDispatcher.V($"Found SNMP method with name [{methodName}] for {methodPurpose} of switch [{@switch.Label}].");
        }

        private const string METHOD_PURPOSE_ACCESS_VLAN_MEMBERSHIP = "accessing and setting VLAN memberships";
        private const string METHOD_PURPOSE_PERSIST_CHANGES = "persisting changes";

    }
}
