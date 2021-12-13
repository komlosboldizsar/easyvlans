using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Switch
    {

        public string ID { get; init; }
        public string Label { get; init; }
        public string IP { get; init; }
        private List<SwitchAccessMode> accessModes = new List<SwitchAccessMode>();
        private List<Port> ports = new List<Port>();
        private List<Port> portsWithPendingChange = new List<Port>();

        public delegate void PortsWithPendingChangeCountChangedDelegate(Switch @switch, int newValue);
        public event PortsWithPendingChangeCountChangedDelegate PortsWithPendingChangeCountChanged;
        private int _portsWithPendingChangeCount;
        public int PortsWithPendingChangeCount
        {
            get => _portsWithPendingChangeCount;
            private set
            {
                if (value == _portsWithPendingChangeCount)
                    return;
                _portsWithPendingChangeCount = value;
                PortsWithPendingChangeCountChanged?.Invoke(this, value);
            }
        }

        public Switch(string id, string label, string ip)
        {
            ID = id;
            Label = label;
            IP = ip;
        }

        public void AddAccessMode(SwitchAccessMode sam)
        {
            accessModes.Add(sam);
        }

        internal void AssociatePort(Port port)
        {
            if ((port.Switch == this) && !ports.Contains(port))
                ports.Add(port);
        }

        public void PersistChanges()
        {
            portsWithPendingChange.Clear();
            PortsWithPendingChangeCount = 0;
        }

    }

}
