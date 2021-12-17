using easyvlans.Logger;
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
        public bool HasAccessMode => (accessModes.Count > 0);
        private List<Port> ports = new List<Port>();
        private List<Port> portsWithPendingChange = new List<Port>();
        private Config config;

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

        public delegate void StatusChangedDelegate(Switch @switch, SwitchStatus newValue);
        public event StatusChangedDelegate StatusChanged;
        private SwitchStatus _status;
        public SwitchStatus Status
        {
            get => _status;
            private set
            {
                if (value == _status)
                    return;
                _status = value;
                StatusChanged?.Invoke(this, value);
            }
        }

        public Switch(string id, string label, string ip)
        {
            Status = SwitchStatus.NotConnected;
            ID = id;
            Label = label;
            IP = ip;
        }

        internal void AssignConfig(Config config)
        {
            this.config = config;
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

        public async Task DoRemoteAction(Func<SwitchAccessMode, object, Task> actionBody, object tag = null)
        {
            if (accessModes.Count == 0)
            {
                Status = SwitchStatus.NoAccessMode;
                return;
            }
            try
            {
                SwitchAccessMode sam = accessModes[0];
                Status = SwitchStatus.Connecting;
                LogDispatcher.I($"Switch \"{Label}\" is connecting...");
                await sam.Connect();
                Status = SwitchStatus.Connected;
                await actionBody.Invoke(sam, tag);
            }
            catch (CouldNotConnectException)
            {
                Status = SwitchStatus.CantConnect;
                LogDispatcher.E($"Failed to connect to switch \"{Label}\".");
            }
        }

        public async Task ReadVlans() => await DoRemoteAction(_readVlans);

        private async Task _readVlans(SwitchAccessMode sam, object tag = null)
        {
            LogDispatcher.I($"Reading VLAN settings from switch \"{Label}\"...");
            sam.WriteLine("");
            sam.WriteLine("enable");
            sam.WriteLine("show interfaces status");
            List<string> lineBuffer = new List<string>();
            lineBuffer.AddRange(await sam.ReadLines());
            bool readingPortData = false;
            while (lineBuffer.Count > 0)
            {
                string line = lineBuffer[0];
                lineBuffer.RemoveAt(0);
                if (line.StartsWith("Port"))
                    readingPortData = true;
                if (readingPortData)
                {
                    if (line.Contains("More"))
                    {
                        sam.WriteLine("");
                    }
                    else
                    {
                        string[] columns = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (columns.Length >= 6)
                        {
                            string portId = columns[0];
                            string vlanIdStr = columns[^3];
                            Vlan vlan = null;
                            if (int.TryParse(vlanIdStr, out int vlanId))
                                config.Vlans.TryGetValue(vlanId, out vlan);
                            ports.FindAll(p => p.Index == portId).ForEach(p => {
                                p.CurrentVlan = vlan;
                                p.Status = PortStatus.VlanRead;
                                string endOfSentence = "";
                                if (vlanIdStr == "trunk")
                                    endOfSentence = "trunk";
                                else
                                    endOfSentence = $"assigned to VLAN \"{vlanId} ({vlan?.Name ?? "unknown"})\"";
                                LogDispatcher.I($"Port \"{p.Label}\" (\"{p.Index}\" on switch \"{Label}\") is {endOfSentence}.");
                            });
                        }
                    }
                }
            }
            Status = SwitchStatus.VlansRead;
            LogDispatcher.I($"Reading VLAN settings from switch \"{Label}\" ready.");
        }

        public async Task SetVlan(Port port, Vlan vlan) => await DoRemoteAction(_setVlan, Tuple.Create<Port, Vlan>(port, vlan));

        private async Task _setVlan(SwitchAccessMode sam, object tag)
        {
            Tuple<Port, Vlan> typedTag = tag as Tuple<Port, Vlan>;
            Port port = typedTag.Item1;
            Vlan vlan = typedTag.Item2;
            port.Status = PortStatus.SettingVlan;
            LogDispatcher.I($"Assigning port \"{port.Label}\" (\"{port.Index}\" on switch \"{Label}\") to VLAN \"{vlan.ID} ({vlan.Name})\"...");
            sam.WriteLine("");
            sam.WriteLine("enable");
            sam.WriteLine("configure terminal");
            sam.WriteLine($"interface {port.Index}");
            sam.WriteLine("switchport mode access");
            sam.WriteLine($"switchport access vlan {vlan.ID}");
            sam.WriteLine("exit");
            sam.WriteLine("exit");
            foreach (string line in await sam.ReadLines())
            {
                if (line.Contains("Configured from"))
                {
                    Status = SwitchStatus.PortVlanChanged;
                    LogDispatcher.I($"Assigning port \"{port.Label}\" (\"{port.Index}\" on switch \"{Label}\") to VLAN \"{vlan.ID} ({vlan.Name})\" ready.");
                    port.CurrentVlan = vlan;
                    port.Status = PortStatus.VlanSetNotPersisted;
                    if (!portsWithPendingChange.Contains(port) && ports.Contains(port))
                        portsWithPendingChange.Add(port);
                    PortsWithPendingChangeCount = portsWithPendingChange.Count;
                    return;
                }
            }
            Status = SwitchStatus.PortVlanChangeError;
            port.Status = PortStatus.VlanSetFailed;
            LogDispatcher.E($"Assigning port \"{port.Label}\" (\"{port.Index}\" on switch \"{Label}\") to VLAN \"{vlan.ID} ({vlan.Name})\" failed.");
        }

        public async Task PersistConfig() => await DoRemoteAction(_persistConfig);

        private async Task _persistConfig(SwitchAccessMode sam, object tag = null)
        {
            LogDispatcher.I($"Copying running-config to startup-config on switch \"{Label}\"...");
            sam.WriteLine("");
            sam.WriteLine("enable");
            sam.WriteLine("copy running-config startup-config");
            sam.WriteLine("");
            sam.WriteLine("exit");
            foreach (string line in await sam.ReadLines())
            {
                if (line.Contains("[OK]"))
                {
                    Status = SwitchStatus.ConfigSaved;
                    LogDispatcher.I($"Copying running-config to startup-config on switch \"{Label}\" ready.");
                    foreach (Port port in ports)
                        port.Status = PortStatus.VlanSetPersisted;
                    portsWithPendingChange.Clear();
                    PortsWithPendingChangeCount = 0;
                    return;
                }
            }
            Status = SwitchStatus.ConfigSaveError;
            LogDispatcher.E($"Copying running-config to startup-config on switch \"{Label}\" failed.");
        }

    }

}
