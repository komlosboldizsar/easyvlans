using easyvlans.Logger;
using easyvlans.Model.Remote;
using easyvlans.Model.SwitchOperationMethods;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

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

        internal void AssociatePort(Port port)
        {
            if ((port.Switch == this) && !Ports.Contains(port))
                Ports.Add(port);
        }

        public async Task ReadConfigAsync()
        {
            if (OperationMethodCollection?.ReadConfigMethod == null)
            {
                LogDispatcher.E($"Couldn't read configuration of switch [{Label}], because no method is associated.");
                return;
            }
            ReadVlanConfigStatus = Status.Querying;
            LogDispatcher.I($"Reading configuration of switch [{Label}]...");
            LogDispatcher.V($"Method for reading configuration of switch [{Label}]: [{OperationMethodCollection.SetPortToVlanMethod.DetailedCode}].");
            try
            {
                await OperationMethodCollection.ReadConfigMethod.DoAsync();
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

        private void portUpdated(Port port)
        {
            if (portsWithPendingChange.Contains(port))
                return;
            portsWithPendingChange.Add(port);
            PortsWithPendingChangeCount++;
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

        private void notifyPortsChangesPersisted()
        {
            portsWithPendingChange.ForEach(up => up.ChangesPersisted());
            portsWithPendingChange.Clear();
            PortsWithPendingChangeCount = 0;
        }

    }

}
