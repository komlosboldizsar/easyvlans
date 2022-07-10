using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class UserPort
    {

        public string Label { get; init; }
        public Switch Switch { get; init; }
        public int Index { get; init; }
        public List<UserVlan> Vlans { get; } = new List<UserVlan>();
        public UserPortPage Page { get; init; }

        public delegate void CurrentVlanChangedDelegate(UserPort port, UserVlan newValue);
        public event CurrentVlanChangedDelegate CurrentVlanChanged;
        private UserVlan _currentVlan;
        public UserVlan CurrentVlan
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

        public delegate void HasComplexMembershipChangedDelegate(UserPort port, bool newValue);
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

        public delegate void StatusChangedDelegate(UserPort port, PortStatus newValue);
        public event StatusChangedDelegate StatusChanged;
        private PortStatus _status;
        public PortStatus Status
        {
            get => _status;
            internal set
            {
                if (value == _status)
                    return;
                _status = value;
                StatusChanged?.Invoke(this, value);
            }
        }

        public delegate void PendingChangesChangedDelegate(UserPort port, bool newValue);
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

        public UserPort(string label, Switch @switch, int index, IEnumerable<UserVlan> vlans, UserPortPage page)
        {
            Status = PortStatus.Unknown;
            Label = label;
            Switch = @switch;
            @switch.AssociatePort(this);
            Index = index;
            Vlans.AddRange(vlans);
            Page = page;
        }

        public async Task SetVlanTo(UserVlan vlan)
        {
            if (!await Switch.SetPortToVlanAsync(this, vlan))
                return;
            CurrentVlan = vlan;
            PendingChanges = true;
        }

        internal void ChangesPersisted() => PendingChanges = false;

    }

}
