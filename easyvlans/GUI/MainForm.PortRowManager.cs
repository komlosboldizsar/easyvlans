using easyvlans.GUI.Helpers;
using easyvlans.GUI.Helpers.DropDowns;
using easyvlans.Helpers;
using easyvlans.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace easyvlans.GUI
{
    public partial class MainForm
    {
        private class PortRowManager : RowManagerBase<Port>
        {

            private Label _portLabel;
            private Label _switchLabel;
            private Label _portIndexLabel;
            private Label _currentVlanLabel;
            private ComboBox _setVlanToComboBox;
            private Button _setButton;
            private Label _setVlanStatusLabel;

            private bool _setVlanToComboBox_changingAdapter;

            protected override IMemberBinding[] MemberBindings => new IMemberBinding[]
            {
                new MemberBinding<Label>(0, c => _portLabel = c),
                new MemberBinding<Label>(1, c => _switchLabel = c),
                new MemberBinding<Label>(2, c => _portIndexLabel = c),
                new MemberBinding<Label>(3, c => _currentVlanLabel = c),
                new MemberBinding<ComboBox>(4, c => _setVlanToComboBox = c),
                new MemberBinding<Button>(5, c => _setButton = c),
                new MemberBinding<Label>(6, c =>_setVlanStatusLabel = c),
            };

            public override void SubscribeControlEvents()
            {
                _setVlanToComboBox.SelectedIndexChanged += setVlanToComboBoxSelectedIndexChangedHandler;
                _setButton.Click += setButtonClickHandler;
            }

            protected override void DebindItem()
            {
                Item.SetVlanMembershipStatusChanged -= setVlanMembershipStatusChangedHandler;
                Item.SetVlanMembershipStatusUpdateTimeChanged -= setVlanMembershipStatusUpdateTimeChangedHandler;
                Item.PendingChangesChanged -= pendingChangesChangedHandler;
                Item.HasComplexMembershipChanged -= hasComplexMembershipChangedHandler;
                Item.CurrentVlanChanged -= currentVlanChangedHandler;
            }

            protected override void BindItem()
            {
                Item.SetVlanMembershipStatusChanged += setVlanMembershipStatusChangedHandler;
                Item.SetVlanMembershipStatusUpdateTimeChanged += setVlanMembershipStatusUpdateTimeChangedHandler;
                Item.PendingChangesChanged += pendingChangesChangedHandler;
                Item.HasComplexMembershipChanged += hasComplexMembershipChangedHandler;
                Item.CurrentVlanChanged += currentVlanChangedHandler;
                //
                _portLabel.Text = Item.Label;
                _switchLabel.Text = Item.Switch?.Label ?? string.Empty;
                _portIndexLabel.Text = Item.Index.ToString();
                displayVlanMembership();
                _setButton.Enabled = false;
                displaySetVlanMembershipStatus();
                _setVlanToComboBox_changingAdapter = true;
                _setVlanToComboBox.SetAdapterAsDataSource(getSetVlanToComboBoxAdapterForPort(Item));
                _setVlanToComboBox_changingAdapter = false;
                if (setVlanToComboBoxSelections.TryGetValue(Item, out Vlan selectedVlan))
                    _setVlanToComboBox.SelectByValue(selectedVlan);
            }

            private void displayVlanMembership()
            {
                string vlanText = CURRENT_VLAN_UNKNOWN;
                Color foreColor = Color.LightGray;
                if (Item.CurrentVlan != null)
                {
                    vlanText = Item.CurrentVlan.Label;
                    if (Item.PendingChanges)
                        foreColor = COLOR_HAS_PENDING_CHANGES;
                    else
                        foreColor = Item.HasNotAllowedMembership ? COLOR_HAS_NOT_ALLOWED_MEMBERSHIP : COLOR_NO_PENDING_CHANGES;
                }
                else if (Item.HasComplexMembership)
                {
                    vlanText = CURRENT_VLAN_COMPLEX;
                    foreColor = COLOR_HAS_COMPLEX_MEMBERSHIP;
                }
                _currentVlanLabel.Text = vlanText;
                _currentVlanLabel.ForeColor = foreColor;
            }

            private void displaySetVlanMembershipStatus() => displayStatus(_setVlanStatusLabel, Item.SetVlanMembershipStatus, Item.SetVlanMembershipStatusUpdateTime);

            private void setVlanMembershipStatusChangedHandler(Port port, Status newValue) => Table.InvokeIfRequired(displaySetVlanMembershipStatus);
            private void setVlanMembershipStatusUpdateTimeChangedHandler(Port port, DateTime newValue) => Table.InvokeIfRequired(displaySetVlanMembershipStatus);
            private void pendingChangesChangedHandler(Port port, bool newValue) => Table.InvokeIfRequired(displayVlanMembership);
            private void hasComplexMembershipChangedHandler(Port port, bool newValue) => Table.InvokeIfRequired(displayVlanMembership);

            private void currentVlanChangedHandler(Port port, Vlan newValue)
                => Table.InvokeIfRequired(() =>
                {
                    if (port == Item)
                        _setVlanToComboBox.SelectedIndex = 0;
                    displayVlanMembership();
                });

            private void setVlanToComboBoxSelectedIndexChangedHandler(object sender, EventArgs e)
            {
                _setButton.Enabled = (_setVlanToComboBox.SelectedIndex > 0);
                if (!_setVlanToComboBox_changingAdapter)
                    setVlanToComboBoxSelections[Item] = _setVlanToComboBox.SelectedValue as Vlan;
            }

            private async void setButtonClickHandler(object sender, EventArgs e) => await Item?.SetVlanTo(_setVlanToComboBox.SelectedValue as Vlan);

            private static readonly Dictionary<Port, IComboBoxAdapter> setVlanToComboBoxAdaptersByPort = new();

            private static IComboBoxAdapter getSetVlanToComboBoxAdapterForPort(Port port)
                => setVlanToComboBoxAdaptersByPort.GetAnyway(port, p => new ComboBoxAdapter<Vlan>(port.Vlans, v => v.Label, true, string.Empty));

            private static readonly Dictionary<Port, Vlan> setVlanToComboBoxSelections = new();

            private const string CURRENT_VLAN_COMPLEX = "complex";
            private const string CURRENT_VLAN_UNKNOWN = "unknown";
            private static readonly Color COLOR_NO_PENDING_CHANGES = SystemColors.ControlText;
            private static readonly Color COLOR_HAS_PENDING_CHANGES = Color.DarkRed;
            private static readonly Color COLOR_HAS_COMPLEX_MEMBERSHIP = Color.Blue;
            private static readonly Color COLOR_HAS_NOT_ALLOWED_MEMBERSHIP = Color.Red;

        }

    }

}
