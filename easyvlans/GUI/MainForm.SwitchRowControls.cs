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
            public Label _readVlanConfigStatusLabel;
            public Label _persistConfigStatusLabel;

            protected override void createControls(int itemIndex)
            {
                _switchNameLabel = cloneOrOriginal(mainForm.rowSwitchSwitchName, itemIndex);
                _pendingChangesLabel = cloneOrOriginal(mainForm.rowSwitchPendingChanges, itemIndex);
                _persistChangesButton = cloneOrOriginal(mainForm.rowSwitchPersistChanges, itemIndex);
                _readVlanConfigStatusLabel = cloneOrOriginal(mainForm.rowSwitchStatusRead, itemIndex);
                _persistConfigStatusLabel = cloneOrOriginal(mainForm.rowSwitchStatusPersist, itemIndex);
                if (itemIndex > 0)
                {
                    int tableRowIndex = itemIndex + HEADER_ROWS;
                    table.RowCount++;
                    table.RowStyles.Add(_rowStyle);
                    table.Controls.Add(_switchNameLabel, 0, tableRowIndex);
                    table.Controls.Add(_pendingChangesLabel, 1, tableRowIndex);
                    table.Controls.Add(_persistChangesButton, 2, tableRowIndex);
                    table.Controls.Add(_readVlanConfigStatusLabel, 3, tableRowIndex);
                    table.Controls.Add(_persistConfigStatusLabel, 4, tableRowIndex);
                }
                _persistChangesButton.Click += persistChangesButtonClickHandler;
            }

            protected override void debindItem()
            {
                _item.ReadVlanConfigStatusChanged -= readVlanConfigStatusChangedHandler;
                _item.ReadVlanConfigStatusUpdateTimeChanged -= readVlanConfigStatusUpdateTimeChangedHandler;
                _item.PersistConfigStatusChanged -= persistConfigStatusChangedHandler;
                _item.PersistVlanConfigStatusUpdateTimeChanged -= persistConfigStatusUpdateTimeChangedHandler;
                _item.PortsWithPendingChangeCountChanged -= portsWithPendingChangeCountChangedHandler;
            }

            protected override void bindItem()
            {
                _item.ReadVlanConfigStatusChanged += readVlanConfigStatusChangedHandler;
                _item.ReadVlanConfigStatusUpdateTimeChanged += readVlanConfigStatusUpdateTimeChangedHandler;
                _item.PersistConfigStatusChanged += persistConfigStatusChangedHandler;
                _item.PersistVlanConfigStatusUpdateTimeChanged += persistConfigStatusUpdateTimeChangedHandler;
                _item.PortsWithPendingChangeCountChanged += portsWithPendingChangeCountChangedHandler;
                //
                _switchNameLabel.Text = _item.Label;
                displayPortsWithPendingChangeCount();
                displayReadVlanConfigStatus();
                displayPersistConfigStatus();
            }

            private void displayReadVlanConfigStatus() => displayStatus(_readVlanConfigStatusLabel, _item.ReadVlanConfigStatus, _item.ReadVlanConfigStatusUpdateTime);
            private void displayPersistConfigStatus() => displayStatus(_persistConfigStatusLabel, _item.PersistVlanConfigStatus, _item.PersistVlanConfigStatusUpdateTime);

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

            private void readVlanConfigStatusChangedHandler(Switch @switch, Status newValue) => table.InvokeIfRequired(displayReadVlanConfigStatus);
            private void readVlanConfigStatusUpdateTimeChangedHandler(Switch @switch, DateTime newValue) => table.InvokeIfRequired(displayReadVlanConfigStatus);
            private void persistConfigStatusChangedHandler(Switch @switch, Status newValue) => table.InvokeIfRequired(displayPersistConfigStatus);
            private void persistConfigStatusUpdateTimeChangedHandler(Switch @switch, DateTime newValue) => table.InvokeIfRequired(displayPersistConfigStatus);
            private void portsWithPendingChangeCountChangedHandler(Switch @switch, int newValue) => table.InvokeIfRequired(displayPortsWithPendingChangeCount);

            private async void persistChangesButtonClickHandler(object sender, EventArgs e) => await _item?.PersistChangesAsync();

            private static readonly Color COLOR_NO_PENDING_CHANGES = SystemColors.ControlDark;
            private static readonly Color COLOR_HAS_PENDING_CHANGES = Color.DarkRed;

        }
    }
}
