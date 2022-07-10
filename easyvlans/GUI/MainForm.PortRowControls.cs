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
        private class PortRowControls : RowControls<PortRowControls, UserPort>
        {

            private Label _portLabel;
            private Label _switchLabel;
            private Label _portIndexLabel;
            private Label _currentVlanLabel;
            private ComboBox _setVlanToComboBox;
            private Button _setButton;
            private Label _stateLabel;

            private bool _setVlanToComboBox_changingAdapter;

            protected override void createControls(int itemIndex)
            {
                _portLabel = cloneOrOriginal(mainForm.rowPortPortLabel, itemIndex);
                _switchLabel = cloneOrOriginal(mainForm.rowPortSwitch, itemIndex);
                _portIndexLabel = cloneOrOriginal(mainForm.rowPortPortIndex, itemIndex);
                _currentVlanLabel = cloneOrOriginal(mainForm.rowPortCurrentVlan, itemIndex);
                _setVlanToComboBox = cloneOrOriginal(mainForm.rowPortSetVlanTo, itemIndex);
                _setButton = cloneOrOriginal(mainForm.rowPortSet, itemIndex);
                _stateLabel = cloneOrOriginal(mainForm.rowPortState, itemIndex);
                //
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
                    table.Controls.Add(_stateLabel, 6, tableRowIndex);
                }
                //
                _setVlanToComboBox.SelectedIndexChanged += setVlanToComboBoxSelectedIndexChangedHandler;
                _setButton.Click += setButtonClickHandler;
            }

            protected override void debindItem()
            {
                _item.StatusChanged -= portsStatusChangedHandler;
                _item.PendingChangesChanged -= pendingChangesChangedHandler;
                _item.HasComplexMembershipChanged -= hasComplexMembershipChangedHandler;
                _item.CurrentVlanChanged -= currentVlanChangedHandler;
            }

            protected override void bindItem()
            {
                _item.StatusChanged += portsStatusChangedHandler;
                _item.PendingChangesChanged += pendingChangesChangedHandler;
                _item.HasComplexMembershipChanged += hasComplexMembershipChangedHandler;
                _item.CurrentVlanChanged += currentVlanChangedHandler;
                //
                _portLabel.Text = _item.Label;
                _switchLabel.Text = _item.Switch.Label;
                _portIndexLabel.Text = _item.Index.ToString();
                displayVlanMembership();
                _setButton.Enabled = false;
                displayStatus();
                _setVlanToComboBox_changingAdapter = true;
                _setVlanToComboBox.SetAdapterAsDataSource(getSetVlanToComboBoxAdapterForPort(_item));
                _setVlanToComboBox_changingAdapter = false;
                if (setVlanToComboBoxSelections.TryGetValue(_item, out UserVlan selectedVlan))
                    _setVlanToComboBox.SelectByValue(selectedVlan);
            }

            private void displayVlanMembership()
            {
                string vlanText;
                Color foreColor;
                if (_item.CurrentVlan != null)
                {
                    vlanText = vlanToStr(_item.CurrentVlan);
                    foreColor = _item.PendingChanges ? COLOR_HAS_PENDING_CHANGES : COLOR_NO_PENDING_CHANGES;
                }
                else if (_item.HasComplexMembership)
                {
                    vlanText = CURRENT_VLAN_COMPLEX;
                    foreColor = Color.LightBlue;
                }
                else
                {
                    vlanText = CURRENT_VLAN_UNKNOWN;
                    foreColor = Color.LightGray;
                }
                _currentVlanLabel.Text = vlanText;
                _currentVlanLabel.ForeColor = foreColor;
            }

            private void displayStatus()
            {
                _stateLabel.Text = portStatusStrings[_item.Status];
                _stateLabel.ForeColor = portStatusColors[_item.Status];
            }

            private void portsStatusChangedHandler(UserPort port, PortStatus newValue) => displayStatus();
            private void pendingChangesChangedHandler(UserPort port, bool newValue) => displayVlanMembership();
            private void hasComplexMembershipChangedHandler(UserPort port, bool newValue) => displayVlanMembership();

            private void currentVlanChangedHandler(UserPort port, UserVlan newValue)
            {
                if (port == _item)
                    _setVlanToComboBox.SelectedIndex = 0;
                displayVlanMembership();
            }

            private void setVlanToComboBoxSelectedIndexChangedHandler(object sender, EventArgs e)
            {
                _setButton.Enabled = (_setVlanToComboBox.SelectedIndex > 0);
                if (!_setVlanToComboBox_changingAdapter)
                    setVlanToComboBoxSelections[_item] = _setVlanToComboBox.SelectedValue as UserVlan;
            }

            private async void setButtonClickHandler(object sender, EventArgs e)
                => await _item?.SetVlanTo(_setVlanToComboBox.SelectedValue as UserVlan);

            private static readonly Dictionary<UserPort, IComboBoxAdapter> setVlanToComboBoxAdaptersByPort = new();

            private static IComboBoxAdapter getSetVlanToComboBoxAdapterForPort(UserPort port)
            {
                if (!setVlanToComboBoxAdaptersByPort.TryGetValue(port, out IComboBoxAdapter adapter))
                {
                    adapter = new ComboBoxAdapter<UserVlan>(port.Vlans, vlanToStr, true, string.Empty);
                    setVlanToComboBoxAdaptersByPort.Add(port, adapter);
                }
                return adapter;
            }

            private static string vlanToStr(UserVlan vlan) => $"{vlan.ID} - {vlan.Name}";

            private static readonly Dictionary<UserPort, UserVlan> setVlanToComboBoxSelections = new();

            private static readonly Dictionary<PortStatus, string> portStatusStrings = new()
            {
                { PortStatus.Unknown, "unknown" },
                { PortStatus.VlanRead, "VLAN setting loaded" },
                { PortStatus.SettingVlan, "changing VLAN setting..." },
                { PortStatus.VlanSetNotPersisted, "VLAN set (not permanent)" },
                { PortStatus.VlanSetFailed, "changing VLAN failed!" },
                { PortStatus.VlanSetPersisted, "VLAN set (permanent)" }
            };

            private static readonly Dictionary<PortStatus, Color> portStatusColors = new()
            {
                { PortStatus.Unknown, Color.Black },
                { PortStatus.VlanRead, Color.Black },
                { PortStatus.SettingVlan, Color.DarkGreen },
                { PortStatus.VlanSetNotPersisted, Color.DarkGreen },
                { PortStatus.VlanSetFailed, Color.Red },
                { PortStatus.VlanSetPersisted, Color.DarkGreen }
            };

            private const string CURRENT_VLAN_COMPLEX = "complex";
            private const string CURRENT_VLAN_UNKNOWN = "unknown";
            private Color COLOR_NO_PENDING_CHANGES = SystemColors.ControlDark;
            private Color COLOR_HAS_PENDING_CHANGES = Color.DarkRed;

        }

    }

}
