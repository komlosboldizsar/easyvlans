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
                _setVlanToComboBox.DrawMode = DrawMode.OwnerDrawFixed;
                _setVlanToComboBox.DrawItem += _setVlanToComboBox_DrawItem;
            }

            private void _setVlanToComboBox_DrawItem(object sender, DrawItemEventArgs e)
            {
                if (e.Index == -1)
                    return;
                ComboBoxAdapter<Vlan>.ItemProxy comboBoxItem = _setVlanToComboBox.Items[e.Index] as ComboBoxAdapter<Vlan>.ItemProxy;
                e.DrawBackground();
                int defaultMarkVerticalSpace = (e.Bounds.Height - DEFAULT_MARK_SIZE) / 2;
                Rectangle textBounds = e.Bounds with
                {
                    Width = e.Bounds.Width - DEFAULT_MARK_SIZE - 2 * defaultMarkVerticalSpace
                };
                using Pen textPen = new(e.ForeColor);
                using Brush textBrush = new SolidBrush(e.ForeColor);
                StringFormat stringFormat = new(StringFormat.GenericDefault)
                {
                    Alignment = StringAlignment.Far
                };
                Rectangle vlanIdFieldBounds = textBounds with
                {
                    Width = VLAN_ID_FIELD_WIDTH
                };
                Rectangle vlanNameFieldBounds = textBounds with
                {
                    X = textBounds.X + VLAN_ID_FIELD_WIDTH + FIELD_SEPARATOR_WIDTH,
                    Width = textBounds.Width - VLAN_ID_FIELD_WIDTH - FIELD_SEPARATOR_WIDTH
                };
                e.Graphics.DrawString(comboBoxItem.Value?.ID.ToString(), e.Font, textBrush, vlanIdFieldBounds, stringFormat);
                int px = textBounds.X + VLAN_ID_FIELD_WIDTH + FIELD_SEPARATOR_BAR_WIDTH;
                Point p1 = new(px, textBounds.Top + 3);
                Point p2 = new(px, textBounds.Bottom - 3);
                e.Graphics.DrawLine(textPen, p1, p2);
                e.Graphics.DrawString(comboBoxItem.Value?.Name.ToString(), e.Font, textBrush, vlanNameFieldBounds, StringFormat.GenericDefault);
                if ((Item.DefaultVlan != null) && (comboBoxItem?.Value == Item.DefaultVlan))
                {
                    Rectangle defaultMarkRectangle = new(
                        e.Bounds.X + e.Bounds.Width - defaultMarkVerticalSpace - DEFAULT_MARK_SIZE,
                        (e.Bounds.Top + e.Bounds.Bottom - DEFAULT_MARK_SIZE) / 2,
                        DEFAULT_MARK_SIZE,
                        DEFAULT_MARK_SIZE
                    );
                    e.Graphics.FillRectangle(Brushes.Purple, defaultMarkRectangle);
                }
                e.DrawFocusRectangle();
            }

            private const int VLAN_ID_FIELD_WIDTH = 30;
            private const int FIELD_SEPARATOR_SPACE_HALF_WIDTH = 2;
            private const int FIELD_SEPARATOR_BAR_WIDTH = 1;
            private const int FIELD_SEPARATOR_WIDTH = FIELD_SEPARATOR_SPACE_HALF_WIDTH * 2 + FIELD_SEPARATOR_BAR_WIDTH;
            private const int DEFAULT_MARK_SIZE = 10;

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
                _setVlanToComboBox.Tag = Item;
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
                    else if ((Item.DefaultVlan != null) && (Item.DefaultVlan != Item.CurrentVlan))
                        foreColor = COLOR_NOT_ON_DEFAULT_VLAN;
                    else if (Item.HasNotAllowedMembership)
                        foreColor = COLOR_HAS_NOT_ALLOWED_MEMBERSHIP;
                    else
                        foreColor = COLOR_NO_PENDING_CHANGES;
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

            private IComboBoxAdapter getSetVlanToComboBoxAdapterForPort(Port port)
                => setVlanToComboBoxAdaptersByPort.GetAnyway(port, p => new ComboBoxAdapter<Vlan>(port.Vlans, v => v.Label, true, string.Empty));

            private static readonly Dictionary<Port, Vlan> setVlanToComboBoxSelections = new();

            private const string CURRENT_VLAN_COMPLEX = "complex";
            private const string CURRENT_VLAN_UNKNOWN = "unknown";
            private static readonly Color COLOR_NO_PENDING_CHANGES = SystemColors.ControlText;
            private static readonly Color COLOR_HAS_PENDING_CHANGES = Color.DarkRed;
            private static readonly Color COLOR_NOT_ON_DEFAULT_VLAN = Color.Purple;
            private static readonly Color COLOR_HAS_COMPLEX_MEMBERSHIP = Color.Blue;
            private static readonly Color COLOR_HAS_NOT_ALLOWED_MEMBERSHIP = Color.Red;

        }

    }

}
