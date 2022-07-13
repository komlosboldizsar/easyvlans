using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Port
    {

        public string Label { get; init; }
        public Switch Switch { get; init; }
        public int Index { get; init; }
        public List<Vlan> Vlans { get; } = new List<Vlan>();
        public PortPage Page { get; init; }

        public delegate void CurrentVlanChangedDelegate(Port port, Vlan newValue);
        public event CurrentVlanChangedDelegate CurrentVlanChanged;
        private Vlan _currentVlan;
        public Vlan CurrentVlan
        {
            get => _currentVlan;
            set
            {
                if (value == _currentVlan)
                    return;
                _currentVlan = value;
                CurrentVlanChanged?.Invoke(this, value);
            }
        }

        public delegate void HasComplexMembershipChangedDelegate(Port port, bool newValue);
        public event HasComplexMembershipChangedDelegate HasComplexMembershipChanged;
        private bool _hasComplexMembership;
        public bool HasComplexMembership
        {
            get => _hasComplexMembership;
            internal set
            {
                if (value == _hasComplexMembership)
                    return;
                _hasComplexMembership = value;
                HasComplexMembershipChanged?.Invoke(this, value);
            }
        }

        public delegate void PersistConfigStatusChangedDelegate(Port port, Status newValue);
        public event PersistConfigStatusChangedDelegate SetVlanMembershipStatusChanged;
        private Status _setVlanMembershipStatus = Status.Empty;
        public Status SetVlanMembershipStatus
        {
            get => _setVlanMembershipStatus;
            private set
            {
                if (value == _setVlanMembershipStatus)
                    return;
                _setVlanMembershipStatus = value;
                SetVlanMembershipStatusChanged?.Invoke(this, value);
                SetVlanMembershipStatusUpdateTime = DateTime.Now;
            }
        }

        public delegate void SetVlanMembershipStatusUpdateTimeChangedDelegate(Port port, DateTime newValue);
        public event SetVlanMembershipStatusUpdateTimeChangedDelegate SetVlanMembershipStatusUpdateTimeChanged;
        private DateTime _setVlanMembershipStatusUpdateTime = DateTime.Now;
        public DateTime SetVlanMembershipStatusUpdateTime
        {
            get => _setVlanMembershipStatusUpdateTime;
            private set
            {
                if (value == _setVlanMembershipStatusUpdateTime)
                    return;
                _setVlanMembershipStatusUpdateTime = value;
                SetVlanMembershipStatusUpdateTimeChanged?.Invoke(this, value);
            }
        }

        public delegate void PendingChangesChangedDelegate(Port port, bool newValue);
        public event PendingChangesChangedDelegate PendingChangesChanged;
        private bool _pendingChanges;
        public bool PendingChanges
        {
            get => _pendingChanges;
            internal set
            {
                if (value == _pendingChanges)
                    return;
                _pendingChanges = value;
                PendingChangesChanged?.Invoke(this, value);
            }
        }

        public Port(string label, Switch @switch, int index, IEnumerable<Vlan> vlans, PortPage page)
        {
            Label = label;
            Switch = @switch;
            @switch.AssociatePort(this);
            Index = index;
            Vlans.AddRange(vlans);
            Page = page;
        }

        public async Task SetVlanTo(Vlan vlan)
        {
            SetVlanMembershipStatus = Status.Querying;
            if (!await Switch.SetPortToVlanAsync(this, vlan))
            {
                SetVlanMembershipStatus = Status.Unsuccessful;
                return;
            }
            SetVlanMembershipStatus = Status.Successful;
            CurrentVlan = vlan;
            PendingChanges = true;
        }

        internal void ChangesPersisted() => PendingChanges = false;

    }

}
