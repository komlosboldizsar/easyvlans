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

    public partial class MainForm
    {

        private class SwitchRowControls : RowControls<SwitchRowControls, Switch>
        {
            public Label _switchNameLabel;
            public Label _pendingChangesLabel;
            public Button _persistChangesButton;
            public Label _stateLabel;

            protected override void createControls(int itemIndex)
            {
                _switchNameLabel = cloneOrOriginal(mainForm.rowSwitchSwitchName, itemIndex);
                _pendingChangesLabel = cloneOrOriginal(mainForm.rowSwitchPendingChanges, itemIndex);
                _persistChangesButton = cloneOrOriginal(mainForm.rowSwitchPersistChanges, itemIndex);
                _stateLabel = cloneOrOriginal(mainForm.rowSwitchState, itemIndex);
                //
                if (itemIndex > 0)
                {
                    int tableRowIndex = itemIndex + HEADER_ROWS;
                    table.RowCount++;
                    table.RowStyles.Add(_rowStyle);
                    table.Controls.Add(_switchNameLabel, 0, tableRowIndex);
                    table.Controls.Add(_pendingChangesLabel, 1, tableRowIndex);
                    table.Controls.Add(_persistChangesButton, 2, tableRowIndex);
                    table.Controls.Add(_stateLabel, 3, tableRowIndex);
                }
                //
                _persistChangesButton.Click += persistChangesButtonClickHandler;
            }

            protected override void debindItem()
            {
                _item.StatusChanged -= statusChangedHandler;
                _item.PortsWithPendingChangeCountChanged -= portsWithPendingChangeCountChangedHandler;
            }

            protected override void bindItem()
            {
                _item.StatusChanged += statusChangedHandler;
                _item.PortsWithPendingChangeCountChanged += portsWithPendingChangeCountChangedHandler;
                _switchNameLabel.Text = _item.Label;
                displayPortsWithPendingChangeCount();
                displayStatus();
            }

            private void displayStatus()
            {
                _stateLabel.Text = switchStatusStrings[_item.Status];
                _stateLabel.ForeColor = switchStatusColors[_item.Status];
            }

            private void displayPortsWithPendingChangeCount()
            {
                int portsWithPendingChangeCount = _item.PortsWithPendingChangeCount;
                string newText = "no ports changed";
                if (portsWithPendingChangeCount == 0)
                {
                    _pendingChangesLabel.Text = newText;
                    _pendingChangesLabel.ForeColor = COLOR_NO_PENDING_CHANGES;
                    _persistChangesButton.Enabled = false;
                }
                else
                {
                    newText = (portsWithPendingChangeCount > 1) ? $"{portsWithPendingChangeCount} ports changed" : "1 port changed";
                    _pendingChangesLabel.Text = newText;
                    _pendingChangesLabel.ForeColor = COLOR_HAS_PENDING_CHANGES;
                    _persistChangesButton.Enabled = true;
                }
            }

            private void statusChangedHandler(Switch @switch, SwitchStatus newValue) => displayStatus();
            private void portsWithPendingChangeCountChangedHandler(Switch @switch, int newValue) => displayPortsWithPendingChangeCount();

            private async void persistChangesButtonClickHandler(object sender, EventArgs e)
                => await _item?.PersistChangesAsync();

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

            private Color COLOR_NO_PENDING_CHANGES = SystemColors.ControlDark;
            private Color COLOR_HAS_PENDING_CHANGES = Color.DarkRed;

        }

    }

}
