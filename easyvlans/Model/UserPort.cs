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

        public UserPort(string label, Switch @switch, int index, IEnumerable<UserVlan> vlans)
        {
            Status = PortStatus.Unknown;
            Label = label;
            Switch = @switch;
            @switch.AssociatePort(this);
            Index = index;
            Vlans.AddRange(vlans);
        }

        public async Task SetVlanTo(UserVlan vlan)
        {
            await Switch.SetPortToVlanAsync(this, vlan);
            CurrentVlan = vlan;
        }

    }

}
