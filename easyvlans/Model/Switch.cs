using easyvlans.Helpers;
using easyvlans.Logger;
using easyvlans.Model.Remote.Snmp;
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

    public class Switch : IRemoteable
    {

        public readonly string ID;
        public readonly string Label;
        public int? RemoteIndex { get; init; }
        private readonly IPEndPoint ipEndPoint;
        private readonly OctetString readCommunityString;
        private readonly OctetString writeCommunityString;
        private readonly IAccessVlanMembershipMethod accessVlanMembershipMethod;
        private readonly IPersistChangesMethod persistChangesMethod;

        public readonly List<Port> Ports = new();
        private readonly List<Port> portsWithPendingChange = new();

        public Config Config { get; set; }

        public event PropertyChangedDelegate<Switch, int> PortsWithPendingChangeCountChanged;
        private int _portsWithPendingChangeCount;
        public int PortsWithPendingChangeCount
        {
            get => _portsWithPendingChangeCount;
            private set => this.setProperty(ref _portsWithPendingChangeCount, value, PortsWithPendingChangeCountChanged);
        }

        public event PropertyChangedDelegate<Switch, Status> ReadVlanConfigStatusChanged;
        private Status _readVlanConfigStatus = Status.Unknown;
        public Status ReadVlanConfigStatus
        {
            get => _readVlanConfigStatus;
            private set
            {
                if (this.setProperty(ref _readVlanConfigStatus, value, ReadVlanConfigStatusChanged))
                    ReadVlanConfigStatusUpdateTime = DateTime.Now;
            }
        }

        public event PropertyChangedDelegate<Switch, DateTime> ReadVlanConfigStatusUpdateTimeChanged;
        private DateTime _readVlanConfigStatusUpdateTime = DateTime.Now;
        public DateTime ReadVlanConfigStatusUpdateTime
        {
            get => _readVlanConfigStatusUpdateTime;
            private set => this.setProperty(ref _readVlanConfigStatusUpdateTime, value, ReadVlanConfigStatusUpdateTimeChanged);
        }

        public event PropertyChangedDelegate<Switch, Status> PersistConfigStatusChanged;
        private Status _persistConfigStatus = Status.Empty;
        public Status PersistVlanConfigStatus
        {
            get => _persistConfigStatus;
            private set
            {
                if (this.setProperty(ref _persistConfigStatus, value, PersistConfigStatusChanged))
                    PersistVlanConfigStatusUpdateTime = DateTime.Now;
            }
        }

        public event PropertyChangedDelegate<Switch, DateTime> PersistVlanConfigStatusUpdateTimeChanged;
        private DateTime _persistVlanConfigStatusUpdateTime = DateTime.Now;
        public DateTime PersistVlanConfigStatusUpdateTime
        {
            get => _persistVlanConfigStatusUpdateTime;
            private set => this.setProperty(ref _persistVlanConfigStatusUpdateTime, value, PersistVlanConfigStatusUpdateTimeChanged);
        }

        public Switch(string id, string label, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, string persistChangesMethodName, int? remoteIndex)
        {
            ID = id;
            Label = label;
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            string[] communityStringParts = communityStrings.Split(':');
            if (communityStringParts.Length > 1)
            {
                readCommunityString = new OctetString(communityStringParts[0]);
                writeCommunityString = new OctetString(communityStringParts[1]);
            }
            else
            {
                readCommunityString = writeCommunityString = new OctetString(communityStrings);
            }
            accessVlanMembershipMethod = AccessVlanMembershipMethods.Instance.GetInstance(accessVlanMembershipMethodName, this);
            logMethodFoundOrNot("accessing and setting VLAN memberships", accessVlanMembershipMethodName, accessVlanMembershipMethod);
            persistChangesMethod = PersistChangesMethods.Instance.GetInstance(persistChangesMethodName, this);
            logMethodFoundOrNot("persisting changes", persistChangesMethodName, persistChangesMethod);
            RemoteIndex = remoteIndex;
        }

        private void logMethodFoundOrNot(string methodPurpose, string methodName, IMethod method)
        {
            if (methodName == null)
            {
                if (method == null)
                    LogDispatcher.W($"No default method found for {methodPurpose} of switch [{Label}].");
                else
                    LogDispatcher.V($"Using default method for {methodPurpose} of switch [{Label}].");
            }
            else
            {
                if (method == null)
                    LogDispatcher.W($"No method found with name [{methodName}] for {methodPurpose} of switch [{Label}].");
                else
                    LogDispatcher.V($"Found method with name [{methodName}] for {methodPurpose} of switch [{Label}].");
            }
        }

        internal void AssociatePort(Port port)
        {
            if ((port.Switch == this) && !Ports.Contains(port))
                Ports.Add(port);
        }

        public async Task<List<Variable>> SnmpBulkWalkAsync(string objectIdentifierStr)
        {
            List<Variable> result = new();
            await Messenger.BulkWalkAsync(VersionCode.V2, ipEndPoint, readCommunityString, OctetString.Empty,
                new ObjectIdentifier(objectIdentifierStr), result, 5, WalkMode.WithinSubtree, null, null);
            return result;
        }

        public async Task SnmpSetAsync(List<Variable> variables)
            => await Messenger.SetAsync(VersionCode.V2, ipEndPoint, writeCommunityString, variables);

        public async Task ReadConfigAsync()
        {
            if (accessVlanMembershipMethod == null)
            {
                LogDispatcher.E($"Couldn't read configuration of switch [{Label}], because no method is associated.");
                return;
            }
            ReadVlanConfigStatus = Status.Querying;
            LogDispatcher.I($"Reading configuration of switch [{Label}]...");
            LogDispatcher.V($"Method for reading configuration of switch [{Label}]: [{accessVlanMembershipMethod.Name}].");
            try
            {
                await accessVlanMembershipMethod.ReadConfigAsync();
                ReadVlanConfigStatus = Status.Successful;
                LogDispatcher.I($"Reading configuration of switch [{Label}] ready.");
            }
            catch (Exception ex)
            {
                ReadVlanConfigStatus = Status.Unsuccessful;
                LogDispatcher.E($"Unsuccessfull reading of configuration of switch [{Label}]. Error message: [{ex.Message}]");
            }
        }

        public async Task<bool> SetPortToVlanAsync(Port port, Vlan vlan)
        {
            if ((port.Switch != this) || !Ports.Contains(port))
                return false;
            if (accessVlanMembershipMethod == null)
            {
                LogDispatcher.E($"Couldn't set VLAN membership of port [{port.Label}] @ switch [{Label}], because no method is associated.");
                return false;
            }
            LogDispatcher.I($"Setting membership of port [{port.Label}] @ switch [{Label}] to VLAN [{vlan.Label}]...");
            LogDispatcher.V($"Method for setting VLAN membership of port [{port.Label}] @ switch [{Label}]: [{accessVlanMembershipMethod.Name}].");
            try
            {
                await accessVlanMembershipMethod.SetPortToVlanAsync(port, vlan);
                portUpdated(port);
                LogDispatcher.I($"Setting membership of port [{port.Label}] @ switch [{Label}] to VLAN [{vlan.Label}] ready.");
                return true;
            }
            catch (Exception ex)
            {
                LogDispatcher.E($"Unsuccessful setting of membership of port [{port.Label}] @ switch [{Label}] to VLAN [{vlan.Label}]. Error message: [{ex.Message}]");
                return false;
            }
        }

        private void portUpdated(Port port)
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
            PersistVlanConfigStatus = Status.Querying;
            LogDispatcher.I($"Persisting changes of switch [{Label}]...");
            LogDispatcher.V($"Method for persisting changes of switch [{Label}]: [{persistChangesMethod.Name}].");
            try
            {
                await persistChangesMethod.Do();
                PersistVlanConfigStatus = Status.Successful;
                notifyPortsChangesPersisted();
                LogDispatcher.I($"Persisting changes of switch [{Label}] ready.");
                return true;
            }
            catch (Exception ex)
            {
                PersistVlanConfigStatus = Status.Unsuccessful;
                LogDispatcher.E($"Didn't succeeded to persist changes of switch [{Label}] with method [{persistChangesMethod.Name}]. Error message: [{ex.Message}]");
            }
            return false;
        }

        private void notifyPortsChangesPersisted()
        {
            portsWithPendingChange.ForEach(up => up.ChangesPersisted());
            portsWithPendingChange.Clear();
            PortsWithPendingChangeCount = 0;
        }

    }

}
