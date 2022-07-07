using easyvlans.GUI;
using easyvlans.GUI.Helpers;
using easyvlans.GUI.Helpers.DropDowns;
using easyvlans.Logger;
using easyvlans.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class MainForm : Form
    {

        private Config config;
        private string parsingError;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(Config config, string parsingError)
        {
            LogDispatcher.NewLogMessage += addLogMessage;
            this.config = config;
            this.parsingError = parsingError;
            Load += load;
            InitializeComponent();
        }

        private void addLogMessage(DateTime timestamp, LogMessageSeverity severity, string message)
        {
            if (!showVerboseLog.Checked && (severity == LogMessageSeverity.Verbose))
                return;
            string textToAdd = $"[{timestamp.ToString("HH:mm:ss")}] {message}\r\n";
            logTextBox.AppendText(textToAdd);
            int textLength = logTextBox.TextLength;
            int selectionLength = textToAdd.Length;
            int selectionStart = textLength - selectionLength + 1;
            if (selectionStart < 0)
            {
                selectionStart = 0;
                selectionLength = 0;
            }
            logTextBox.Select(selectionStart, selectionLength);
            logTextBox.SelectionColor = logColors[severity];
            logTextBox.Select(textLength - 1, 0);
            logTextBox.ScrollToCaret();
        }

        private void showVerboseLogCheckedChanged(object sender, EventArgs e) => reloadLogMessages();

        private void reloadLogMessages()
        {
            logTextBox.Text = "";
            foreach (LogMessage logMessage in LogDispatcher.Messages)
                addLogMessage(logMessage.Timestamp, logMessage.Severity, logMessage.Message);
        }

        private Dictionary<LogMessageSeverity, Color> logColors = new Dictionary<LogMessageSeverity, Color>()
        {
            { LogMessageSeverity.Error, Color.Red },
            { LogMessageSeverity.Warning, Color.Orange },
            { LogMessageSeverity.Info, Color.Black },
            { LogMessageSeverity.Verbose, Color.LightBlue }
        };

        private async void load(object sender, EventArgs e)
        {

            reloadLogMessages();

            string errorToShow = parsingError;
            if ((errorToShow == null) && (config == null))
                errorToShow = "Couldn't load configuration, reason unknown.";
            if (errorToShow != null)
            {
                rowPortPortLabel.Text = "N/A";
                rowPortPortLabel.ForeColor = Color.Red;
                rowPortSwitch.Text = "N/A";
                rowPortPortId.Text = "N/A";
                rowPortCurrentVlan.Text = "N/A";
                rowPortSetVlanTo.Enabled = false;
                rowPortSet.Enabled = false;
                rowPortState.Text = "N/A";
                rowSwitchSwitchName.Text = "N/A";
                rowSwitchSwitchName.ForeColor = Color.Red;
                rowSwitchPendingChanges.Text = "N/A";
                rowSwitchPersistChanges.Enabled = false;
                rowSwitchState.Text = "N/A";
                MessageBox.Show(errorToShow, "Initialization error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            showPorts();
            showSwitches();
            Task[] allReadVlansTask = new Task[config.Switches.Count];
            int i = 0;
            foreach (Switch @switch in config.Switches.Values)
                allReadVlansTask[i++] = @switch.ReadConfigAsync();
            await Task.WhenAll(allReadVlansTask);

        }

        private void showPorts()
        {

            if (config.Ports.Count == 0)
            {
                // Todo...
                return;
            }

            int portRow = 0;
            int portRowHeight = (int)portTable.RowStyles[1].Height;
            foreach (UserPort port in config.Ports)
            {

                int portTableRow = portRow + 1;
                PortRowControls thisPortRowControls = getPortRowControls(portRow);
                portRowControls.Add(thisPortRowControls);
                portAssociatedRowControls.Add(port, thisPortRowControls);

                if (portRow > 0)
                {
                    portTable.RowCount++;
                    portTable.RowStyles.Add(new RowStyle(SizeType.Absolute, portRowHeight));
                    portTable.Controls.Add(thisPortRowControls.PortLabel, 0, portTableRow);
                    portTable.Controls.Add(thisPortRowControls.Switch, 1, portTableRow);
                    portTable.Controls.Add(thisPortRowControls.PortId, 2, portTableRow);
                    portTable.Controls.Add(thisPortRowControls.CurrentVlan, 3, portTableRow);
                    portTable.Controls.Add(thisPortRowControls.SetVlanTo, 4, portTableRow);
                    portTable.Controls.Add(thisPortRowControls.Set, 5, portTableRow);
                    portTable.Controls.Add(thisPortRowControls.State, 6, portTableRow);
                    Size = new Size(Size.Width, Size.Height + portRowHeight);
                }

                thisPortRowControls.PortLabel.Text = port.Label;
                thisPortRowControls.Switch.Text = port.Switch.Label;
                thisPortRowControls.PortId.Text = port.Index.ToString();
                thisPortRowControls.CurrentVlan.Text = CURRENT_VLAN_UNKNOWN;
                thisPortRowControls.CurrentVlan.ForeColor = COLOR_NO_PENDING_CHANGES;
                thisPortRowControls.SetVlanTo.Tag = port;
                //thisPortRowControls.SetVlanTo.Enabled = port.Switch.HasAccessMode;
                thisPortRowControls.Set.Tag = port;
                thisPortRowControls.Set.Enabled = false;
                thisPortRowControls.State.Text = portStatusStrings[port.Status];
                thisPortRowControls.State.ForeColor = portStatusColors[port.Status];
                port.StatusChanged += portsStatusChangedHandler;
                port.PendingChangesChanged += portPendingChangesChangedHandler;

                portRow++;

            }

            portRow = 0;
            foreach (UserPort port in config.Ports)
                portRowControls[portRow++].SetVlanTo.CreateAdapterAsDataSource(port.Vlans, vlanToStr, true, "");

            foreach (UserPort port in config.Ports)
            {
                port.CurrentVlanChanged += portsCurrentVlanChangedHandler;
                PortRowControls rowControls = portAssociatedRowControls[port];
                rowControls.SetVlanTo.SelectedIndexChanged += portsSetVlanToSelectedIndexChangedHandler;
                rowControls.Set.Click += portsSetButtonClickHandler;
            }

        }

        private void portsStatusChangedHandler(UserPort port, PortStatus newValue)
        {
            Label thisPortStateControl = portAssociatedRowControls[port].State;
            thisPortStateControl.Text = portStatusStrings[newValue];
            thisPortStateControl.ForeColor = portStatusColors[newValue];
        }

        private void portPendingChangesChangedHandler(UserPort port, bool newValue)
        {
            Label thisPortCurrentVlanControl = portAssociatedRowControls[port].CurrentVlan;
            thisPortCurrentVlanControl.ForeColor = newValue ? COLOR_HAS_PENDING_CHANGES : COLOR_NO_PENDING_CHANGES;
        }

        private void portsCurrentVlanChangedHandler(UserPort port, UserVlan newValue)
        {
            PortRowControls rowControls = portAssociatedRowControls[port];
            rowControls.CurrentVlan.Text = vlanToStr(newValue);
            rowControls.SetVlanTo.SelectedIndex = 0;
        }

        private void portsSetVlanToSelectedIndexChangedHandler(object sender, EventArgs e)
        {
            ComboBox typedSender = sender as ComboBox;
            UserPort port = typedSender?.Tag as UserPort;
            if (port == null)
                return;
            portAssociatedRowControls[port].Set.Enabled = (typedSender.SelectedIndex > 0);
        }

        private async void portsSetButtonClickHandler(object sender, EventArgs e)
        {
            Button typedSender = sender as Button;
            UserPort port = typedSender?.Tag as UserPort;
            if (port == null)
                return;
            UserVlan selectedVlan = portAssociatedRowControls[port].SetVlanTo.SelectedValue as UserVlan;
            port.SetVlanTo(selectedVlan);
        }

        private void showSwitches()
        {

            if (config.Switches.Count == 0)
            {
                // Todo...
                return;
            }

            int switchRow = 0;
            int switchRowHeight = (int)switchTable.RowStyles[1].Height;
            foreach (Switch @switch in config.Switches.Values)
            {

                int switchTableRow = switchRow + 1;
                SwitchRowControls thisSwitchRowControls = getSwitchRowControls(switchRow);
                switchRowControls.Add(thisSwitchRowControls);
                switchAssociatedRowControls.Add(@switch, thisSwitchRowControls);

                if (switchRow > 0)
                {
                    switchTable.RowCount++;
                    switchTable.RowStyles.Add(new RowStyle(SizeType.Absolute, switchRowHeight));
                    switchTable.Controls.Add(thisSwitchRowControls.SwitchName, 0, switchTableRow);
                    switchTable.Controls.Add(thisSwitchRowControls.PendingChanges, 1, switchTableRow);
                    switchTable.Controls.Add(thisSwitchRowControls.PersistChanges, 2, switchTableRow);
                    switchTable.Controls.Add(thisSwitchRowControls.State, 3, switchTableRow);
                    Size = new Size(Size.Width, Size.Height + switchRowHeight);
                }

                thisSwitchRowControls.SwitchName.Text = @switch.Label;
                thisSwitchRowControls.PendingChanges.Text = "no ports changed";
                thisSwitchRowControls.PersistChanges.Tag = @switch;
                thisSwitchRowControls.PersistChanges.Enabled = false;
                thisSwitchRowControls.PersistChanges.Click += switchesPersistChangesButtonClickHandler;
                thisSwitchRowControls.State.Text = switchStatusStrings[@switch.Status];
                thisSwitchRowControls.State.ForeColor = switchStatusColors[@switch.Status];
                @switch.StatusChanged += switchesStatusChangedHandler;
                @switch.PortsWithPendingChangeCountChanged += switchesPortsWithPendingChangeCountChangedHandler;

                switchRow++;

            }

        }

        private void switchesStatusChangedHandler(Switch @switch, SwitchStatus newValue)
        {
            Label thisSwitchStateControl = switchAssociatedRowControls[@switch].State;
            thisSwitchStateControl.Text = switchStatusStrings[newValue];
            thisSwitchStateControl.ForeColor = switchStatusColors[newValue];
        }

        private void switchesPortsWithPendingChangeCountChangedHandler(Switch @switch, int newValue)
        {
            SwitchRowControls thisSwitchRowControls = switchAssociatedRowControls[@switch];
            string newText = "no ports changed";
            if (newValue == 0)
            {
                thisSwitchRowControls.PendingChanges.Text = newText;
                thisSwitchRowControls.PendingChanges.ForeColor = COLOR_NO_PENDING_CHANGES;
                thisSwitchRowControls.PersistChanges.Enabled = false;
            }
            else
            {
                newText = (newValue > 1) ? $"{newValue} ports changed" : "1 port changed";
                thisSwitchRowControls.PendingChanges.Text = newText;
                thisSwitchRowControls.PendingChanges.ForeColor = COLOR_HAS_PENDING_CHANGES;
                thisSwitchRowControls.PersistChanges.Enabled = true;
            }
        }

        private async void switchesPersistChangesButtonClickHandler(object sender, EventArgs e)
        {
            Button typedSender = sender as Button;
            Switch @switch = typedSender?.Tag as Switch;
            await @switch?.PersistChangesAsync();
        }

        private string vlanToStr(UserVlan vlan) => $"{vlan.ID} - {vlan.Name}";

        public class PortRowControls
        {
            public Label PortLabel { get; init; }
            public Label Switch { get; init; }
            public Label PortId { get; init; }
            public Label CurrentVlan { get; init; }
            public ComboBox SetVlanTo { get; init; }
            public Button Set { get; init; }
            public Label State { get; init; }
        }

        public class SwitchRowControls
        {
            public Label SwitchName { get; init; }
            public Label PendingChanges { get; init; }
            public Button PersistChanges { get; init; }
            public Label State { get; init; }
        }

        private List<PortRowControls> portRowControls = new List<PortRowControls>();
        private Dictionary<UserPort, PortRowControls> portAssociatedRowControls = new Dictionary<UserPort, PortRowControls>();

        private List<SwitchRowControls> switchRowControls = new List<SwitchRowControls>();
        private Dictionary<Switch, SwitchRowControls> switchAssociatedRowControls = new Dictionary<Switch, SwitchRowControls>();

        private T cloneOrOriginal<T>(T originalControl, int row)
            where T : Control
            => (row == 0) ? originalControl : originalControl.Clone();

        private PortRowControls getPortRowControls(int portRow)
        {
            return new PortRowControls()
            {
                PortLabel = cloneOrOriginal(rowPortPortLabel, portRow),
                Switch = cloneOrOriginal(rowPortSwitch, portRow),
                PortId = cloneOrOriginal(rowPortPortId, portRow),
                CurrentVlan = cloneOrOriginal(rowPortCurrentVlan, portRow),
                SetVlanTo = cloneOrOriginal(rowPortSetVlanTo, portRow),
                Set = cloneOrOriginal(rowPortSet, portRow),
                State = cloneOrOriginal(rowPortState, portRow)
            };
        }

        private SwitchRowControls getSwitchRowControls(int switchRow)
        {
            return new SwitchRowControls()
            {
                SwitchName = cloneOrOriginal(rowSwitchSwitchName, switchRow),
                PendingChanges = cloneOrOriginal(rowSwitchPendingChanges, switchRow),
                PersistChanges = cloneOrOriginal(rowSwitchPersistChanges, switchRow),
                State = cloneOrOriginal(rowSwitchState, switchRow),
            };
        }

        private const string CURRENT_VLAN_UNKNOWN = "unknown";
        private Color COLOR_NO_PENDING_CHANGES = SystemColors.ControlDark;
        private Color COLOR_HAS_PENDING_CHANGES = Color.DarkRed;

        private Dictionary<PortStatus, string> portStatusStrings = new Dictionary<PortStatus, string>()
        {
            { PortStatus.Unknown, "unknown" },
            { PortStatus.VlanRead, "VLAN setting loaded" },
            { PortStatus.SettingVlan, "changing VLAN setting..." },
            { PortStatus.VlanSetNotPersisted, "VLAN set (not permanent)" },
            { PortStatus.VlanSetFailed, "changing VLAN failed!" },
            { PortStatus.VlanSetPersisted, "VLAN set (permanent)" }
        };

        private Dictionary<PortStatus, Color> portStatusColors = new Dictionary<PortStatus, Color>()
        {
            { PortStatus.Unknown, Color.Black },
            { PortStatus.VlanRead, Color.Black },
            { PortStatus.SettingVlan, Color.DarkGreen },
            { PortStatus.VlanSetNotPersisted, Color.DarkGreen },
            { PortStatus.VlanSetFailed, Color.Red },
            { PortStatus.VlanSetPersisted, Color.DarkGreen }
        };

        private Dictionary<SwitchStatus, string> switchStatusStrings = new Dictionary<SwitchStatus, string>()
        {
            { SwitchStatus.NotConnected, "not connected" },
            { SwitchStatus.Connecting, "connecting..." },
            { SwitchStatus.CantConnect, "couldn't connect!" },
            { SwitchStatus.Connected, "connected" },
            { SwitchStatus.NoAccessMode, "no access method defined!" },
            { SwitchStatus.VlansRead, "VLAN settings loaded" },
            { SwitchStatus.PortVlanChanged, "VLAN setting of a port changed" },
            { SwitchStatus.PortVlanChangeError, "VLAN setting of a port failed!" },
            { SwitchStatus.ConfigSaved, "configuration saved as startup" },
            { SwitchStatus.ConfigSaveError, "saving configuration as startup failed!" }
        };

        private Dictionary<SwitchStatus, Color> switchStatusColors = new Dictionary<SwitchStatus, Color>()
        {
            { SwitchStatus.NotConnected, Color.Black },
            { SwitchStatus.Connecting, Color.Black },
            { SwitchStatus.CantConnect, Color.Red },
            { SwitchStatus.Connected, Color.DarkGreen },
            { SwitchStatus.NoAccessMode, Color.Red },
            { SwitchStatus.VlansRead, Color.DarkGreen },
            { SwitchStatus.PortVlanChanged, Color.DarkGreen },
            { SwitchStatus.PortVlanChangeError, Color.Red },
            { SwitchStatus.ConfigSaved, Color.DarkGreen },
            { SwitchStatus.ConfigSaveError, Color.Red }
        };

    }
}
