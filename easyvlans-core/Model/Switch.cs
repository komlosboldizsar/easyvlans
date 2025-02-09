using BToolbox.Logger;
using easyvlans.Model.Remote;
using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Model
{

    public class Switch : IRemoteable, ISetProperty
    {

        public string ID { get; init; }
        public string Label { get; init; }
        public int? RemoteIndex { get; init; }

        public List<Vlan> Vlans;

        public ISwitchOperationMethodCollection OperationMethodCollection { get; set; }

        public readonly List<Port> Ports = new();
        private readonly List<Port> portsWithPendingChange = new();

        public event PropertyChangedDelegate<Switch, DateTime> BoottimeChanged;
        private DateTime _boottime;
        public DateTime Boottime
        {
            get => _boottime;
            set
            {
                this.setProperty(ref _boottime, value, BoottimeChanged);
                foreach (Port port in Ports)
                    port.SwitchBoottimeChanged();
            }
        }

        public Config Config { get; set; }

        internal void AssociatePort(Port port)
        {
            if ((port.Switch == this) && !Ports.Contains(port))
                Ports.Add(port);
        }

        public Port GetPort(int index)
            => Ports.FirstOrDefault(p => p.Index == index);

        #region Method: Read switch boottime
        public event PropertyChangedDelegate<Switch, Status> ReadBoottimeStatusChanged;
        private Status _readBoottimeStatusChanged = Status.Unknown;
        public Status ReadBootimeStatus
        {
            get => _readBoottimeStatusChanged;
            private set
            {
                if (this.setProperty(ref _readBoottimeStatusChanged, value, ReadBoottimeStatusChanged))
                    ReadBoottimeStatusUpdateTime = DateTime.Now;
            }
        }

        public event PropertyChangedDelegate<Switch, DateTime> ReadBoottimeStatusUpdateTimeChanged;
        private DateTime _readBoottimeStatusUpdateTime = DateTime.Now;
        public DateTime ReadBoottimeStatusUpdateTime
        {
            get => _readBoottimeStatusUpdateTime;
            private set => this.setProperty(ref _readBoottimeStatusUpdateTime, value, ReadBoottimeStatusUpdateTimeChanged);
        }

        public async Task ReadBoottimeAsync()
        {
            if (OperationMethodCollection?.ReadSwitchBoottimeMethod == null)
            {
                LogDispatcher.E($"Couldn't read boot time of switch [{Label}], because no method is associated.");
                return;
            }
            ReadBootimeStatus = Status.Querying;
            LogDispatcher.I($"Reading boot time of switch [{Label}]...");
            LogDispatcher.V($"Method for reading uptime of switch [{Label}]: [{OperationMethodCollection.ReadSwitchBoottimeMethod.DetailedCode}].");
            try
            {
                await OperationMethodCollection.ReadSwitchBoottimeMethod.DoAsync();
                ReadBootimeStatus = Status.Successful;
                LogDispatcher.I($"Reading boot time of switch [{Label}] ready.");
            }
            catch (Exception ex)
            {
                ReadBootimeStatus = Status.Unsuccessful;
                LogDispatcher.E($"Unsuccessful reading of boot time of switch [{Label}]. Error message: [{ex.Message}]");
            }
        }
        #endregion

        #region Method: Read interface status
        public Task ReadInterfaceStatusAsync(Port port)
            => ReadInterfaceStatusAsync(new Port[] { port });

        public async Task ReadInterfaceStatusAsync(IEnumerable<Port> ports = null)
        {
            if (OperationMethodCollection?.ReadInterfaceStatusMethod == null)
            {
                LogDispatcher.E($"Couldn't read interface statuses of switch [{Label}], because no method is associated.");
                return;
            }
            ReadVlanConfigStatus = Status.Querying;
            LogDispatcher.I($"Reading interface statuses of switch [{Label}]...");
            LogDispatcher.V($"Method for reading interface statuses of switch [{Label}]: [{OperationMethodCollection.ReadInterfaceStatusMethod.DetailedCode}].");
            try
            {
                await OperationMethodCollection.ReadInterfaceStatusMethod.DoAsync(ports);
                ReadVlanConfigStatus = Status.Successful;
                LogDispatcher.I($"Reading interface statuses of switch [{Label}] ready.");
            }
            catch (Exception ex)
            {
                ReadVlanConfigStatus = Status.Unsuccessful;
                LogDispatcher.E($"Unsuccessful reading of interface statuses of switch [{Label}]. Error message: [{ex.Message}]");
            }
        }
        #endregion

        #region Method: Read VLAN membership
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

        public Task ReadVlanMembershipAsync(Port port)
            => ReadVlanMembershipAsync(new Port[] { port });

        public async Task ReadVlanMembershipAsync(IEnumerable<Port> ports = null)
        {
            if (OperationMethodCollection?.ReadVlanMembershipMethod == null)
            {
                LogDispatcher.E($"Couldn't read VLAN memberships of switch [{Label}], because no method is associated.");
                return;
            }
            ReadVlanConfigStatus = Status.Querying;
            LogDispatcher.I($"Reading VLAN memberships of switch [{Label}]...");
            LogDispatcher.V($"Method for reading VLAN memberships of switch [{Label}]: [{OperationMethodCollection.ReadVlanMembershipMethod.DetailedCode}].");
            try
            {
                await OperationMethodCollection.ReadVlanMembershipMethod.DoAsync(ports);
                ReadVlanConfigStatus = Status.Successful;
                LogDispatcher.I($"Reading VLAN memberships of switch [{Label}] ready.");
            }
            catch (Exception ex)
            {
                ReadVlanConfigStatus = Status.Unsuccessful;
                LogDispatcher.E($"Unsuccessful reading of VLAN memberships of switch [{Label}]. Error message: [{ex.Message}]");
            }
        }
        #endregion

        #region Method: Set VLAN membership
        public async Task<bool> SetPortToVlanAsync(Port port, Vlan vlan)
        {
            if ((port.Switch != this) || !Ports.Contains(port))
                return false;
            if (OperationMethodCollection?.SetPortToVlanMethod == null)
            {
                LogDispatcher.E($"Couldn't set VLAN membership of port [{port.Label}] @ switch [{Label}], because no method is associated.");
                return false;
            }
            LogDispatcher.I($"Setting membership of port [{port.Label}] @ switch [{Label}] to VLAN [{vlan.Label}]...");
            LogDispatcher.V($"Method for setting VLAN membership of port [{port.Label}] @ switch [{Label}]: [{OperationMethodCollection.SetPortToVlanMethod.DetailedCode}].");
            try
            {
                await OperationMethodCollection.SetPortToVlanMethod.DoAsync(port, vlan);
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
        #endregion

        #region Method: Persist changes
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

        public async Task<bool> PersistChangesAsync()
        {
            if (OperationMethodCollection?.PersistChangesMethod == null)
            {
                LogDispatcher.E($"Couldn't persist changes of switch [{Label}], because no method is associated.");
                return false;
            }
            PersistVlanConfigStatus = Status.Querying;
            LogDispatcher.I($"Persisting changes of switch [{Label}]...");
            LogDispatcher.V($"Method for persisting changes of switch [{Label}]: [{OperationMethodCollection?.PersistChangesMethod.DetailedCode ?? "-"}].");
            try
            {
                await OperationMethodCollection.PersistChangesMethod.DoAsync();
                PersistVlanConfigStatus = Status.Successful;
                notifyPortsChangesPersisted();
                LogDispatcher.I($"Persisting changes of switch [{Label}] ready.");
                return true;
            }
            catch (Exception ex)
            {
                PersistVlanConfigStatus = Status.Unsuccessful;
                LogDispatcher.E($"Didn't succeeded to persist changes of switch [{Label}] with method [{OperationMethodCollection?.PersistChangesMethod.DetailedCode ?? "-"}]. Error message: [{ex.Message}]");
            }
            return false;
        }
        #endregion

        #region Port change handler
        public event PropertyChangedDelegate<Switch, int> PortsWithPendingChangeCountChanged;
        private int _portsWithPendingChangeCount;
        public int PortsWithPendingChangeCount
        {
            get => _portsWithPendingChangeCount;
            private set => this.setProperty(ref _portsWithPendingChangeCount, value, PortsWithPendingChangeCountChanged);
        }

        private void portUpdated(Port port)
        {
            if (portsWithPendingChange.Contains(port))
                return;
            portsWithPendingChange.Add(port);
            PortsWithPendingChangeCount++;
        }

        private void notifyPortsChangesPersisted()
        {
            portsWithPendingChange.ForEach(up => up.ChangesPersisted());
            portsWithPendingChange.Clear();
            PortsWithPendingChangeCount = 0;
        }
        #endregion

    }

}
