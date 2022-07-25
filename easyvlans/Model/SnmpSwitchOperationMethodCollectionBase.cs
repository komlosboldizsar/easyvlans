using easyvlans.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
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

        public SnmpSwitchOperationMethodCollectionBase(Switch @switch, string accessVlanMembershipMethodName, string persistChangesMethodName)
        {
            Switch = @switch;
            ISnmpAccessVlanMembershipMethod accessVlanMembershipMethod = SnmpAccessVlanMembershipMethodRegister.Instance.GetMethodInstance(accessVlanMembershipMethodName, this);
            logMethodFoundOrNot(@switch, "accessing and setting VLAN memberships", accessVlanMembershipMethodName, accessVlanMembershipMethod);
            ReadConfigMethod = accessVlanMembershipMethod;
            ISnmpPersistChangesMethod persistChangesMethod = SnmpPersistChangesMethodRegister.Instance.GetMethodInstance(persistChangesMethodName, this);
            logMethodFoundOrNot(@switch, "persisting changes", persistChangesMethodName, persistChangesMethod);
            SetPortToVlanMethod = accessVlanMembershipMethod;
            PersistChangesMethod = persistChangesMethod;
        }

        private static void logMethodFoundOrNot(Switch @switch, string methodPurpose, string methodName, ISnmpMethod method)
        {
            if (methodName == null)
            {
                if (method == null)
                    LogDispatcher.W($"No default method found for {methodPurpose} of switch [{@switch.Label}].");
                else
                    LogDispatcher.V($"Using default method for {methodPurpose} of switch [{@switch.Label}].");
            }
            else
            {
                if (method == null)
                    LogDispatcher.W($"No method found with name [{methodName}] for {methodPurpose} of switch [{@switch.Label}].");
                else
                    LogDispatcher.V($"Found method with name [{methodName}] for {methodPurpose} of switch [{@switch.Label}].");
            }
        }

    }
}
