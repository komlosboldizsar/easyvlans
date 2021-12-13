﻿using easyvlans.GUI;
using easyvlans.GUI.Helpers;
using easyvlans.GUI.Helpers.DropDowns;
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

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(Config config)
        {
            this.config = config;
            Load += loadConfig;
            InitializeComponent();
        }

        private void loadConfig(object sender, EventArgs e)
        {

            if (config == null)
            {
                // Todo...
                return;
            }

            if (config.Ports.Count == 0)
            {
                // Todo...
                return;
            }

            int portRow = 0;
            int rowHeight = (int)table.RowStyles[1].Height;
            foreach (Port port in config.Ports)
            {

                int tableRow = portRow + 1;
                RowControls thisRowControls = getRowControls(portRow);
                rowControls.Add(thisRowControls);
                portsRowControls.Add(port, thisRowControls);

                if (portRow > 0)
                {
                    table.RowCount++;
                    table.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
                    table.Controls.Add(thisRowControls.PortLabel, 0, tableRow);
                    table.Controls.Add(thisRowControls.Switch, 1, tableRow);
                    table.Controls.Add(thisRowControls.PortId, 2, tableRow);
                    table.Controls.Add(thisRowControls.CurrentVlan, 3, tableRow);
                    table.Controls.Add(thisRowControls.SetVlanTo, 4, tableRow);
                    table.Controls.Add(thisRowControls.Set, 5, tableRow);
                    table.Controls.Add(thisRowControls.State, 6, tableRow);
                    Size = new Size(Size.Width, Size.Height + rowHeight);
                }

                thisRowControls.PortLabel.Text = port.Label;
                thisRowControls.Switch.Text = port.Switch.Label;
                thisRowControls.PortId.Text = port.Index;
                thisRowControls.CurrentVlan.Text = CURRENT_VLAN_UNKNOWN;
                thisRowControls.SetVlanTo.Tag = port;
                thisRowControls.Set.Tag = port;
                thisRowControls.Set.Enabled = false;
                thisRowControls.State.Text = "";

                portRow++;

            }

            portRow = 0;
            foreach (Port port in config.Ports)
                rowControls[portRow++].SetVlanTo.CreateAdapterAsDataSource(port.Vlans, vlanToStr, true, "");

            foreach (Port port in config.Ports)
            {
                port.CurrentVlanChanged += portsCurrentVlanChangedHandler;
                RowControls rowControls = portsRowControls[port];
                rowControls.SetVlanTo.SelectedIndexChanged += setVlanToSelectedIndexChangedHandler;
                rowControls.Set.Click += setButtonClickHandler;
            }

        }

        private void portsCurrentVlanChangedHandler(Port port, Vlan newValue)
            => portsRowControls[port].CurrentVlan.Text = vlanToStr(newValue);

        private void setVlanToSelectedIndexChangedHandler(object sender, EventArgs e)
        {
            ComboBox typedSender = sender as ComboBox;
            Port port = typedSender?.Tag as Port;
            if (port == null)
                return;
            portsRowControls[port].Set.Enabled = (typedSender.SelectedIndex > 0);
        }

        private void setButtonClickHandler(object sender, EventArgs e)
        {
            Button typedSender = sender as Button;
            Port port = typedSender?.Tag as Port;
            if (port == null)
                return;
            Vlan selectedVlan = portsRowControls[port].SetVlanTo.SelectedValue as Vlan;
            port.SetVlanTo(selectedVlan);
        }

        private string vlanToStr(Vlan vlan) => $"{vlan.ID} - {vlan.Name}";

        public class RowControls
        {
            public Label PortLabel { get; init; }
            public Label Switch { get; init; }
            public Label PortId { get; init; }
            public Label CurrentVlan { get; init; }
            public ComboBox SetVlanTo { get; init; }
            public Button Set { get; init; }
            public Label State { get; init; }
        }

        private List<RowControls> rowControls = new List<RowControls>();
        private Dictionary<Port, RowControls> portsRowControls = new Dictionary<Port, RowControls>();

        private T cloneOrOriginal<T>(T originalControl, int portRow)
            where T : Control
            => (portRow == 0) ? originalControl : originalControl.Clone();

        private RowControls getRowControls(int portRow)
        {
            return new RowControls()
            {
                PortLabel = cloneOrOriginal(rowPortLabel, portRow),
                Switch = cloneOrOriginal(rowSwitch, portRow),
                PortId = cloneOrOriginal(rowPortId, portRow),
                CurrentVlan = cloneOrOriginal(rowCurrentVlan, portRow),
                SetVlanTo = cloneOrOriginal(rowSetVlanTo, portRow),
                Set = cloneOrOriginal(rowSet, portRow),
                State = cloneOrOriginal(rowState, portRow)
            };
        }

        private const string CURRENT_VLAN_UNKNOWN = "unknown";

    }
}