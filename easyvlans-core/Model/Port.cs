using easyvlans.Model.Remote.Snmp;

namespace easyvlans.Model
{

    public class Port : IRemoteable
    {

        public string Label { get; init; }
        public int Index { get; init; }
        public int? RemoteIndex { get; init; }

        public Switch Switch;
        public List<Vlan> Vlans;
        public PortPage Page;

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

        public event PropertyChangedDelegate<Port, bool> HasComplexMembershipChanged;
        private bool _hasComplexMembership;
        public bool HasComplexMembership
        {
            get => _hasComplexMembership;
            set => this.setProperty(ref _hasComplexMembership, value, HasComplexMembershipChanged);
        }

        public event PropertyChangedDelegate<Port, bool> HasNotAllowedMembershipChanged;
        private bool _hasNotAllowedMembership;
        public bool HasNotAllowedMembership
        {
            get => _hasNotAllowedMembership;
            private set => this.setProperty(ref _hasNotAllowedMembership, value, HasNotAllowedMembershipChanged);
        }

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

        public event PropertyChangedDelegate<Port, DateTime> SetVlanMembershipStatusUpdateTimeChanged;
        private DateTime _setVlanMembershipStatusUpdateTime = DateTime.Now;
        public DateTime SetVlanMembershipStatusUpdateTime
        {
            get => _setVlanMembershipStatusUpdateTime;
            private set => this.setProperty(ref _setVlanMembershipStatusUpdateTime, value, SetVlanMembershipStatusUpdateTimeChanged);
        }

        public event PropertyChangedDelegate<Port, bool> PendingChangesChanged;
        private bool _pendingChanges;
        public bool PendingChanges
        {
            get => _pendingChanges;
            internal set => this.setProperty(ref _pendingChanges, value, PendingChangesChanged);
        }

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

        internal void ChangesPersisted() => PendingChanges = false;

    }

}
