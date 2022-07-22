using easyvlans.GUI;
using easyvlans.GUI.Helpers;
using easyvlans.GUI.Helpers.DropDowns;
using easyvlans.Helpers;
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
        private class PortRowControls : RowControls<PortRowControls, Port>
        {

            private Label _portLabel;
            private Label _switchLabel;
            private Label _portIndexLabel;
            private Label _currentVlanLabel;
            private ComboBox _setVlanToComboBox;
            private Button _setButton;
            private Label _setVlanStatusLabel;

            private bool _setVlanToComboBox_changingAdapter;

            protected override void createControls(int itemIndex)
            {
                _portLabel = cloneOrOriginal(mainForm.rowPortPortLabel, itemIndex);
                _switchLabel = cloneOrOriginal(mainForm.rowPortSwitch, itemIndex);
                _portIndexLabel = cloneOrOriginal(mainForm.rowPortPortIndex, itemIndex);
                _currentVlanLabel = cloneOrOriginal(mainForm.rowPortCurrentVlan, itemIndex);
                _setVlanToComboBox = cloneOrOriginal(mainForm.rowPortSetVlanTo, itemIndex);
                _setButton = cloneOrOriginal(mainForm.rowPortSet, itemIndex);
                _setVlanStatusLabel = cloneOrOriginal(mainForm.rowPostStatusSetVlan, itemIndex);
                if (itemIndex > 0)
                {
                    int tableRowIndex = itemIndex + HEADER_ROWS;
                    table.RowCount++;
                    table.RowStyles.Add(_rowStyle);
                    table.Controls.Add(_portLabel, 0, tableRowIndex);
                    table.Controls.Add(_switchLabel, 1, tableRowIndex);
                    table.Controls.Add(_portIndexLabel, 2, tableRowIndex);
                    table.Controls.Add(_currentVlanLabel, 3, tableRowIndex);
                    table.Controls.Add(_setVlanToComboBox, 4, tableRowIndex);
                    table.Controls.Add(_setButton, 5, tableRowIndex);
                    table.Controls.Add(_setVlanStatusLabel, 6, tableRowIndex);
                }
                _setVlanToComboBox.SelectedIndexChanged += setVlanToComboBoxSelectedIndexChangedHandler;
                _setButton.Click += setButtonClickHandler;
            }

            protected override void debindItem()
            {
                _item.SetVlanMembershipStatusChanged -= setVlanMembershipStatusChangedHandler;
                _item.SetVlanMembershipStatusUpdateTimeChanged -= setVlanMembershipStatusUpdateTimeChangedHandler;
                _item.PendingChangesChanged -= pendingChangesChangedHandler;
                _item.HasComplexMembershipChanged -= hasComplexMembershipChangedHandler;
                _item.CurrentVlanChanged -= currentVlanChangedHandler;
            }

            protected override void bindItem()
            {
                _item.SetVlanMembershipStatusChanged += setVlanMembershipStatusChangedHandler;
                _item.SetVlanMembershipStatusUpdateTimeChanged += setVlanMembershipStatusUpdateTimeChangedHandler;
                _item.PendingChangesChanged += pendingChangesChangedHandler;
                _item.HasComplexMembershipChanged += hasComplexMembershipChangedHandler;
                _item.CurrentVlanChanged += currentVlanChangedHandler;
                //
                _portLabel.Text = _item.Label;
                _switchLabel.Text = _item.Switch.Label;
                _portIndexLabel.Text = _item.Index.ToString();
                displayVlanMembership();
                _setButton.Enabled = false;
                displaySetVlanMembershipStatus();
                _setVlanToComboBox_changingAdapter = true;
                _setVlanToComboBox.SetAdapterAsDataSource(getSetVlanToComboBoxAdapterForPort(_item));
                _setVlanToComboBox_changingAdapter = false;
                if (setVlanToComboBoxSelections.TryGetValue(_item, out Vlan selectedVlan))
                    _setVlanToComboBox.SelectByValue(selectedVlan);
            }

            private void displayVlanMembership()
            {
                string vlanText = CURRENT_VLAN_UNKNOWN;
                Color foreColor = Color.LightGray;
                if (_item.CurrentVlan != null)
                {
                    vlanText = _item.CurrentVlan.Label;
                    foreColor = _item.PendingChanges ? COLOR_HAS_PENDING_CHANGES : COLOR_NO_PENDING_CHANGES;
                }
                else if (_item.HasComplexMembership)
                {
                    vlanText = CURRENT_VLAN_COMPLEX;
                    foreColor = Color.LightBlue;
                }
                _currentVlanLabel.Text = vlanText;
                _currentVlanLabel.ForeColor = foreColor;
            }

            private void displaySetVlanMembershipStatus() => displayStatus(_setVlanStatusLabel, _item.SetVlanMembershipStatus, _item.SetVlanMembershipStatusUpdateTime);

            private void setVlanMembershipStatusChangedHandler(Port port, Status newValue) => table.InvokeIfRequired(displaySetVlanMembershipStatus);
            private void setVlanMembershipStatusUpdateTimeChangedHandler(Port port, DateTime newValue) => table.InvokeIfRequired(displaySetVlanMembershipStatus);
            private void pendingChangesChangedHandler(Port port, bool newValue) => table.InvokeIfRequired(displayVlanMembership);
            private void hasComplexMembershipChangedHandler(Port port, bool newValue) => table.InvokeIfRequired(displayVlanMembership);

            private void currentVlanChangedHandler(Port port, Vlan newValue)
                => table.InvokeIfRequired(() => {
                    if (port == _item)
                        _setVlanToComboBox.SelectedIndex = 0;
                    displayVlanMembership();
                });

            private void setVlanToComboBoxSelectedIndexChangedHandler(object sender, EventArgs e)
            {
                _setButton.Enabled = (_setVlanToComboBox.SelectedIndex > 0);
                if (!_setVlanToComboBox_changingAdapter)
                    setVlanToComboBoxSelections[_item] = _setVlanToComboBox.SelectedValue as Vlan;
            }

            private async void setButtonClickHandler(object sender, EventArgs e) => await _item?.SetVlanTo(_setVlanToComboBox.SelectedValue as Vlan);

            private static readonly Dictionary<Port, IComboBoxAdapter> setVlanToComboBoxAdaptersByPort = new();

            private static IComboBoxAdapter getSetVlanToComboBoxAdapterForPort(Port port)
                => setVlanToComboBoxAdaptersByPort.GetAnyway(port, p => new ComboBoxAdapter<Vlan>(port.Vlans, v => v.Label, true, string.Empty));

            private static readonly Dictionary<Port, Vlan> setVlanToComboBoxSelections = new();

            private const string CURRENT_VLAN_COMPLEX = "complex";
            private const string CURRENT_VLAN_UNKNOWN = "unknown";
            private static readonly Color COLOR_NO_PENDING_CHANGES = SystemColors.ControlText;
            private static readonly Color COLOR_HAS_PENDING_CHANGES = Color.DarkRed;

        }

    }

}
