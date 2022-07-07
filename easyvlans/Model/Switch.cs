using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public partial class Switch
    {

        public readonly string ID;
        public readonly string Label;
        private readonly IPEndPoint ipEndPoint;
        private readonly OctetString communityString;
        private readonly IAccessVlanMembershipMethod accessVlanMembershipMethod;
        private readonly IPersistChangesMethod persistChangesMethod;

        public readonly List<UserPort> Ports = new();
        private List<UserPort> portsWithPendingChange = new();

        public Config Config { get; private set; }

        public delegate void PortsWithPendingChangeCountChangedDelegate(Switch @switch, int newValue);
        public event PortsWithPendingChangeCountChangedDelegate PortsWithPendingChangeCountChanged;
        private int _portsWithPendingChangeCount;
        public int PortsWithPendingChangeCount
        {
            get => _portsWithPendingChangeCount;
            private set
            {
                if (value == _portsWithPendingChangeCount)
                    return;
                _portsWithPendingChangeCount = value;
                PortsWithPendingChangeCountChanged?.Invoke(this, value);
            }
        }

        public delegate void StatusChangedDelegate(Switch @switch, SwitchStatus newValue);
        public event StatusChangedDelegate StatusChanged;
        private SwitchStatus _status = SwitchStatus.NotConnected;
        public SwitchStatus Status
        {
            get => _status;
            private set
            {
                if (value == _status)
                    return;
                _status = value;
                StatusChanged?.Invoke(this, value);
            }
        }

        public Switch(string id, string label, string ip, int port, string communityString, string accessVlanMembershipMethodName, string persistChangesMethodName)
        {
            ID = id;
            Label = label;
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.communityString = new OctetString(communityString);
            accessVlanMembershipMethod = AccessVlanMembershipMethods.Instance.GetInstance(accessVlanMembershipMethodName, this);
            logMethodFoundOrNot("accessing and setting VLAN memberships", accessVlanMembershipMethodName, accessVlanMembershipMethod);
            persistChangesMethod = PersistChangesMethods.Instance.GetInstance(persistChangesMethodName, this);
            logMethodFoundOrNot("persisting changes", persistChangesMethodName, persistChangesMethod);
        }

        private void logMethodFoundOrNot(string methodPurpose, string methodName, IMethod method)
        {
            if (method == null)
                LogDispatcher.W($"No method found with name [{methodName}] for {methodPurpose} of switch [{Label}].");
            else
                LogDispatcher.V($"Found method with name [{methodName}] for {methodPurpose} of switch [{Label}].");
        }

        internal void AssignConfig(Config config) => Config = config;

        internal void AssociatePort(UserPort port)
        {
            if ((port.Switch == this) && !Ports.Contains(port))
                Ports.Add(port);
        }

        public async Task<List<Variable>> SnmpBulkWalkAsync(string objectIdentifierStr)
        {
            List<Variable> result = new List<Variable>();
            await Messenger.BulkWalkAsync(VersionCode.V2, ipEndPoint, communityString, OctetString.Empty,
                new ObjectIdentifier(objectIdentifierStr), result, 5, WalkMode.WithinSubtree, null, null);
            return result;
        }

        public async Task SnmpSetAsync(List<Variable> variables)
            => await Messenger.SetAsync(VersionCode.V2, ipEndPoint, communityString, variables);

        public async Task ReadConfigAsync()
        {
            if (accessVlanMembershipMethod == null)
            {
                LogDispatcher.E($"Couldn't read configuration of switch [{Label}], because no method is associated.");
                return;
            }
            LogDispatcher.I($"Reading configuration of switch [{Label}]...");
            LogDispatcher.V($"Method for reading configuration of switch [{Label}]: [{accessVlanMembershipMethod.Name}].");
            try
            {
                await accessVlanMembershipMethod.ReadConfigAsync();
                LogDispatcher.I($"Reading configuration of switch [{Label}] ready.");
            }
            catch (Exception ex)
            {
                LogDispatcher.E($"Unsuccessfull reading of configuration of switch [{Label}]. Error message: [{ex.Message}]");
            }
        }

        public async Task<bool> SetPortToVlanAsync(UserPort port, UserVlan vlan)
        {
            if ((port.Switch != this) || !Ports.Contains(port))
                return false;
            if (accessVlanMembershipMethod == null)
            {
                LogDispatcher.E($"Couldn't set VLAN membership of port [{port.Label}] @ switch [{Label}], because no method is associated.");
                return false;
            }
            LogDispatcher.I($"Setting membership of port [{port.Label}] @ switch [{Label}] to VLAN [{vlan.Name}]...");
            LogDispatcher.V($"Method for setting VLAN membership of port [{port.Label}] @ switch [{Label}]: [{accessVlanMembershipMethod.Name}].");
            try
            {
                await accessVlanMembershipMethod.SetPortToVlanAsync(port, vlan);
                portUpdated(port);
                LogDispatcher.I($"Setting membership of port [{port.Label}] @ switch [{Label}] to VLAN [{vlan.Name}] ready.");
                return true;
            }
            catch (Exception ex)
            {
                LogDispatcher.E($"Unsuccessful setting of membership of port [{port.Label}] @ switch [{Label}] to VLAN [{vlan.Name}]. Error message: [{ex.Message}]");
                return false;
            }
        }

        private void portUpdated(UserPort port)
        {
            if (portsWithPendingChange.Contains(port))
                return;
            portsWithPendingChange.Add(port);
            PortsWithPendingChangeCount++;
        }

        public async Task<bool> PersistChangesAsync()
        {
            if (persistChangesMethod == null)
            {
                LogDispatcher.E($"Couldn't persist changes of switch [{Label}], because no method is associated.");
                return false;
            }
            LogDispatcher.I($"Persisting changes of switch [{Label}]...");
            LogDispatcher.V($"Method for persisting changes of switch [{Label}]: [{persistChangesMethod.Name}].");
            try
            {
                await persistChangesMethod.Do();
                changesPersisted();
                LogDispatcher.I($"Persisting changes of switch [{Label}] ready.");
                return true;
            }
            catch (Exception ex)
            {
                LogDispatcher.E($"Didn't succeeded to persist changes of switch [{Label}] with method [{persistChangesMethod.Name}]. Error message: [{ex.Message}]");
            }
            return false;
        }

        private void changesPersisted()
        {
            foreach (UserPort userPort in portsWithPendingChange)
                userPort.ChangesPersisted();
            portsWithPendingChange.Clear();
            PortsWithPendingChangeCount = 0;
        }

    }

}
