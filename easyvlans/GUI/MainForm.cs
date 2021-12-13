using easyvlans.GUI;
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
            int portRowHeight = (int)portTable.RowStyles[1].Height;
            foreach (Port port in config.Ports)
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
                thisPortRowControls.PortId.Text = port.Index;
                thisPortRowControls.CurrentVlan.Text = CURRENT_VLAN_UNKNOWN;
                thisPortRowControls.SetVlanTo.Tag = port;
                thisPortRowControls.Set.Tag = port;
                thisPortRowControls.Set.Enabled = false;
                thisPortRowControls.State.Text = "";

                portRow++;

            }

            portRow = 0;
            foreach (Port port in config.Ports)
                portRowControls[portRow++].SetVlanTo.CreateAdapterAsDataSource(port.Vlans, vlanToStr, true, "");

            foreach (Port port in config.Ports)
            {
                port.CurrentVlanChanged += portsCurrentVlanChangedHandler;
                PortRowControls rowControls = portAssociatedRowControls[port];
                rowControls.SetVlanTo.SelectedIndexChanged += portsSetVlanToSelectedIndexChangedHandler;
                rowControls.Set.Click += portsSetButtonClickHandler;
            }

        }

        private void portsCurrentVlanChangedHandler(Port port, Vlan newValue)
            => portAssociatedRowControls[port].CurrentVlan.Text = vlanToStr(newValue);

        private void portsSetVlanToSelectedIndexChangedHandler(object sender, EventArgs e)
        {
            ComboBox typedSender = sender as ComboBox;
            Port port = typedSender?.Tag as Port;
            if (port == null)
                return;
            portAssociatedRowControls[port].Set.Enabled = (typedSender.SelectedIndex > 0);
        }

        private void portsSetButtonClickHandler(object sender, EventArgs e)
        {
            Button typedSender = sender as Button;
            Port port = typedSender?.Tag as Port;
            if (port == null)
                return;
            Vlan selectedVlan = portAssociatedRowControls[port].SetVlanTo.SelectedValue as Vlan;
            port.SetVlanTo(selectedVlan);
        }

        private string vlanToStr(Vlan vlan) => $"{vlan.ID} - {vlan.Name}";

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

        private List<PortRowControls> portRowControls = new List<PortRowControls>();
        private Dictionary<Port, PortRowControls> portAssociatedRowControls = new Dictionary<Port, PortRowControls>();

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

        private const string CURRENT_VLAN_UNKNOWN = "unknown";

    }
}
