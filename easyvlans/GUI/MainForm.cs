using easyvlans.GUI;
using easyvlans.GUI.Helpers;
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
                    table.Controls.Add(thisRowControls.Persist, 6, tableRow);
                    table.Controls.Add(thisRowControls.State, 7, tableRow);
                    Size = new Size(Size.Width, Size.Height + rowHeight);
                }

                thisRowControls.PortLabel.Text = port.Label;
                thisRowControls.Switch.Text = port.Switch.Label;
                thisRowControls.PortId.Text = port.Index;
                thisRowControls.CurrentVlan.Text = CURRENT_VLAN_UNKNOWN;
                thisRowControls.State.Text = "";

                portRow++;

            }

        }

        public class RowControls
        {
            public Label PortLabel { get; init; }
            public Label Switch { get; init; }
            public Label PortId { get; init; }
            public Label CurrentVlan { get; init; }
            public ComboBox SetVlanTo { get; init; }
            public Button Set { get; init; }
            public Button Persist { get; init; }
            public Label State { get; init; }
        }

        private List<RowControls> rowControls = new List<RowControls>();

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
                Persist = cloneOrOriginal(rowPersist, portRow),
                State = cloneOrOriginal(rowState, portRow)
            };
        }

        private const string CURRENT_VLAN_UNKNOWN = "unknown";

    }
}
