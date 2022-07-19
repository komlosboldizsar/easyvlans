using easyvlans.Model.Remote.Snmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Port : IRemoteable
    {

        public readonly string Label;
        public readonly Switch Switch;
        public readonly int Index;
        public readonly List<Vlan> Vlans = new();
        public readonly PortPage Page;
        public int? RemoteIndex { get; init; }

        public event PropertyChangedDelegate<Port, Vlan> CurrentVlanChanged;
        private Vlan _currentVlan;
        public Vlan CurrentVlan
        {
            get => _currentVlan;
            set => this.setProperty(ref _currentVlan, value, CurrentVlanChanged);
        }

        public event PropertyChangedDelegate<Port, bool> HasComplexMembershipChanged;
        private bool _hasComplexMembership;
        public bool HasComplexMembership
        {
            get => _hasComplexMembership;
            internal set => this.setProperty(ref _hasComplexMembership, value, HasComplexMembershipChanged);
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

        public Port(string label, Switch @switch, int index, IEnumerable<Vlan> vlans, PortPage page, int? remoteIndex)
        {
            Label = label;
            Switch = @switch;
            @switch.AssociatePort(this);
            Index = index;
            Vlans.AddRange(vlans);
            Page = page;
            RemoteIndex = remoteIndex;
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
