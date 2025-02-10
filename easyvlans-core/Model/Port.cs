using easyvlans.Model.Remote;

namespace easyvlans.Model
{
    public class Port : IPortOrPortCollection, IRemoteable, ISetProperty
    {

        public string Label { get; init; }
        public int Index { get; init; }
        public int? RemoteIndex { get; init; }

        public Switch Switch;
        public PortCollection Collection;
        public Vlan DefaultVlan;

        #region Property: AdministrativeStatus
        public event PropertyChangedDelegate<Port, PortStatus> AdministrativeStatusChanged;
        private PortStatus _administrativeStatus = PortStatus.Unknown;
        public PortStatus AdministrativeStatus
        {
            get => _administrativeStatus;
            set => this.setProperty(ref _administrativeStatus, value, AdministrativeStatusChanged);
        }
        #endregion

        #region Property: AdministrativeStatusString
        public event PropertyChangedDelegate<Port, string> AdministrativeStatusStringChanged;
        private string _administrativeStatusString;
        public string AdministrativeStatusString
        {
            get => _administrativeStatusString;
            set => this.setProperty(ref _administrativeStatusString, value, AdministrativeStatusStringChanged);
        }
        #endregion

        #region Property: OperationalStatus
        public event PropertyChangedDelegate<Port, PortStatus> OperationalStatusChanged;
        private PortStatus _operationalStatus = PortStatus.Unknown;
        public PortStatus OperationalStatus
        {
            get => _operationalStatus;
            set => this.setProperty(ref _operationalStatus, value, OperationalStatusChanged);
        }
        #endregion

        #region Property: OperationalStatusString
        public event PropertyChangedDelegate<Port, string> OperationalStatusStringChanged;
        private string _operationalStatusString;
        public string OperationalStatusString
        {
            get => _operationalStatusString;
            set => this.setProperty(ref _operationalStatusString, value, OperationalStatusStringChanged);
        }
        #endregion

        #region Property: Speed
        public event PropertyChangedDelegate<Port, long?> SpeedChanged;
        private long? _speed = null;
        public long? Speed
        {
            get => _speed;
            set => this.setProperty(ref _speed, value, SpeedChanged);
        }
        #endregion

        #region Property: LastStatusChange
        public event PropertyChangedDelegate<Port, LastStatusChangeData> LastStatusChangeChanged;
        private LastStatusChangeData _lastStatusChange;
        public LastStatusChangeData LastStatusChange
        {
            get => _lastStatusChange;
            private set => this.setProperty(ref _lastStatusChange, value, LastStatusChangeChanged);
        }

        public record LastStatusChangeData(DateTime? Timestamp, LastStatusChangeSourceType Source);

        public enum LastStatusChangeSourceType
        {
            Absolute,
            BoottimeRelative
        }

        public void LastStatusChangeUpdateAbsolute(DateTime timestamp)
            => LastStatusChange = new(timestamp, LastStatusChangeSourceType.Absolute);

        private void calculateLastStatusChangeByBoottime()
            => LastStatusChange = new(Switch.Boottime + _lastStatusChangeSinceBoot, LastStatusChangeSourceType.BoottimeRelative);

        private TimeSpan? _lastStatusChangeSinceBoot;
        public void LastStatucChangeUpdateBootimeRelative(TimeSpan? relativeToBoot)
        {
            if (relativeToBoot == _lastStatusChangeSinceBoot)
                return;
            _lastStatusChangeSinceBoot = relativeToBoot;
            calculateLastStatusChangeByBoottime();
        }

        internal void SwitchBoottimeChanged()
            => calculateLastStatusChangeByBoottime();
        #endregion

        #region Vlans
        private readonly List<Vlan> _vlans = new();
        public List<Vlan> Vlans
        {
            get => _vlans;
            set
            {
                _vlans.Clear();
                if (Switch == null)
                    return;
                _vlans.AddRange(value.Intersect(Switch.Vlans));
            }
        }
        #endregion

        #region Property: CurrentVlan
        public event PropertyChangedDelegate<Port, Vlan> CurrentVlanChanged;
        private Vlan _currentVlan;
        public Vlan CurrentVlan
        {
            get => _currentVlan;
            set
            {
                if (this.setProperty(ref _currentVlan, value, CurrentVlanChanged))
                    HasNotAllowedMembership = (value != null) && !Vlans.Contains(value);
            }
        }
        #endregion

        #region Property: HasComplexMembership
        public event PropertyChangedDelegate<Port, bool> HasComplexMembershipChanged;
        private bool _hasComplexMembership;
        public bool HasComplexMembership
        {
            get => _hasComplexMembership;
            set => this.setProperty(ref _hasComplexMembership, value, HasComplexMembershipChanged);
        }
        #endregion

        #region Property: HasNotAllowedMembership
        public event PropertyChangedDelegate<Port, bool> HasNotAllowedMembershipChanged;
        private bool _hasNotAllowedMembership;
        public bool HasNotAllowedMembership
        {
            get => _hasNotAllowedMembership;
            private set => this.setProperty(ref _hasNotAllowedMembership, value, HasNotAllowedMembershipChanged);
        }
        #endregion

        #region Property: SetVlanMembershipStatus
        public event PropertyChangedDelegate<Port, Status> SetVlanMembershipStatusChanged;
        private Status _setVlanMembershipStatus = Status.Empty;
        public Status SetVlanMembershipStatus
        {
            get => _setVlanMembershipStatus;
            private set
            {
                if (this.setProperty(ref _setVlanMembershipStatus, value, SetVlanMembershipStatusChanged))
                    SetVlanMembershipStatusUpdateTime = DateTime.Now;
            }
        }
        #endregion

        #region Property: SetVlanMembershipStatusUpdateTime
        public event PropertyChangedDelegate<Port, DateTime> SetVlanMembershipStatusUpdateTimeChanged;
        private DateTime _setVlanMembershipStatusUpdateTime = DateTime.Now;
        public DateTime SetVlanMembershipStatusUpdateTime
        {
            get => _setVlanMembershipStatusUpdateTime;
            private set => this.setProperty(ref _setVlanMembershipStatusUpdateTime, value, SetVlanMembershipStatusUpdateTimeChanged);
        }
        #endregion

        #region Property: PendingChanges
        public event PropertyChangedDelegate<Port, bool> PendingChangesChanged;
        private bool _pendingChanges;
        public bool PendingChanges
        {
            get => _pendingChanges;
            internal set => this.setProperty(ref _pendingChanges, value, PendingChangesChanged);
        }
        #endregion

        public async Task<bool> SetVlanTo(Vlan vlan)
        {
            SetVlanMembershipStatus = Status.Querying;
            if (!await Switch.SetPortToVlanAsync(this, vlan))
            {
                SetVlanMembershipStatus = Status.Unsuccessful;
                return false;
            }
            SetVlanMembershipStatus = Status.Successful;
            CurrentVlan = vlan;
            PendingChanges = true;
            return true;
        }

        internal void ChangesPersisted()
            => PendingChanges = false;

    }
}
