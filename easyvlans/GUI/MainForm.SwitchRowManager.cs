using BToolbox.GUI.RecyclerTables;
using easyvlans.GUI.Helpers;
using easyvlans.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class MainForm
    {
        private class SwitchRowManager : RowManagerBase<Switch>
        {

            public Label _switchNameLabel;
            public Label _pendingChangesLabel;
            public Button _persistChangesButton;
            public Label _readVlanConfigStatusLabel;
            public Label _persistConfigStatusLabel;

            protected override IMemberBinding[] MemberBindings => new IMemberBinding[]
            {
                new MemberBinding<Label>(0, c => _switchNameLabel = c),
                new MemberBinding<Label>(1, c => _pendingChangesLabel = c),
                new MemberBinding<Button>(2, c => _persistChangesButton = c),
                new MemberBinding<Label>(3, c => _readVlanConfigStatusLabel = c),
                new MemberBinding<Label>(4, c => _persistConfigStatusLabel = c)
            };

            public override void SubscribeControlEvents()
            {
                _persistChangesButton.Click += persistChangesButtonClickHandler;
            }

            protected override void DebindItem()
            {
                Item.ReadVlanConfigStatusChanged -= readVlanConfigStatusChangedHandler;
                Item.ReadVlanConfigStatusUpdateTimeChanged -= readVlanConfigStatusUpdateTimeChangedHandler;
                Item.PersistConfigStatusChanged -= persistConfigStatusChangedHandler;
                Item.PersistVlanConfigStatusUpdateTimeChanged -= persistConfigStatusUpdateTimeChangedHandler;
                Item.PortsWithPendingChangeCountChanged -= portsWithPendingChangeCountChangedHandler;
            }

            protected override void BindItem()
            {
                Item.ReadVlanConfigStatusChanged += readVlanConfigStatusChangedHandler;
                Item.ReadVlanConfigStatusUpdateTimeChanged += readVlanConfigStatusUpdateTimeChangedHandler;
                Item.PersistConfigStatusChanged += persistConfigStatusChangedHandler;
                Item.PersistVlanConfigStatusUpdateTimeChanged += persistConfigStatusUpdateTimeChangedHandler;
                Item.PortsWithPendingChangeCountChanged += portsWithPendingChangeCountChangedHandler;
                //
                _switchNameLabel.Text = Item.Label;
                displayPortsWithPendingChangeCount();
                if (Item.OperationMethodCollection?.PersistChangesMethod == null)
                {
                    _persistChangesButton.Enabled = false;
                    _persistChangesButton.Text = "";
                }
                displayReadVlanConfigStatus();
                displayPersistConfigStatus();
            }

            private void displayReadVlanConfigStatus() => displayStatus(_readVlanConfigStatusLabel, Item.ReadVlanConfigStatus, Item.ReadVlanConfigStatusUpdateTime);
            private void displayPersistConfigStatus() => displayStatus(_persistConfigStatusLabel, Item.PersistVlanConfigStatus, Item.PersistVlanConfigStatusUpdateTime);

            private void displayPortsWithPendingChangeCount()
            {
                int portsWithPendingChangeCount = Item.PortsWithPendingChangeCount;
                string newText = "no ports changed";
                if (portsWithPendingChangeCount == 0)
                {
                    _pendingChangesLabel.Text = newText;
                    _pendingChangesLabel.ForeColor = COLOR_NO_PENDING_CHANGES;
                    _persistChangesButton.ForeColor = COLOR_PERSIST_BUTTON_INACTIVE;
                }
                else
                {
                    newText = (portsWithPendingChangeCount > 1) ? $"{portsWithPendingChangeCount} ports changed" : "1 port changed";
                    _pendingChangesLabel.Text = newText;
                    _pendingChangesLabel.ForeColor = COLOR_HAS_PENDING_CHANGES;
                    _persistChangesButton.ForeColor = COLOR_PERSIST_BUTTON_ACTIVE;
                }
            }

            private void readVlanConfigStatusChangedHandler(Switch @switch, Status newValue) => Table.InvokeIfRequired(displayReadVlanConfigStatus);
            private void readVlanConfigStatusUpdateTimeChangedHandler(Switch @switch, DateTime newValue) => Table.InvokeIfRequired(displayReadVlanConfigStatus);
            private void persistConfigStatusChangedHandler(Switch @switch, Status newValue) => Table.InvokeIfRequired(displayPersistConfigStatus);
            private void persistConfigStatusUpdateTimeChangedHandler(Switch @switch, DateTime newValue) => Table.InvokeIfRequired(displayPersistConfigStatus);
            private void portsWithPendingChangeCountChangedHandler(Switch @switch, int newValue) => Table.InvokeIfRequired(displayPortsWithPendingChangeCount);

            private async void persistChangesButtonClickHandler(object sender, EventArgs e) => await Item?.PersistChangesAsync();

            private static readonly Color COLOR_NO_PENDING_CHANGES = SystemColors.ControlDark;
            private static readonly Color COLOR_HAS_PENDING_CHANGES = Color.DarkRed;

            private static readonly Color COLOR_PERSIST_BUTTON_INACTIVE = SystemColors.ControlDark;
            private static readonly Color COLOR_PERSIST_BUTTON_ACTIVE = SystemColors.ControlText;

        }
    }
}
