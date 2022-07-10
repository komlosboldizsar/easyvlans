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
    public partial class MainForm : Form
    {

        private Config config;
        private string parsingError;

        public MainForm() => InitializeComponent();

        public MainForm(Config config, string parsingError)
        {
            LogDispatcher.NewLogMessage += addLogMessage;
            this.config = config;
            this.parsingError = parsingError;
            Load += load;
            InitializeComponent();
        }

        private void addLogMessage(DateTime timestamp, LogMessageSeverity severity, string message)
        {
            if (!showVerboseLog.Checked && (severity == LogMessageSeverity.Verbose))
                return;
            string textToAdd = $"[{timestamp.ToString("HH:mm:ss")}] {message}\r\n";
            logTextBox.AppendText(textToAdd);
            int textLength = logTextBox.TextLength;
            int selectionLength = textToAdd.Length;
            int selectionStart = textLength - selectionLength + 1;
            if (selectionStart < 0)
            {
                selectionStart = 0;
                selectionLength = 0;
            }
            logTextBox.Select(selectionStart, selectionLength);
            logTextBox.SelectionColor = logColors[severity];
            logTextBox.Select(textLength - 1, 0);
            logTextBox.ScrollToCaret();
        }

        private void showVerboseLogCheckedChanged(object sender, EventArgs e) => reloadLogMessages();

        private void reloadLogMessages()
        {
            logTextBox.Text = "";
            foreach (LogMessage logMessage in LogDispatcher.Messages)
                addLogMessage(logMessage.Timestamp, logMessage.Severity, logMessage.Message);
        }

        private Dictionary<LogMessageSeverity, Color> logColors = new Dictionary<LogMessageSeverity, Color>()
        {
            { LogMessageSeverity.Error, Color.Red },
            { LogMessageSeverity.Warning, Color.Orange },
            { LogMessageSeverity.Info, Color.Black },
            { LogMessageSeverity.Verbose, Color.LightBlue }
        };

        private async void load(object sender, EventArgs e)
        {
            reloadLogMessages();
            string errorToShow = parsingError;
            if ((errorToShow == null) && (config == null))
                errorToShow = "Couldn't load configuration, reason unknown.";
            if (errorToShow != null)
            {
                rowPortPortLabel.Text = "N/A";
                rowPortPortLabel.ForeColor = Color.Red;
                rowPortSwitch.Text = "N/A";
                rowPortPortIndex.Text = "N/A";
                rowPortCurrentVlan.Text = "N/A";
                rowPortSetVlanTo.Enabled = false;
                rowPortSet.Enabled = false;
                rowPortState.Text = "N/A";
                rowSwitchSwitchName.Text = "N/A";
                rowSwitchSwitchName.ForeColor = Color.Red;
                rowSwitchPendingChanges.Text = "N/A";
                rowSwitchPersistChanges.Enabled = false;
                rowSwitchState.Text = "N/A";
                MessageBox.Show(errorToShow, "Initialization error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            showPages();
            createPortsTable();
            showDefaultPortPage();
            createAndShowSwitchesTable();
            Task[] allReadVlansTask = new Task[config.Switches.Count];
            int i = 0;
            foreach (Switch @switch in config.Switches.Values)
                allReadVlansTask[i++] = @switch.ReadConfigAsync();
            await Task.WhenAll(allReadVlansTask);
        }

        private void showPages()
        {
            if (config.PortPages.Count == 0)
            {
                portPageButtonContainer.Visible = false;
                return;
            }
            int portPageIndex = 0;
            foreach (UserPortPage portPage in config.PortPages)
            {
                Button newPortPageButton = (portPageIndex > 0) ? portPageButton.Clone() : portPageButton;
                newPortPageButton.Text = portPage.Title;
                newPortPageButton.Tag = portPage;
                newPortPageButton.Click += portPageButtonClick;
                if (portPageIndex > 0)
                    portPageButtonContainer.Controls.Add(newPortPageButton);
                portPageIndex++;
            }
        }

        private void createPortsTable()
        {
            if (config.Ports.Count == 0)
                return; // Todo...
            int maxPortRowCount = config.Ports.Where(p => p.Page == null).Count() + config.Ports.GroupBy(p => p.Page).Max(gp => gp.Count());
            PortRowControls.Init(this, portTable);
            PortRowControls.CreateAll(maxPortRowCount);
        }

        private void showDefaultPortPage()
            => showPortPage(config.PortPages.FirstOrDefault(pp => pp.IsDefault) ?? config.PortPages.FirstOrDefault());

        private void createAndShowSwitchesTable()
        {
            if (config.Switches.Count == 0)
                return; // Todo...
            SwitchRowControls.Init(this, switchTable);
            SwitchRowControls.CreateAll(config.Switches.Count);
            SwitchRowControls.Bind(config.Switches.Values);
        }

        private void portPageButtonClick(object sender, EventArgs e) => showPortPage(((Button)sender).Tag as UserPortPage);

        private void showPortPage(UserPortPage portPage)
        {
            foreach (Control ctrl in portPageButtonContainer.Controls)
            {
                if (ctrl is Button btn)
                {
                    bool selected = (btn.Tag == portPage);
                    btn.BackColor = selected ? Color.DarkBlue : SystemColors.Control;
                    btn.ForeColor = selected ? Color.White : SystemColors.ControlText;
                }
            }
            IEnumerable<UserPort> shownPorts = config.Ports.Where(p => ((p.Page == null) || (p.Page == portPage)));
            PortRowControls.Bind(shownPorts);
        }

    }

}
