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
        public string Index { get; init; }
        public List<Vlan> Vlans { get; } = new List<Vlan>();

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

        public Port(string label, Switch @switch, string index, IEnumerable<Vlan> vlans)
        {
            Status = PortStatus.Unknown;
            Label = label;
            Switch = @switch;
            @switch.AssociatePort(this);
            Index = index;
            Vlans.AddRange(vlans);
        }

        public void SetVlanTo(Vlan vlan)
        {
            Switch.SetVlan(this, vlan);
            CurrentVlan = vlan;
        }

    }

}
