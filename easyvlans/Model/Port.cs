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

        public delegate void StatusChangedDelegate(Port port, PortStatus newValue);
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
            Status = PortStatus.Unknown;
            Label = label;
            Switch = @switch;
            @switch.AssociatePort(this);
            Index = index;
            Vlans.AddRange(vlans);
            Page = page;
        }

        public async Task SetVlanTo(Vlan vlan)
        {
            if (!await Switch.SetPortToVlanAsync(this, vlan))
                return;
            CurrentVlan = vlan;
            PendingChanges = true;
        }

        internal void ChangesPersisted() => PendingChanges = false;

    }

}
