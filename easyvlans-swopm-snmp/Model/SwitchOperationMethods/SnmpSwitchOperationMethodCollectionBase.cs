using easyvlans.Logger;

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

        public SnmpSwitchOperationMethodCollectionBase(Switch @switch, string accessVlanMembershipMethodName, string accessVlanMembershipMethodParams, string persistChangesMethodName, string persistChangesMethodParams)
        {
            Switch = @switch;
            ISnmpAccessVlanMembershipMethod accessVlanMembershipMethod = SnmpAccessVlanMembershipMethodRegister.Instance.GetMethodInstance(accessVlanMembershipMethodName, accessVlanMembershipMethodParams, this);
            logMethodFoundOrNot(@switch, "accessing and setting VLAN memberships", accessVlanMembershipMethodName, accessVlanMembershipMethod);
            ReadConfigMethod = accessVlanMembershipMethod;
            ISnmpPersistChangesMethod persistChangesMethod = SnmpPersistChangesMethodRegister.Instance.GetMethodInstance(persistChangesMethodName, persistChangesMethodParams, this);
            logMethodFoundOrNot(@switch, "persisting changes", persistChangesMethodName, persistChangesMethod);
            SetPortToVlanMethod = accessVlanMembershipMethod;
            PersistChangesMethod = persistChangesMethod;
        }

        private static void logMethodFoundOrNot(Switch @switch, string methodPurpose, string methodName, ISnmpMethod method)
        {
            if (method == null)
                LogDispatcher.W($"No SNMP method found with name [{methodName}] for {methodPurpose} of switch [{@switch.Label}].");
            else
                LogDispatcher.V($"Found SNMP method with name [{methodName}] for {methodPurpose} of switch [{@switch.Label}].");
        }

    }
}
